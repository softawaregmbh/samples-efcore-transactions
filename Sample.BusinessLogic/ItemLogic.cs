using System;
using System.Threading.Tasks;
using System.Transactions;
using Sample.Domain;

namespace Sample.BusinessLogic
{
    public class ItemLogic
    {
        private readonly IItemManager manager;

        public ItemLogic(IItemManager manager)
        {
            this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        public bool UseTransactions { get; set; }

        public async Task AddItemsAsync(params string[] names)
        {
            if (UseTransactions)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await this.AddItemsCoreAsync(names);
                    transaction.Complete();
                }
            }
            else
            {
                await this.AddItemsCoreAsync(names);
            }
        }

        private async Task AddItemsCoreAsync(string[] names)
        {
            foreach (var name in names)
            {
                await this.manager.AddItemAsync(new Item() { Name = name });
            }
        }
    }
}
