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
        DetailPageDataBuilder builder;
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

            builder = new DetailPageDataBuilder();
            builder.Build( "进程名称", process.ProcessName );


            var noticeTargetTask = Common.GetNoticeTargetNamesAsync( dataSource, process.NoticeTarget );
            var statusTask = Common.GetStatusAsync( delegate {
                return dataSource.GetProcessStatus();
            }, process.ID );
            builder.Build( "通知人员", noticeTargetTask.Result );
            builder.Build( "状态", statusTask.Result );
            ContentList.DataContext = builder.DataSource;
        }

        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            var item = ContentList.SelectedItem as DataRowView;
            Clipboard.SetDataObject( item[ "Value" ].ToString() );
        }

        private void MenuItem_Click_Copy_All( object sender, RoutedEventArgs e ) {
            var copied = Common.DataTableToString( builder.DataSource );
            Clipboard.SetDataObject( copied );
        }
    }
}
