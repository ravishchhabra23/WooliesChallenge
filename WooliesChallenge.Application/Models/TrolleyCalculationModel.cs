using System;
using System.Collections.Generic;
using System.Text;

namespace WooliesChallenge.Application.Models
{
    public class TrolleyCalculationModel
    {
        public List<Product> Products { get; set; }
        public List<Special> Specials { get; set; }
        public List<Quantity> Quantities { get; set; }
    }
}
