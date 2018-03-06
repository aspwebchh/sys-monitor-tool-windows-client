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
using System.Data;
using System.Threading;
using sys_monitor_tool.entity;

namespace sys_monitor_tool {
    /// <summary>
    /// MySqlView.xaml 的交互逻辑
    /// </summary>
    public partial class MySqlView : Window {
        public static void NewWindow( Window owner, MySql mySql, DataSource dataSource ) {
            var window = new MySqlView(mySql, dataSource);
            window.Owner = owner;
            window.ShowDialog();
        }

        private MySql mysql;
        private DataSource dataSource;

        public MySqlView(MySql mysql, DataSource dataSource) {
            InitializeComponent();

            this.mysql = mysql;
            this.dataSource = dataSource;

            var builder = new DetailPageDataBuilder();
            builder.Build( "监控名称", mysql.Description );
            builder.Build( "主机", mysql.Host );
            builder.Build( "端口", mysql.Port );
            builder.Build( "用户", mysql.User );
            builder.Build( "密码", mysql.Password );
            builder.Build( "数据库", mysql.Database );
            builder.Build( "延时", mysql.Delay + " ms" );


            ContentList.DataContext = builder.DataSource;
  

            ThreadPool.QueueUserWorkItem( delegate {
                Dispatcher.Invoke( delegate {
                    builder.Build( "通知人员", Common.GetNoticeTarget( dataSource, mysql.NoticeTarget ) );
                } );
            } );
            ThreadPool.QueueUserWorkItem( delegate {
                var mySqlStatus = dataSource.GetMySqlStatus();
                var currStatus = mySqlStatus.Find( item => item.ID == mysql.ID );
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
