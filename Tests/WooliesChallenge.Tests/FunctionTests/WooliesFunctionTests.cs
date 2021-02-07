using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Models;
using System.Linq;

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
            _userService.Setup(p => p.GetUser()).ReturnsAsync(targetResult);

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
        public async Task GetSortedProduct_Low_Success()
        {
            var query = new Dictionary<String, StringValues>();
            query.Add("sortOption", "Low");
            string body = "";
            int targetStatusCode = 200;
            var products = TestHelper.ReturnProductsForSort();
            string jsonText = JsonConvert.SerializeObject(products);

            _resourceService.Setup(p => p.GetProducts()).ReturnsAsync(jsonText);

            var sortProductFunction = new WooliesChallenge.Functions.WooliesFunctions(_userService.Object, _resourceService.Object);

            var result = await sortProductFunction.GetSortedProducts(req: HttpRequestSetup(query, body),
                log: _log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);
            var targetObject = resultObject.Value as List<Product>;
            var lowerPricedProduct = products.OrderBy(p => p.Price).FirstOrDefault();

            Assert.AreEqual(Convert.ToDecimal(lowerPricedProduct.Price), targetObject[0].Price);
        }


        [TestMethod]
        public async Task GetSortedProduct_High_Success()
        {
            var query = new Dictionary<String, StringValues>();
            query.Add("sortOption", "High");
            string body = "";
            int targetStatusCode = 200;
            var products = TestHelper.ReturnProductsForSort();
            string jsonText = JsonConvert.SerializeObject(products);

            _resourceService.Setup(p => p.GetProducts()).ReturnsAsync(jsonText);

            var sortProductFunction = new WooliesChallenge.Functions.WooliesFunctions(_userService.Object, _resourceService.Object);

            var result = await sortProductFunction.GetSortedProducts(req: HttpRequestSetup(query, body),
                log: _log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);
            var targetObject = resultObject.Value as List<Product>;
            var lowerPricedProduct = products.OrderByDescending(p => p.Price).FirstOrDefault();

            Assert.AreEqual(Convert.ToDecimal(lowerPricedProduct.Price), targetObject[0].Price);
        }

        [TestMethod]
        public async Task GetSortedProduct_Ascending_Success()
        {
            var query = new Dictionary<String, StringValues>();
            query.Add("sortOption", "Ascending");
            string body = "";
            int targetStatusCode = 200;
            var products = TestHelper.ReturnProductsForSort();
            string jsonText = JsonConvert.SerializeObject(products);

            _resourceService.Setup(p => p.GetProducts()).ReturnsAsync(jsonText);

            var sortProductFunction = new WooliesChallenge.Functions.WooliesFunctions(_userService.Object, _resourceService.Object);

            var result = await sortProductFunction.GetSortedProducts(req: HttpRequestSetup(query, body),
                log: _log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);
            var targetObject = resultObject.Value as List<Product>;
            var lowerPricedProduct = products.OrderBy(p => p.Name).FirstOrDefault();

            Assert.AreEqual(lowerPricedProduct.Name, targetObject[0].Name);
        }

        [TestMethod]
        public async Task GetSortedProduct_Descending_Success()
        {
            var query = new Dictionary<String, StringValues>();
            query.Add("sortOption", "Descending");
            string body = "";
            int targetStatusCode = 200;
            var products = TestHelper.ReturnProductsForSort();
            string jsonText = JsonConvert.SerializeObject(products);

            _resourceService.Setup(p => p.GetProducts()).ReturnsAsync(jsonText);

            var sortProductFunction = new WooliesChallenge.Functions.WooliesFunctions(_userService.Object, _resourceService.Object);

            var result = await sortProductFunction.GetSortedProducts(req: HttpRequestSetup(query, body),
                log: _log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);
            var targetObject = resultObject.Value as List<Product>;
            var lowerPricedProduct = products.OrderByDescending(p => p.Name).FirstOrDefault();

            Assert.AreEqual(lowerPricedProduct.Name, targetObject[0].Name);
        }

        [TestMethod]
        public async Task GetSortedProduct_SortRecommended_Success()
        {
            var query = new Dictionary<String, StringValues>();
            query.Add("sortOption", "Recommended");
            string body = "";
            int targetStatusCode = 200;
            var shopperHistory = TestHelper.ReturnProductsForSortRecommended();
            string jsonText = JsonConvert.SerializeObject(shopperHistory);

            _resourceService.Setup(p => p.GetShopperHistory()).ReturnsAsync(jsonText);

            var sortProductFunction = new WooliesChallenge.Functions.WooliesFunctions(_userService.Object, _resourceService.Object);

            var result = await sortProductFunction.GetSortedProducts(req: HttpRequestSetup(query, body),
                log: _log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);
            var targetObject = resultObject.Value as List<Product>;

            Assert.AreEqual(shopperHistory[0].Products[0].Name, targetObject[0].Name);
        }

        [TestMethod]
        public async Task GetTrolleyCalculation_Success()
        {
            var query = new Dictionary<String, StringValues>();
            string body = JsonConvert.SerializeObject(TestHelper.ReturnTrolleyInput());
            int targetStatusCode = 200;
            var shopperHistory = TestHelper.ReturnProductsForSortRecommended();
            decimal targetTotal = 134m;
            string jsonText = JsonConvert.SerializeObject(targetTotal);

            _resourceService.Setup(p => p.GetTrolleyCalculation(body)).ReturnsAsync(jsonText);

            var trolleyCalculationFunction = new WooliesChallenge.Functions.WooliesFunctions(_userService.Object, _resourceService.Object);

            var result = await trolleyCalculationFunction.GetTrolleyCalculation(req: HttpRequestSetup(query, body),
                log: _log.Object);
            var resultObject = (OkObjectResult)result;
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultObject.StatusCode, targetStatusCode);
            Assert.AreEqual(targetTotal, Convert.ToDecimal(resultObject.Value));
        }
    }
}
