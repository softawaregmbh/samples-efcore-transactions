using System;
using static System.Console;

namespace Sample.Domain
{
    public static class Console
    {
        public static void WriteLine(string message, ConsoleColor color)
        {
            var previousColor = ForegroundColor;
            ForegroundColor = color;
            System.Console.WriteLine(message);
            ForegroundColor = previousColor;
        }
    }
}
