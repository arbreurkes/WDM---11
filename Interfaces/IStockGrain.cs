using DataModels;
using Orleans;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStockGrain : IGrainWithGuidKey
    {
        [Transaction(TransactionOption.CreateOrJoin)]
        Task<Stock> GetStock();

        [Transaction(TransactionOption.CreateOrJoin)]
        Task ChangeAmount(int amount);

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<int> GetAmount();
        [Transaction(TransactionOption.CreateOrJoin)]
        Task<Stock> Create(decimal price);
    }
}
