using Microsoft.VisualStudio.TestTools.UnitTesting;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Models;
using WooliesChallenge.Application.Helpers;
using WooliesChallenge.Application.Services;

namespace WooliesChallenge.Tests
{
    [TestClass]
    public class GetUser
    {
        [TestMethod]
        public void GetUser_Success()
        {
            User target = ReturnUser();

            IUserService userService = new UserService();
            var expected = userService.GetUser();

            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.Name, target.Name);
        }
        private User ReturnUser()
        {
            return new User { Name = Constants.Name, Token = Constants.Token };
        }

    }
}
