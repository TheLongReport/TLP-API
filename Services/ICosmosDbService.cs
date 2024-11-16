using System.Threading.Tasks;

namespace TLP_API.Services
{
    public interface ICosmosDbService
    {
        Task AddItemAsync<T>(T item);
    }
}
