// <copyright file="Program.cs" company="Beef Erikson Studios">
// Copyright (c) Beef Erikson Studios. All rights reserved.
// </copyright>
using System.Data;
using Microsoft.Data.SqlClient;

// Sets builder and parameters.
SqlConnectionStringBuilder builder = new ();

builder.InitialCatalog = "Northwind";
builder.MultipleActiveResultSets = true;
builder.Encrypt = true;
builder.TrustServerCertificate = true;
builder.ConnectTimeout = 10;

// Welcome screen.
WriteLine("Connect to:");
WriteLine("  1 - SQL Server on local machine");
WriteLine("  2 - Azure SQL Database");
WriteLine("  3 - Azure SQL Edge");
WriteLine();
Write("Press a key: ");

ConsoleKey key = ReadKey().Key;
WriteLine();
WriteLine();

// Key has been hit.
if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    builder.DataSource = "."; // local SQL server
}
else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    builder.DataSource = "tcp:apps-services-net7.database.windows.net,1433"; // Azure SQL Database
}
else if (key is ConsoleKey.D3 or ConsoleKey.NumPad3)
{
    builder.DataSource = "tcp:120.0.0.1,1433"; // Azure SQL Edge
}
else
{
    WriteLine("No data source selected.");
    return;
}

// Authenticate screen.
WriteLine("Authenticate using:");
WriteLine("  1 - Windows Integrated Security");
WriteLine("  2 - SQL Login, for example, sa");
WriteLine();
Write("Press a key: ");

key = ReadKey().Key;
WriteLine();
WriteLine();

// Key has been hit.
if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    builder.IntegratedSecurity = true; // Windows Integrated Security
}
else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    builder.UserID = "sa"; // Azure SQL Edge - Change this to your username

    Write("Enter your SQL Server password: ");
    string? password = ReadLine();
    if (string.IsNullOrWhiteSpace(password))
    {
        WriteLine("Password cannot be empty or null.");
        return;
    }

    builder.Password = password;
    builder.PersistSecurityInfo = false;
}
else
{
    WriteLine("No authentication selected.");
    return;
}

// Initialize conntection.
SqlConnection connection = new (builder.ConnectionString);

WriteLine(connection.ConnectionString);
WriteLine();

// Change state and display info message.
connection.StateChange += Connection_StateChange;
connection.InfoMessage += Connection_InfoMessage;

// Open connection.
try
{
    WriteLine(
        "Opening connection. Please wait up to {0} seconds...",
        builder.ConnectTimeout);
    WriteLine();
    connection.Open();

    WriteLine($"SQL Server version: {connection.ServerVersion}");

    connection.StatisticsEnabled = true;
}
catch (SqlException ex)
{
    WriteLine($"SQL exception: {ex.Message}");
}

// Filter out by unit price.
Write("Enter a unit price: ");
string? priceText = ReadLine();

if (!decimal.TryParse(priceText, out decimal price))
{
    WriteLine("You must enter a valid unit price.");
    return;
}

// Selects ID, name and price from products table.
SqlCommand cmd = connection.CreateCommand();

cmd.CommandType = CommandType.Text;
cmd.CommandText = "SELECT ProductId, ProductName, UnitPrice FROM Products" +
    " WHERE UnitPrice > @price";
cmd.Parameters.AddWithValue("price", price);

SqlDataReader reader = cmd.ExecuteReader();

// Print output.
WriteLine("----------------------------------------------------------");
WriteLine("| {0,5} | {1,-35} | {2,8} |", "Id", "Name", "Price");
WriteLine("----------------------------------------------------------");

while (reader.Read())
{
    WriteLine(
        "| {0,5} | {1,-35} | {2,8:C} |",
        reader.GetInt32("ProductID"),
        reader.GetString("ProductName"),
        reader.GetDecimal("UnitPrice"));
}

WriteLine("----------------------------------------------------------");

// Clear resources.
reader.Close();

connection.Close();