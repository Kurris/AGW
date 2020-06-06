using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Main
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


#if DEBUG
            Application.Run(new frmMain());
#else

            var login = new frmLogin();

            if (login.ShowDialog() == DialogResult.Yes)
            {
                var splash = new frmSplash();
                if (splash.ShowDialog() == DialogResult.Yes)
                {
                    Application.Run(new frmMain());
                }
            }
            else
            {
                Application.Exit();
            }
#endif
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

        }
    }
}
