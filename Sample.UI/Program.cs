using System;
using System.Linq;
using System.Threading.Tasks;
using Sample.BusinessLogic;
using Sample.EntityFramework;

namespace Sample.UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var manager = new ItemManager(TransactionAwareContextFactory.CreateContext);
            var logic = new ItemLogic(manager);

            Console.WriteLine("Use the following commands:");
            Console.WriteLine();
            Console.WriteLine("\"on\": Enable transactions");
            Console.WriteLine("\"off\": Disable transactions");
            Console.WriteLine("\"list\": List items in database");
            Console.WriteLine("\"clear\": Remove all items");
            Console.WriteLine();
            Console.WriteLine("All other commands will be interpreted as one or more item names that should be added to the database.");

            while (true)
            {
                Console.WriteLine();
                var command = Console.ReadLine();

                switch (command)
                {
                    case "on":
                        logic.UseTransactions = true;
                        Console.WriteLine("Transactions are ENABLED");
                        break;

                    case "off":
                        logic.UseTransactions = false;
                        Console.WriteLine("Transactions are DISABLED");
                        break;

                    case "list":
                        Console.WriteLine(string.Join(' ', await manager.GetItemsAsync()));
                        break;

                    case "clear":
                        await manager.ClearItemsAsync();
                        Console.WriteLine("Done.");
                        break;

                    default:
                        try
                        {
                            var names = command.Split(' ');
                            await logic.AddItemsAsync(names);
                            Console.WriteLine($"Added {names.Length} item(s) to the database");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error adding items:");
                            Console.WriteLine(e);
                        }

                        break;
                }
            }
        }
    }
}
