using BlazingPizza.Client.Services;
using BlazingPizza.Shared;
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
    public partial class MyOrders
    {
        [Inject]
        public OrdersClient OrdersClient { get; set; }

        async Task<List<OrderWithStatus>> LoaderOrders()
        {
            List<OrderWithStatus> orderWithStatus = new List<OrderWithStatus>();
            try
            {
                orderWithStatus = await OrdersClient.GetOrders();
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
            }
            return orderWithStatus;
        }
    }
}
