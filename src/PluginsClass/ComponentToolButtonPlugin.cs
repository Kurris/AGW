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

        /// <summary>
        /// Connected data container
        /// </summary>
        public ComponentDataGrid DataGrid { get; internal set; }

        /// <summary>
        /// User Info
        /// </summary>
        public UserInfo UserInfo { get; internal set; }

        /// <summary>
        /// Function Entry
        /// </summary>
        public abstract void OnActivated();
    }
}
