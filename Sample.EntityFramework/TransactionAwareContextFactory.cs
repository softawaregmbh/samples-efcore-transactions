using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using static Sample.Domain.Console;

namespace Sample.EntityFramework
{
    public static class TransactionAwareContextFactory
    {
        private static ConcurrentDictionary<Transaction, DbConnection> connections = new ConcurrentDictionary<Transaction, DbConnection>();        

        public static Context CreateContext()
        {
            var currentTransaction = Transaction.Current;
            if (currentTransaction == null)
            {
                return new Context();
            }
            else
            {
                WriteLine("  - Trying to find connection for transaction " + currentTransaction, ConsoleColor.DarkGray);

                var connectionForTransaction = connections.GetOrAdd(currentTransaction, valueFactory: t =>
                {
                    WriteLine("    - Creating connection for transaction " + t.TransactionInformation.LocalIdentifier, ConsoleColor.DarkGray);

                    var connection = new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=Sample.db;Trusted_Connection=True;");
                    connection.Open();

                    t.TransactionCompleted += (s, e) =>
                    {
                        connection.Close();
                        WriteLine("    - Connection closed for transaction " + t.TransactionInformation.LocalIdentifier, ConsoleColor.DarkGray);
                    };

                    return connection;
                });

                return new Context(connectionForTransaction);
            }
        }
    }
}
