using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WooliesChallenge.Application.Services;

namespace WooliesChallenge.Tests.ApplicationTests
{
    [TestClass]
    public class ResourceServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),AllowDerivedTypes = false)]
        public async Task GetProducts_HttpClient_Is_Null()
        {
            var resourceService = new ResourceService(null);
            var result = await resourceService.GetProducts();
        }

        [TestMethod]
        public async Task GetProducts_Success()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(TestHelper.ReturnProducts());
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://dev-wooliesx-recruitment.azurewebsites.net/"),
            };
            var resourceService = new ResourceService(httpClient); 

            var result = await resourceService.GetProducts();
            Assert.IsNotNull(result);

            var productsResp = result as string;
            Assert.IsTrue(!string.IsNullOrEmpty(productsResp));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = false)]
        public async Task GetShopperHistory_HttpClient_Is_Null()
        {
            var resourceService = new ResourceService(null);
            var result = await resourceService.GetShopperHistory();
        }

        [TestMethod]
        public async Task GetShopperHistory_Success()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(TestHelper.ReturnShopperHistory());
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://dev-wooliesx-recruitment.azurewebsites.net/"),
            };
            var resourceService = new ResourceService(httpClient);

            var result = await resourceService.GetShopperHistory();
            Assert.IsNotNull(result);

            var shopperHistoryResp = result as string;
            Assert.IsTrue(!string.IsNullOrEmpty(shopperHistoryResp));
        }

     
    }
}
