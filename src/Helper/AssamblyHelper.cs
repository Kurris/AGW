using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base.Helper
{
    [Serializable]
    public class AssamblyHelper
    {

        public AssamblyHelper(string FileName)
        {
            _msfileName = FileName;
        }

        private string _msfileName = string.Empty;

        public void LoadAssembly(string FullName)
        {

            AppDomain appDomain = AppDomain.CreateDomain(FullName);

            string name = Assembly.GetExecutingAssembly().GetName().FullName;

            AssamblyProxyObject Proxy = (AssamblyProxyObject)appDomain.CreateInstanceAndUnwrap(name, typeof(AssamblyProxyObject).FullName);

            Proxy.LoadAssembly(_msfileName, FullName);

            AppDomain.Unload(appDomain);
            appDomain = null;
        }

        private class AssamblyProxyObject : MarshalByRefObject
        {
            public AssamblyProxyObject()
            {

            }

            public void LoadAssembly(string FileName, string FullName)
            {
                Assembly assembly = Assembly.LoadFrom(FileName);

                Type type = assembly.GetType(FullName);
                (Activator.CreateInstance(type) as Form).ShowDialog();
            }

        }
    }
}

