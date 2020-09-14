using AGW.Base.Components;
using System;
using static AGW.Base.GlobalInvariant;

namespace AGW.Base.Plugins
{
    public abstract class ComponentToolBarPlugins
    {
        public ComponentToolBarPlugins()
        {

        }

        /// <summary>
        /// Function Entry
        /// </summary>
        public abstract void OnActivated();


        /// <summary>
        /// Connected data container
        /// </summary>
        public ComponentDataGrid DataGrid { get; internal set; }

        /// <summary>
        /// User Info
        /// </summary>
        public UserInfo User { get; internal set; }


        internal Action<object, EventArgs> RefreshDataGrid { private get; set; }

        public virtual void RefreshGrid()
        {
            RefreshDataGrid.Invoke(DataGrid.ToolBar, null);
        }
    }
}
