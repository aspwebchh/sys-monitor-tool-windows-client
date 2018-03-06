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
using System.Data;
using sys_monitor_tool.entity;

namespace sys_monitor_tool {
    /// <summary>
    /// ProcessView.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessView : Window {
        public static void NewWindow( Window owner, Process process, DataSource dataSource ) {
            var window = new ProcessView(process, dataSource);
            window.Owner = owner;
            window.ShowDialog();
        }

        Process process;
        DataSource dataSource;

        public ProcessView( Process process, DataSource dataSource ) {
            InitializeComponent();

            this.process = process;
            this.dataSource = dataSource;

            var builder = new DetailPageDataBuilder();
            builder.Build( "进程名称", process.ProcessName );

            ContentList.DataContext = builder.DataSource;

            ThreadPool.QueueUserWorkItem( delegate {
                var processStatus = dataSource.GetProcessStatus();
                var currStatus = processStatus.Find( item => item.ID == process.ID );
                if( currStatus == null ) {
                    return;
                }
                var status = currStatus.Status ? currStatus.StatusDesc : currStatus.Message;
                Dispatcher.Invoke( delegate {
                    builder.Build( "状态", status );
                } );
            } );

            ThreadPool.QueueUserWorkItem( delegate {
                Dispatcher.Invoke( delegate {
                    builder.Build( "通知人员", Common.GetNoticeTarget( dataSource, process.NoticeTarget ) );
                } );
            } );
        }

        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            var item = ContentList.SelectedItem as DataRowView;
            Clipboard.SetDataObject( item[ "Value" ].ToString() );
        }
    }
}
