using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Shared
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }



        protected override void OnInitialized()
        {
            string EscapedUri = Uri.EscapeDataString(NavigationManager.Uri);
            NavigationManager.NavigateTo($"authentication/login?returnUrl={EscapedUri}");
        }
    }
}
