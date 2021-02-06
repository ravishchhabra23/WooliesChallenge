
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static SortOption GetEnumValue(string sortOptionValue)
        {
            SortOption retSortOption = SortOption.Low;
            bool isSortOptionParsed = string.IsNullOrEmpty(sortOptionValue) ? false : Enum.TryParse<SortOption>(sortOptionValue, true, out retSortOption);
            return retSortOption;
        }
    }
}
