using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Threading;

namespace sys_monitor_tool {
   

    public partial class App : Application {
        protected override void OnStartup( System.Windows.StartupEventArgs e ) {
            base.OnStartup( e );

            Process[] processcollection = Process.GetProcessesByName( "系统可用性监控工具" );
            if( processcollection.Length >= 2 ) {
                MessageBox.Show( "应用程序已运行" );
                Thread.Sleep( 1000 );
                System.Environment.Exit( 1 );
            }
        }
    }
}
