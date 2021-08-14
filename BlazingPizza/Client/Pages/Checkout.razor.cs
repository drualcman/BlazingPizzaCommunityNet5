using BlazingPizza.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Pages
{
    [Authorize]
    public partial class Checkout
    {
        [Inject]
        public OrderState OrderState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public OrdersClient OrdersClient { get; set; }

        bool Clicked;
        #region manejador de eventos
        async Task PlaceOrder()
        {
            if (!Clicked)
            {
                Clicked = true;
                try
                {
                    int NewOrderID = await OrdersClient.PlaceOrder(OrderState.Order);
                    OrderState.ResetOrder();
                    NavigationManager.NavigateTo($"myorders/{NewOrderID}");
                }
                catch (AccessTokenNotAvailableException ex)
                {
                    ex.Redirect();
                }
                finally
                {
                    Clicked = false;
                }
            }
        }
        #endregion
    }
}
