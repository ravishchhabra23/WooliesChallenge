using WooliesChallenge.Application.Helpers;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Models;

namespace WooliesChallenge.Application.Services
{
    public class UserService: IUserService
    {
        public User GetUser()
        {
            return new User {Name = Constants.Name, Token = Constants.Token };
        }
    }
}
