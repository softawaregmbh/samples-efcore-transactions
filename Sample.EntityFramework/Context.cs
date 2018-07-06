using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Sample.Domain;

namespace Sample.EntityFramework
{
    public class Context : DbContext
    {
        private readonly DbConnection connection;

        public Context()
        {
        }

        public Context(DbConnection connection)
        {
            this.connection = connection;
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (this.connection != null)
            {
                optionsBuilder.UseSqlServer(this.connection);
            }
            else
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Sample.db;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().HasKey(i => i.Name);
        }
    }
}
