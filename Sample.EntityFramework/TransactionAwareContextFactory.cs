using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

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
                Console.WriteLine(" - Trying to get connection for transaction " + currentTransaction.TransactionInformation.LocalIdentifier);
                var connectionForTransaction = connections.GetOrAdd(currentTransaction, valueFactory: t =>
                {
                    Console.WriteLine("  - Creating connection for transaction " + t.TransactionInformation.LocalIdentifier);

                    var connection = new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=Sample.db;Trusted_Connection=True;");
                    connection.Open();

                    t.TransactionCompleted += (s, e) =>
                    {
                        connection.Close();
                        Console.WriteLine("  - Connection closed for transaction " + t.TransactionInformation.LocalIdentifier);
                    };

                    return connection;
                });

                return new Context(connectionForTransaction);
            }
        }
    }
}
