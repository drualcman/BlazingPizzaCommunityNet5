using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingPizza.TemplatedComponents
{
    public partial class TemplatedList<TItem>
    {
        [Parameter]
        public Func<Task<List<TItem>>> Loader { get; set; }

        [Parameter]
        public RenderFragment Loading { get; set; }

        [Parameter]
        public RenderFragment Empty { get; set; }

        [Parameter]
        public RenderFragment<TItem> Item { get; set; }

        [Parameter]
        public string ListGroupClass { get; set; }

        List<TItem> Items { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Items = await Loader();
        }
    }
}
