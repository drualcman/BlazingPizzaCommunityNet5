using BlazingPizza.Shared;
using Microsoft.AspNetCore.Components;
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
        HttpClient HttpClient { get; set; }
        #endregion

        #region Variables
        List<PizzaSpecial> Specials;

        Pizza ConfiguringPizza;
        bool ShowingConfigureDialog;
        #endregion

        #region Overrides
        protected async override Task OnInitializedAsync()
        {
            Specials = await HttpClient.GetFromJsonAsync<List<PizzaSpecial>>("specials");
        }
        #endregion

        #region Metodos
        void ShowConfigurePizzaDialog(PizzaSpecial special)
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
    }
}
