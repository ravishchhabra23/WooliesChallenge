using System.Threading.Tasks;

namespace WooliesChallenge.Application.Interfaces
{
    public interface IResourceService
    {
        Task<string> GetProducts();
        Task<string> GetShopperHistory();
    }
}
