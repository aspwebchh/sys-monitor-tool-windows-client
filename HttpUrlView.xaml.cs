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

            var builder = new DetailPageDataBuilder();
            builder.Build( "监控名称", httpUrl.Description );
            builder.Build( "监控链接", httpUrl.Url );
            builder.Build( "请求方法",httpUrl.Method );
            builder.Build( "请求延时", httpUrl.Delay + " ms" );

            ContentList.DataContext = builder.DataSource;

            ThreadPool.QueueUserWorkItem( delegate {
                Dispatcher.Invoke( delegate {
                    builder.Build( "通知人员", Common.GetNoticeTarget( dataSource, httpUrl.NoticeTarget ) );
                } );
            } );

            ThreadPool.QueueUserWorkItem( delegate {
                var urlStatus = dataSource.GetUrlStatus();
                var currStatus = urlStatus.Find( item => item.ID == httpUrl.ID );
                if( currStatus == null ) {
                    return;
                }
                var status = currStatus.Status ? currStatus.StatusDesc : currStatus.Message;
                Dispatcher.Invoke( delegate {
                    builder.Build( "状态", status );
                } );
            } );
        }

        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            var item = ContentList.SelectedItem as DataRowView;
            Clipboard.SetDataObject( item[ "Value" ].ToString() );
        }
    }
}
