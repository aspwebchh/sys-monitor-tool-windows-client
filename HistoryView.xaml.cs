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
    /// HistoryView.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryView : Window {
        private DataSource dataSource;
        public HistoryView( DataSource dataSource, HistoryItem historyItem ) {
            InitializeComponent();
            this.dataSource = dataSource;
            var result = dataSource.GetHistoryDetail( historyItem );
            HistoryList.DataContext = result;

            new Thread( delegate () {
                result = result.Select( item => {
                    var targetName = dataSource.GetTargetName( item.MonitorType, item.ItemID );
                    if( string.IsNullOrEmpty( targetName ) ) {
                        targetName = "监控被删除";
                    }
                    item.MonitorName = targetName;
                    return item; 
                } ).ToList();
                this.Dispatcher.Invoke( delegate () {
                    HistoryList.DataContext = result;
                } );
            } ).Start();
        }

        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            var dataItem = HistoryList.SelectedItem as HistoryDetailItem;
            switch( dataItem.MonitorType ) {
                case MonitorType.Process:
                    var processWindow = new ProcessView( dataSource.GetProcessItem( dataItem.ItemID ), dataSource );
                    processWindow.Owner = this;
                    processWindow.ShowDialog();
                    break;
                case MonitorType.MySql:
                    var mySqlWindow = new MySqlView( dataSource.GetMySqlItem( dataItem.ItemID ), dataSource );
                    mySqlWindow.Owner = this;
                    mySqlWindow.ShowDialog();
                    break;
                case MonitorType.HttpUrl:
                    var urlWindow = new HttpUrlView( dataSource.GetUrlItem( dataItem.ItemID ), dataSource );
                    urlWindow.Owner = this;
                    urlWindow.ShowDialog();
                    break;
                default:
                    MsgBox.Alert( "未知监控类型" );
                    break;
            }

        }
    }
}
