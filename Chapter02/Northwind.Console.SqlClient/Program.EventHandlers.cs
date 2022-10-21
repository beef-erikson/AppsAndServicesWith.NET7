// <copyright file="Program.EventHandlers.cs" company="Beef Erikson Studios">
// Copyright (c) Beef Erikson Studios. All rights reserved.
// </copyright>
using System.Data;
using Microsoft.Data.SqlClient;

namespace Northwind.Console.SqlClient
{
    /// <summary>
    /// Partial class for Program that defines event handler methods.
    /// </summary>
    internal partial class Program
    {
        /// <summary>
        /// Database connection state change.
        /// </summary>
        /// <param name="sender">object of sender.</param>
        /// <param name="e">event args.</param>
        internal static void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            ConsoleColor previousColor = ForegroundColor;
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine($"State change from {e.OriginalState} to {e.CurrentState}.");
            ForegroundColor = previousColor;
        }

        /// <summary>
        /// Writes connection info and any sql errors.
        /// </summary>
        /// <param name="sender">object of sender.</param>
        /// <param name="e">event args.</param>
        internal static void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            ConsoleColor previousColor = ForegroundColor;
            ForegroundColor = ConsoleColor.DarkBlue;
            WriteLine($"Info: {e.Message}.");
            foreach (SqlError error in e.Errors)
            {
                WriteLine($"   Error: {error.Message}.");
            }

            ForegroundColor = previousColor;
        }
    }
}
