using System;
using System.Threading.Tasks;
using Sample.BusinessLogic;
using Sample.EntityFramework;
using static System.Console;
using static Sample.Domain.Console;

namespace Sample.UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var manager = new ItemManager(TransactionAwareContextFactory.CreateContext);
            var logic = new ItemLogic(manager);

            WriteLine("Use the following commands:");
            WriteLine();
            WriteLine("\"on\": Enable transactions");
            WriteLine("\"off\": Disable transactions");
            WriteLine("\"list\": List items in database");
            WriteLine("\"clear\": Remove all items");
            WriteLine();
            WriteLine("All other commands will be interpreted as one or more item names that should be added to the database.");

            while (true)
            {
                WriteLine();
                var command = ReadLine();

                switch (command)
                {
                    case "on":
                        logic.UseTransactions = true;
                        WriteLine("Transactions are ENABLED.");
                        break;

                    case "off":
                        logic.UseTransactions = false;
                        WriteLine("Transactions are DISABLED.");
                        break;

                    case "list":
                        WriteLine(string.Join(' ', await manager.GetItemsAsync()));
                        break;

                    case "clear":
                        await manager.ClearItemsAsync();
                        WriteLine("Done.");
                        break;

                    default:
                        try
                        {
                            var names = command.Split(' ');
                            await logic.AddItemsAsync(names);
                            WriteLine($"Added {names.Length} item(s) to the database");
                        }
                        catch (Exception e)
                        {
                            WriteLine("Error adding items:");
                            WriteLine("  " + e.Message, ConsoleColor.DarkGray);
                        }

                        break;
                }
            }
        }
    }
}
