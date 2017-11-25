using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using sys_monitor_tool.entity;

namespace sys_monitor_tool {
    /// <summary>
    /// ProcessView.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessView : Window {
        private static Window window;
        public static void NewWindow( Window owner, Process process, DataSource dataSource ) {
            if( window != null ) {
                window.Close();
                window = null;
            }
            window = new ProcessView(process, dataSource);
            window.Owner = owner;
            window.Show();
        }

        Process process;
        DataSource dataSource;

        public ProcessView( Process process, DataSource dataSource ) {
            InitializeComponent();

            this.process = process;
            this.dataSource = dataSource;

            Name.Text = process.ProcessName;

            new Thread(() => {
                Dispatcher.Invoke(() => {
                    FillStatus();
                    NoticeTarget.Text = Common.GetNoticeTarget(dataSource, process.NoticeTarget);
                });
            }).Start();
        }

        private void FillStatus() {
            var processStatus = dataSource.GetProcessStatus();
            var currStatus = processStatus.Find(item => item.ID == process.ID);
            if( currStatus == null ) {
                return;
            }
            if( currStatus.Status ) {
                Status.Text = currStatus.StatusDesc;
            } else {
                Status.Foreground = Brushes.Red;
                Status.Text = currStatus.Message;
            }
        }
    }
}
