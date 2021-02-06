using System.Collections.Generic;

namespace WooliesChallenge.Application.Models
{
    public class ShopperHistory
    {
        public long CustomerId { get; set; }
        public List<Product> Products { get; set; }
    }
}
