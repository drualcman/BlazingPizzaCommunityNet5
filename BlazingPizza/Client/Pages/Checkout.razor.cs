using BlazingPizza.Client.Services;
using BlazingPizza.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
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

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        bool Clicked;

        //protected override void OnInitialized()
        //{
        //    //preguntar al usuario si desea ser notificado con las actualizaciones de la order
        //    _ = RequestNotificationsSubscriptionAsync();
        //}

        protected override async Task OnInitializedAsync()
        {
            await RequestNotificationsSubscriptionAsync();
        }

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

        async Task RequestNotificationsSubscriptionAsync()
        {
            NotificationSubscription subscription = await JSRuntime.InvokeAsync<NotificationSubscription>("blazorPushNotifications.requestSubscription");
            if (subscription is not null)
            {
                try
                {
                    await OrdersClient.SubscribeToNotification(subscription);
                }
                catch (AccessTokenNotAvailableException ex)
                {
                    ex.Redirect();
                }
            }
        }
    }
}
