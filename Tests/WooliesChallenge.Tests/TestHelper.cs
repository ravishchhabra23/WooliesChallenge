using WooliesChallenge.Application.Models;
using WooliesChallenge.Application.Helpers;
using System.Net.Http;
using System.Net;

namespace WooliesChallenge.Tests
{
    internal static class TestHelper
    {
        internal static User ReturnUser()
        {
            return new User { Name = Constants.Name, Token = Constants.Token };
        }
        internal static HttpResponseMessage ReturnProducts()
        {
            string jsonText = @"[{""name"": ""Test Product A"",""price"": 99.99,""quantity"": 0 },
                                   {""name"": ""Test Product B"",""price"": 101.99,""quantity"": 0}]";
            var content = new StringContent(jsonText);
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content };
        }
        internal static HttpResponseMessage ReturnShopperHistory()
        {
            string jsonText = @"[{""customerId"": 123,""products"":[{ ""name"": ""Test Product A"",""price"": 99.99,""quantity"": 0 },
                                   {""name"": ""Test Product B"",""price"": 101.99,""quantity"": 0}]}]";
            var content = new StringContent(jsonText);
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content };
        }
    }
}
