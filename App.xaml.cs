using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;

namespace sys_monitor_tool {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public class EntryPoint {

        [STAThread]
        public static void Main( string[] args ) {
            SingleInstanceManager manager = new SingleInstanceManager();
            manager.Run( args );
        }
    }



    // Using VB bits to detect single instances and process accordingly:
    //  * OnStartup is fired when the first instance loads
    //  * OnStartupNextInstance is fired when the application is re-run again
    //    NOTE: it is redirected to this instance thanks to IsSingleInstance
    public class SingleInstanceManager : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase {
        App app;

        public SingleInstanceManager() {
            this.IsSingleInstance = true;
        }

        protected override bool OnStartup( Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e ) {
            // First time app is launched
            app = new App();
            app.Run();
            return false;
        }


        protected override void OnStartupNextInstance( StartupNextInstanceEventArgs eventArgs ) {
            // Subsequent launches
            base.OnStartupNextInstance( eventArgs );
            app.Activate();
        }

    }

    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>

    public partial class App : Application {
        protected override void OnStartup( System.Windows.StartupEventArgs e ) {
            base.OnStartup( e );
            MainWindow mw = new MainWindow();
            mw.Show();
        }



        public void Activate() {
            var mainWindow = this.MainWindow as MainWindow;
            mainWindow.Show();
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Visibility = System.Windows.Visibility.Visible;
            mainWindow.Activate();
        }
    }
}
