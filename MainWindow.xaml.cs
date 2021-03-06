﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Data;
using sys_monitor_tool.entity;

namespace sys_monitor_tool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            UpdateStatusTaskManager.Execute(this, UpdateStatusTaskType.Server, () => {
                var serverData = ListenServerData.GetServerList();
                serverData.Columns.Add(new DataColumn("StatusColor", typeof(string)));
                foreach( DataRow row in serverData.Rows ) {
                    var host = row ["Host"].ToString();
                    var port = row ["HttpPort"].ToString();
                    var serverItem = new ListenServerItem();
                    serverItem.Host = host;
                    serverItem.HttpPort = port;
                    var url = serverItem.HttpUrl;
                    var result = HttpHelper.CheckHttp(url);
                    if( result.Item1 ) {
                        row ["Status"] = "正常";
                        row ["StatusColor"] = Common.DEFAULT_TEXT_COLOR;
                    } else {
                        row ["Status"] = "异常";
                        row ["StatusColor"] = "Red";
                    }
                }
                var dataView = serverData.DefaultView;
                if( serverData.Rows.Count > 0 ) {
                    dataView.Sort = "Status desc";
                }
                Dispatcher.Invoke( (Action)delegate {
                    listView.DataContext = dataView.ToTable();
                });
            });
        }

        private DataRowView GetSelectedItem() {
            var dataItem = listView.SelectedItem as DataRowView;
            return dataItem;
        }

        private string GetSelectedItemID() {
            var selectedItem = GetSelectedItem();
            if( selectedItem == null ) {
                return null;
            }
            return selectedItem ["ID"].ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wind = new ListenServerForm();
            wind.Owner = this;
            wind.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var id = GetSelectedItemID();
            if( string.IsNullOrEmpty(id) ) {
                return;
            }
            MessageBoxResult confirmToDel = MsgBox.Comfirn("确认要删除所选行吗？");
            if (confirmToDel == MessageBoxResult.Yes)
            {
                ListenServerData.Delete(id);
                Refresh();
            }
        }

        private void listView_SelectionChanged( object sender, MouseButtonEventArgs e ) {
            var id = GetSelectedItemID();
            if( string.IsNullOrEmpty(id) ) {
                return;
            }
            var serverItem = ListenServerData.GetServerItem(id);
            var serverManager = new ServerManager(serverItem);
            serverManager.Closed += delegate {
                this.Show();
            };
            serverManager.Owner = this;
            serverManager.Show();
            this.Hide();
        }

        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            var id = GetSelectedItemID();
            var property = new ServerProperty( id );
            property.Owner = this;
            property.ShowDialog();
        }
    }
}
