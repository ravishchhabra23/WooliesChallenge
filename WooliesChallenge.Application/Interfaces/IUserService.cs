using System.Threading.Tasks;
using WooliesChallenge.Application.Models;

namespace WooliesChallenge.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser();
    }
}
