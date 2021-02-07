using System.Collections.Generic;

namespace WooliesChallenge.Application.Models
{
    public class TrolleyCalculationModel
    {
        public List<Product> Products { get; set; }
        public List<Special> Specials { get; set; }
        public List<Quantity> Quantities { get; set; }
    }
}
