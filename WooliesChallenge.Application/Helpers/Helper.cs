
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WooliesChallenge.Application.Models;

namespace WooliesChallenge.Application.Helpers
{
    public class Helper
    {
        public static List<Product> SortProducts(string resourceResponse, SortOption sortOption)
        {
            List<Product> lstProduct = null;
            if (sortOption == SortOption.Recommended)
            {
                var respShooperHistory = JsonConvert.DeserializeObject<List<ShopperHistory>>(resourceResponse);
                lstProduct = SortRecommendedProducts(respShooperHistory);
            }
            else
            {
                var respProduct = JsonConvert.DeserializeObject<List<Product>>(resourceResponse);
                lstProduct = Sort(respProduct, sortOption);
            }
            return lstProduct;
        }
        public static SortOption GetEnumValue(string sortOptionValue)
        {
            SortOption retSortOption = SortOption.Low;
            bool isSortOptionParsed = string.IsNullOrEmpty(sortOptionValue) ? false : Enum.TryParse<SortOption>(sortOptionValue, true, out retSortOption);
            return retSortOption;
        }
        public static string SerializeInput<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }
        public static T DeSerializeInput<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
        public static async Task<string> ReadRequestBody(Stream inputStream)
        {
            return await new StreamReader(inputStream).ReadToEndAsync(); ;
        }
        private static List<Product> Sort(List<Product> products, SortOption sortOption)
        {
            if (products?.Count == 0) throw new ArgumentNullException("products");
            List<Product> result;
            switch (sortOption)
            {
                case SortOption.Low:
                    result = products.OrderBy(o => o.Price).ToList();
                    break;
                case SortOption.High:
                    result = products.OrderByDescending(o => o.Price).ToList();
                    break;
                case SortOption.Ascending:
                    result = products.OrderBy(o => o.Name).ToList();
                    break;
                case SortOption.Descending:
                    result = products.OrderByDescending(o => o.Name).ToList();
                    break;
                default:
                    result = products;
                    break;
            }
            return result;
        }

        private static List<Product> SortRecommendedProducts(List<ShopperHistory> lstShopperHistory)
        {
            var productList = new List<Product>();
            if(lstShopperHistory?.Count == 0) throw new ArgumentNullException("ShopperHistory");

            var dictProducts = new Dictionary<string,ProductSold>();
            foreach (ShopperHistory shopperHistory in lstShopperHistory)
            {
                foreach (Product product in shopperHistory.Products)
                {
                    if (dictProducts.ContainsKey(product.Name))
                    {
                        dictProducts[product.Name].Product.Quantity += product.Quantity;
                        dictProducts[product.Name].ProductSoldCount++;
                    }
                    else
                    {
                        dictProducts.Add(product.Name, new ProductSold { Product = product,ProductSoldCount =1});
                    }
                }
            }
            return dictProducts.Values.ToList().OrderByDescending(p => p.ProductSoldCount).ThenByDescending(p => p.Product.Quantity).Select(p => p.Product).ToList();
        }
    }
}
