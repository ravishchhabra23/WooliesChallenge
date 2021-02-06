using Microsoft.VisualStudio.TestTools.UnitTesting;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Models;
using WooliesChallenge.Application.Services;

namespace WooliesChallenge.Tests
{
    [TestClass]
    public class GetUserTests
    {
        [TestMethod]
        public void GetUser_Success()
        {
            User target = TestHelper.ReturnUser();

            IUserService userService = new UserService();
            var expected = userService.GetUser();

            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.Name, target.Name);
        }
    }
}
