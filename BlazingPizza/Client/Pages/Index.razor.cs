using BlazingPizza.Client.Services;
using BlazingPizza.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Pages
{
    public partial class Index
    {
        #region Servivcios
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public OrderState OrderState { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        #endregion

        #region Variables
        List<PizzaSpecial> Specials;
        #endregion

        #region Overrides
        protected async override Task OnInitializedAsync()
        {
            Specials = await HttpClient.GetFromJsonAsync<List<PizzaSpecial>>("specials");
        }
        #endregion

        async Task RemovePizza(Pizza configurePizza)
        {
            if (await JSRuntime.Confirm($"Eliminar la pizza {configurePizza.Special.Name} de la ordern?"))
            {
                OrderState.RemoveConfiguredPizza(configurePizza);
            }
        }

    }
}
