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

namespace WooliesChallenge.Tests.FunctionTests
{
    [TestClass]
    public class WooliesFunctionTests
    {
        private MockRepository _repository;
        private Mock<IUserService> _userService;
        private Mock<IResourceService> _resourceService;
        private Mock<Microsoft.Extensions.Logging.ILogger> _log;

        [TestInitialize]
        public void SetUp()
        {
            _repository = new MockRepository(MockBehavior.Default);
            _userService = _repository.Create<IUserService>();
            _log = _repository.Create<Microsoft.Extensions.Logging.ILogger>();
            _resourceService = _repository.Create<IResourceService>();
        }
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
            var targetResult = TestHelper.ReturnUser();
            _userService.Setup(p => p.GetUser()).Returns(targetResult);

            var userServiceFunction = new WooliesChallenge.Functions.WooliesFunctions(_userService.Object, _resourceService.Object);

            var result = await userServiceFunction.GetUser(req: HttpRequestSetup(query, body),
                log: _log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);

            var userExpected = resultObject.Value as User;
            Assert.AreEqual(targetResult.Name, userExpected.Name);
        }

        [TestMethod]
        public async Task GetSortedProduct_High_Success()
        {
            var query = new Dictionary<String, StringValues>();
            query.Add("sortOption", "High");
            string body = "";
            int targetStatusCode = 200;
            string jsonText = @"[{""name"": ""Test Product A"",""price"": 99.99,""quantity"": 0 },
                                   {""name"": ""Test Product B"",""price"": 101.99,""quantity"": 0}]";

            _resourceService.Setup(p => p.GetProducts()).ReturnsAsync(jsonText);

            var sortProductFunction = new WooliesChallenge.Functions.WooliesFunctions(_userService.Object, _resourceService.Object);

            var result = await sortProductFunction.GetSortedProducts(req: HttpRequestSetup(query, body),
                log: _log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);
            var targetObject = resultObject.Value as List<Product>;
            Assert.AreEqual(Convert.ToDecimal(101.99), targetObject[0].Price);
        }
    }
}
