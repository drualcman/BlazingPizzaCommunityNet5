using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingPizza.Shared
{
    public class PizzaTopping
    {
        public Topping Topping { get; set; }
        public int ToppingId { get; set; }
        public int PizzaId { get; set; }
    }

}
