using BlazingPizza.Client.Services;
using BlazingPizza.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Pages
{
    public partial class OrderDetails : IDisposable
    {
        [Inject]
        public OrdersClient OrdersClient { get; set; }

        [Parameter]
        public int OrderId { get; set; }

        OrderWithStatus OrderWithStatus;
        bool InvalidOrder;

        CancellationTokenSource PollingCancellatioinToken;

        private async void PoolForUpdates()
        {
            PollingCancellatioinToken = new CancellationTokenSource();
            while (!PollingCancellatioinToken.IsCancellationRequested)
            {
                try
                {
                    InvalidOrder = false;
                    OrderWithStatus = await OrdersClient.GetOrder(OrderId);
                    StateHasChanged();
                    if (OrderWithStatus.IsDelivered)
                    {
                        PollingCancellatioinToken.Cancel();
                    }
                    else
                    {
                        await Task.Delay(4000);
                    }
                }
                catch (AccessTokenNotAvailableException ex)
                {
                    PollingCancellatioinToken?.Cancel();
                    ex.Redirect();
                }
                catch (Exception ex)
                {
                    InvalidOrder = true;
                    PollingCancellatioinToken.Cancel();
                    Console.Error.WriteLine(ex.Message);
                    StateHasChanged();
                }
            }
        }

        protected override void OnParametersSet()
        {
            PollingCancellatioinToken?.Cancel();
            PoolForUpdates();
        }

        public void Dispose()
        {
            PollingCancellatioinToken?.Cancel();
        }
    }
}
