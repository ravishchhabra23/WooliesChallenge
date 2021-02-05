using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Models;
using WooliesChallenge.Application.Helpers;
using Newtonsoft.Json;

namespace WooliesChallenge.Tests.FunctionTests
{
    [TestClass]
    public class WooliesFunctionTests
    {
        private HttpRequest HttpRequestSetup(Dictionary<String, StringValues> query, string body)
        {
            var reqMock = new Mock<HttpRequest>();

            reqMock.Setup(req => req.Query).Returns(new QueryCollection(query));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            reqMock.Setup(req => req.Body).Returns(stream);
            return reqMock.Object;
        }

        [TestMethod]
        public async Task GetUser_Success()
        {
            var query = new Dictionary<String, StringValues>();
            string body = "";
            int targetStatusCode = 200;
            var targetResult = ReturnUser();

            MockRepository repository = new MockRepository(MockBehavior.Default);
            var userService = repository.Create<IUserService>();
            userService.Setup(p => p.GetUser()).Returns(ReturnUser());

            var log = repository.Create<Microsoft.Extensions.Logging.ILogger>();
            var userServiceFunction = new WooliesChallenge.Functions.WooliesFunctions(userService.Object);

            var result = await userServiceFunction.GetUser(req: HttpRequestSetup(query, body),
                log: log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);

            var userExpected = resultObject.Value as User;
            Assert.AreEqual(targetResult.Name, userExpected.Name);
        }

        private User ReturnUser()
        {
            return new User { Name = Constants.Name, Token = Constants.Token };
        }
    }
}
