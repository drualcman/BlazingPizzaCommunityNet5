using BlazingPizza.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Pages
{
    public partial class Authentication
    {
        [Inject]
		public OrderState OrderState { get; set; }

        public PizzaAuthenticationState RemoteAuthenticationState { get; set; } = new PizzaAuthenticationState();

		protected override void OnInitialized()
		{
			if (RemoteAuthenticationActions.IsAction(RemoteAuthenticationActions.LogIn, this.Action))
			{
				RemoteAuthenticationState.Order = OrderState.Order;
			}
		}

		void RestorePizza(PizzaAuthenticationState saveRemoteAuthenticationState)
		{
			if (saveRemoteAuthenticationState.Order is not null)
			{
				OrderState.ReplaceOrder(saveRemoteAuthenticationState.Order);
			}
		}
	}
}
