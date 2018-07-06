using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Domain;

namespace Sample.Domain
{
    public interface IItemManager
    {
        Task AddItemAsync(Item item);
        Task<IReadOnlyCollection<Item>> GetItemsAsync();
        Task ClearItemsAsync();
    }
}