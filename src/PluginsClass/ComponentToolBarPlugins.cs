using AGW.Base.Components;
using System;

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
        public UserInfo UserInfo { get; internal set; }


        internal Action<object, EventArgs> RefreshDataGrid { private get; set; }

        public virtual void RefreshGrid()
        {
            RefreshDataGrid.Invoke(DataGrid.Toolbar, null);
        }
    }
}
