using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sample.Domain;

namespace Sample.EntityFramework
{
    public class ItemManager : IItemManager
    {
        private readonly Func<Context> contextFactory;

        public ItemManager(Func<Context> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task AddItemAsync(Item item)
        {
            using (var context = this.contextFactory())
            {
                context.Items.Add(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyCollection<Item>> GetItemsAsync()
        {
            using (var context = this.contextFactory())
            {
                return await context.Items.OrderBy(i => i.Name).ToListAsync();
            }
        }

        public async Task ClearItemsAsync()
        {
            using (var context = this.contextFactory())
            {
                await context.Database.ExecuteSqlCommandAsync("DELETE FROM Items");
            }
        }
    }
}
