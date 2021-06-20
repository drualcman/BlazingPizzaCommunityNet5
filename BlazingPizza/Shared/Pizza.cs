using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingPizza.Shared
{
    public class Pizza
    {
        public const int DefaultSize = 30;
        public const int MinimumSize = 20;
        public const int MaximumSize = 40;
        public const int IncrementSize = 2;

        public int Id { get; set; }
        public int OrderId { get; set; }
        public PizzaSpecial Special { get; set; }
        public int SpecialId { get; set; }
        public int Size { get; set; }
        public List<PizzaTopping> Toppings { get; set; }
        public decimal GetBasePrice()
        {
            return ((decimal)Size / (decimal)DefaultSize) * Special.BasePrice;
        }
        public decimal GetTotalPrice()
        {
            return GetBasePrice() + Toppings.Sum(t => t.Topping.Price);
        }

        public string GetFormattedBasePrice()
        {
            return GetBasePrice().ToString("0.00");
        }
        public string GetFormattedTotalPrice()
        {
            return GetTotalPrice().ToString("0.00");
        }
    }

}
