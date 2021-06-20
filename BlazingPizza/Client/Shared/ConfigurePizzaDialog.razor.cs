using BlazingPizza.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Shared
{
    public partial class ConfigurePizzaDialog
    {
        #region parametros
        [Parameter]
        public Pizza Pizza { get; set; }
        #endregion
    }
}
