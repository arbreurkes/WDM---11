using DataModels;
using Orleans;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStockGrain : IGrainWithGuidKey
    {
        Task<Stock> GetStock();

        Task ChangeAmount(int amount);
        Task<int> GetAmount();
        Task<Stock> Create(decimal price);
    }
}
