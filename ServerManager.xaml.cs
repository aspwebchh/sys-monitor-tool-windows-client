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
using System.Windows.Shapes;
using System.Threading;
using sys_monitor_tool.entity;

namespace sys_monitor_tool
{
    /// <summary>
    /// ServerManager.xaml 的交互逻辑
    /// </summary>
    public partial class ServerManager : Window
    {
        ListenServerItem listenServerItem;
        DataSource dataSource;
        UIMySQL uiMySql;
        UIProcess uiProcess;
        UIHttpUrl uiHttpUrl;
        UIUser uiUser;
        UIOverview uiOverview;
        UIHistory uiHistory;

        public ServerManager(ListenServerItem listenServerItem)
        {
            InitializeComponent();

            AuthMessageManager.Clear(listenServerItem.HttpUrl);

            this.listenServerItem = listenServerItem;
            this.Title = listenServerItem.Name;

            this.dataSource = new DataSource( listenServerItem );
            this.uiMySql = new UIMySQL( this, dataSource );
            this.uiProcess = new UIProcess( this, dataSource );
            this.uiHttpUrl = new UIHttpUrl( this, dataSource );
            this.uiUser = new UIUser( this, dataSource );
            this.uiOverview = new UIOverview( this, dataSource );
            this.uiHistory = new UIHistory( this, dataSource );

            ThreadPool.QueueUserWorkItem( delegate {
                if( CheckServerStatus() ) {
                    CheckMailStmpServer();
                }
            } );

            this.Closed += ( o, e ) => {
                UpdateStatusTaskManager.Remove( this, UpdateStatusTaskType.HttpUrl );
                UpdateStatusTaskManager.Remove( this, UpdateStatusTaskType.MySql );
                UpdateStatusTaskManager.Remove( this, UpdateStatusTaskType.Process );
                UpdateStatusTaskManager.Remove( this, UpdateStatusTaskType.Overview );
            };
        }

        private void CheckMailStmpServer() {
            var mailInfo = dataSource.GetSmtpMailInfo();
            if( mailInfo == null ) {
                //服务器还未更新至支持发件箱配置的版本
                return;
            }
            if( !mailInfo.IsValid ) {
                Dispatcher.Invoke( (Action)delegate {
                    var serverProperty = new ServerProperty( listenServerItem.ID );
                    serverProperty.Owner = this;
                    serverProperty.Show();
                    serverProperty.ShowMailTab();
                } );
            }
        }

        private bool CheckServerStatus() {
            var result = HttpHelper.CheckHttp(listenServerItem.HttpUrl);
            if( !result.Item1 ) {
                MsgBox.Alert( result.Item2 );
                return false;
            } else {
                return true;
            }
        }


        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            uiMySql.InitMySqlList();
        }

        private void TreeViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            uiMySql.InitMySqlAdd();
        }

        private void Button_Click( object sender, RoutedEventArgs e ) {
            uiMySql.DeleteItemHandle();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            uiMySql.EditItemHandle();
        }

        private void TreeViewItem_Selected_2(object sender, RoutedEventArgs e)
        {
            uiProcess.InitProcessList();
        }

        private void TreeViewItem_Selected_3(object sender, RoutedEventArgs e)
        {
            uiProcess.InitProcessAdd();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            uiProcess.DeleteItemHandle();
        }

        private void TreeViewItem_Selected_4(object sender, RoutedEventArgs e)
        {
            uiHttpUrl.InitHttpUrlList();
        }

        private void TreeViewItem_Selected_5(object sender, RoutedEventArgs e)
        {
            uiHttpUrl.InitHttpUrlAdd();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            uiHttpUrl.DeleteItemHandle();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            uiHttpUrl.EditItemHandle();
        }

        private void TreeViewItem_Selected_6( object sender, RoutedEventArgs e ) {
            uiUser.InitUserList();
        }

        private void TreeViewItem_Selected_7( object sender, RoutedEventArgs e ) {
            uiUser.InitUserAdd();
        }

        private void Button_Click_5( object sender, RoutedEventArgs e ) {
            uiUser.EditItemHandle();
        }

        private void Button_Click_6( object sender, RoutedEventArgs e ) {
            uiUser.DeleteItemHandle();
        }
        
        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            uiHttpUrl.OpenSelectedItemUrl();
        }

        private void HttpUrlList_SelectionChanged( object sender, MouseButtonEventArgs e ) {
            var dataItem = HttpUrlList.SelectedItem as HttpUrl;
            if( dataItem == null ) {
                return;
            }
            HttpUrlView.NewWindow(this, dataItem, dataSource);
        }

        private void MySqlList_MouseLeftButtonUp( object sender, MouseButtonEventArgs e ) {
            var dataItem = MySqlList.SelectedItem as MySql;
            if( dataItem == null ) {
                return;
            }
            MySqlView.NewWindow(this, dataItem, dataSource);
        }

        private void MenuItem_Click_1( object sender, RoutedEventArgs e ) {
            var dataItem = MySqlList.SelectedItem as MySql;
            if( dataItem == null ) {
                return;
            }
            Clipboard.SetDataObject(dataItem.ConnectionString);
        }

        private void ProcessList_MouseLeftButtonUp( object sender, MouseButtonEventArgs e ) {
            var dataItem = ProcessList.SelectedItem as Process;
            if( dataItem == null ) {
                return;
            }
            ProcessView.NewWindow(this, dataItem, dataSource);
        }

        private void TreeViewItem_Selected_8( object sender, RoutedEventArgs e ) {
            this.uiOverview.show();
        }

        private void TreeViewItem_Selected_9( object sender, RoutedEventArgs e ) {
            this.uiHistory.InitHistoryList();
        }


        private void HistoryList_MouseLeftButtonUp( object sender, MouseButtonEventArgs e ) {
            var historyView = new HistoryView(dataSource, HistoryList.SelectedItem as HistoryItem);
            historyView.Owner = this;
            historyView.ShowDialog();
        }

        private void MenuItem_Click_2( object sender, RoutedEventArgs e ) {
            this.uiProcess.DisableNotice( true );
        }

        private void MenuItem_Click_3( object sender, RoutedEventArgs e ) {
            this.uiProcess.DisableNotice( false );
        }

        private void MenuItem_Click_4( object sender, RoutedEventArgs e ) {
            this.uiMySql.DisableNotice( true );
        }

        private void MenuItem_Click_5( object sender, RoutedEventArgs e ) {
            this.uiMySql.DisableNotice( false );
        }

        private void MenuItem_Click_6( object sender, RoutedEventArgs e ) {
            this.uiHttpUrl.DisableNotice( true );
        }

        private void MenuItem_Click_7( object sender, RoutedEventArgs e ) {
            this.uiHttpUrl.DisableNotice( false );
        }
    }
}
