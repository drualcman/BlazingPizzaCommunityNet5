using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingPizza.TemplatedComponents
{
    public partial class TemplatedDialog
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        
        [Parameter]
        public bool Show { get; set; }
    }
}
