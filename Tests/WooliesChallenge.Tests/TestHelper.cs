using WooliesChallenge.Application.Models;
using WooliesChallenge.Application.Helpers;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;

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
        internal static List<Product> ReturnProductsForSort()
        {
            return new List<Product> {
                new Product{ Name = "Test Product A", Price = 109.8m, Quantity = 2 },
                new Product{ Name = "Test Product B", Price = 98.8m, Quantity = 1 },
                new Product{ Name = "Test Product C", Price = 209.8m, Quantity = 3 },
                new Product{ Name = "Test Product D", Price = 79.8m, Quantity = 1 },
            };
            
        }
        internal static List<ShopperHistory> ReturnProductsForSortRecommended()
        {
            return new List<ShopperHistory> {
                new ShopperHistory {
                    CustomerId = 1, Products = new List<Product>{
                        new Product{ Name = "Test Product A", Price = 109.8m, Quantity = 1 },
                        new Product{ Name = "Test Product B", Price = 98.8m, Quantity = 1 },
                        new Product{ Name = "Test Product C", Price = 209.8m, Quantity = 10 },
                        new Product{ Name = "Test Product D", Price = 79.8m, Quantity = 1 }
                    }
                },
                new ShopperHistory {
                    CustomerId = 2, Products = new List<Product>{
                        new Product{ Name = "Test Product A", Price = 104.8m, Quantity = 1 },
                        new Product{ Name = "Test Product H", Price = 90.8m, Quantity = 5 },
                        new Product{ Name = "Test Product G", Price = 176.8m, Quantity = 7 },
                        new Product{ Name = "Test Product A", Price = 59.8m, Quantity = 1 }
                    }
                },
                new ShopperHistory {
                    CustomerId = 3, Products = new List<Product>{
                        new Product{ Name = "Test Product A", Price = 57m, Quantity = 1 },
                        new Product{ Name = "Test Product F", Price = 65.2m, Quantity = 3 },
                        new Product{ Name = "Test Product G", Price = 65.7m, Quantity = 2 },
                        new Product{ Name = "Test Product H", Price = 76.4m, Quantity = 2 }
                    }
                },

            };

        }
    }
}
