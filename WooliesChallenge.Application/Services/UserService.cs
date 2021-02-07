using System.Threading.Tasks;
using WooliesChallenge.Application.Helpers;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Models;

namespace WooliesChallenge.Application.Services
{
    public class UserService: IUserService
    {
        public async Task<User> GetUser()
        {
            return await Task.FromResult(new User {Name = Constants.Name, Token = Constants.Token });
        }
    }
}
