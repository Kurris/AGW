using AGW.Base.Components;
using AGW.Base.Plugins;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AGW.Base.Helper
{
    public class AssamblyHelper
    {

        public AssamblyHelper(string FileName)
        {
            _msfileName = FileName;
        }

        private string _msfileName = string.Empty;

        public void LoadAssembly(ToolStripButton Button, Action<object, EventArgs> Refresh, string FullName)
        {
            Assembly assembly = Assembly.LoadFrom(_msfileName);

            Type type = assembly.GetType(FullName);

            if (type == null) return;

            object obj = Activator.CreateInstance(type);

            if (obj == null) return;

            if (obj is ComponentToolBarPlugins)
            {
                ComponentToolbar toolbar = Button.GetCurrentParent() as ComponentToolbar;

                ComponentToolBarPlugins abPlugin = obj as ComponentToolBarPlugins;
                abPlugin.DataGrid = toolbar.DataGrid;
                abPlugin.User = GlobalInvariant.User;
                abPlugin.RefreshDataGrid = Refresh;

                abPlugin.OnActivated();
            }
        }
    }
}

