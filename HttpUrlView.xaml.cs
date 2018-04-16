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
    /// HttpUrlView.xaml 的交互逻辑
    /// </summary>
    public partial class HttpUrlView : Window {
        DetailPageDataBuilder builder;
        public static void NewWindow(Window owner, HttpUrl httpUrl, DataSource dataSource ) {
            var window = new HttpUrlView(httpUrl, dataSource);
            window.Owner = owner;
            window.ShowDialog();
        }

        DataSource dataSource;
        HttpUrl httpUrl;
        public HttpUrlView(HttpUrl httpUrl, DataSource dataSource) {
            this.dataSource = dataSource;
            this.httpUrl = httpUrl;

            InitializeComponent();

            builder = new DetailPageDataBuilder();
            builder.Build( "监控名称", httpUrl.Description );
            builder.Build( "监控链接", httpUrl.Url );
            builder.Build( "请求方法",httpUrl.Method );
            builder.Build( "请求延时", httpUrl.Delay + " ms" );

            var noticeTargetTask = Common.GetNoticeTargetNamesAsync( dataSource, httpUrl.NoticeTarget );
            var statusTask = Common.GetStatusAsync( delegate {
                return dataSource.GetUrlStatus();
            }, httpUrl.ID );
            builder.Build( "通知人员", noticeTargetTask.Result );
            builder.Build( "状态", statusTask.Result );
            builder.Build( "访问源", dataSource.ListenServerItem.Host );
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
