// <copyright file="Program.cs" company="Beef Erikson Studios">
// Copyright (c) Beef Erikson Studios. All Rights Reserved.
// </copyright>

namespace CodeAnalyzing.src;

using System.Diagnostics;
using static System.Console;

/// <summary>
/// The main class for this console app.
/// </summary>
public class Program
{
    /// <summary>
    /// The main entry point for this console app.
    /// </summary>
    /// <param name="args">
    /// A string array of arguments passed to the console app.
    /// </param>
    public static void Main(string[] args)
    {
        string csharp11 = """
        {
            "firstName": "Bob",
            "lastName": "Smith"
        }
        """;

        WriteLine($"{csharp11}");
    }
}