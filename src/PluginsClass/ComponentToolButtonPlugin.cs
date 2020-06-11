using AGW.Base.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGW.Base.Plugins
{
    public abstract class ComponentToolBarPlugins
    {
        public ComponentToolBarPlugins()
        {

        }

        public ComponentDataGrid DataGrid { get; internal set; }

        public abstract void OnActived();
    }
}
