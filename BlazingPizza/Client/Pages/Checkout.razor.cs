using BlazingPizza.Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Pages
{
    public partial class Checkout
    {
        [Inject]
        public OrderState OrderState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }


        #region manejadores de eventos
        async Task PlaceOrder()
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("orders", OrderState.Order);
            int NewOrderID = await response.Content.ReadFromJsonAsync<int>();
            OrderState.ResetOrder();
            NavigationManager.NavigateTo($"myorders/{NewOrderID}");
        }
        #endregion
    }
}
