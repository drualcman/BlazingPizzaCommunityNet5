using BlazingPizza.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Services
{
    public class OrderState
    {
        public Pizza ConfiguringPizza { get; private set; }
        public bool ShowingConfigureDialog { get; private set; }
        public Order Order { get; private set; } = new();


        #region Metodos
        public void ShowConfigurePizzaDialog(PizzaSpecial special)
        {
            ConfiguringPizza = new()
            {
                Special = special,
                SpecialId = special.Id,
                Size = Pizza.DefaultSize,
                Toppings = new()
            };
            ShowingConfigureDialog = true;
        }
        #endregion

        #region manejadores de eventos
        public void CancelConfigurePizzaDialog()
        {
            ConfiguringPizza = null;
            ShowingConfigureDialog = false;
        }

        public void ConfirmConfigurePizzaDialog()
        {
            Order.Pizzas.Add(ConfiguringPizza);
            ConfiguringPizza = null;
            ShowingConfigureDialog = false;
        }

        public void RemoveConfiguredPizza(Pizza pizza)
        {
            Order.Pizzas.Remove(pizza);
        }

        public void ResetOrder()
        {
            Order = new Order();
        }
        #endregion
    }
}
