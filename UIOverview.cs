using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;

namespace sys_monitor_tool {
    class UIOverview :UIBase{
        public UIOverview( ServerManager window, DataSource dataSource ) : base(window, dataSource){
            UpdateStatusTaskManager.Execute( window, UpdateStatusTaskType.Overview, delegate () {
                this.FillData();
            } );  
        }

        private void SetColorAs( TextBlock titleField, TextBlock valField, string color  ) {
            var scb = new SolidColorBrush( ( (Color)ColorConverter.ConvertFromString( color ) ) );
            titleField.Foreground = scb;
            valField.Foreground = scb;
        }

        private void FillData() {

            new Thread( () => {
                var mySqlStatus = dataSource.GetMySqlStatus();
                var mySqlAllCount = mySqlStatus.Count;
                var mySqlNormal = mySqlStatus.Where( item => item.Status ).Count();
                var mySqlError = mySqlAllCount - mySqlNormal;

                window.Dispatcher.Invoke( () => {
                    window.MySQL_All.Text = mySqlAllCount.ToString();
                    window.MySQL_Normal.Text = mySqlNormal.ToString();
                    window.MySQL_Error.Text = mySqlError.ToString();
                    if( mySqlError > 0 ) {
                        SetColorAs( window.MySQL_Error_Title, window.MySQL_Error, "Red" );
                    } else {
                        SetColorAs( window.MySQL_Error_Title, window.MySQL_Error, "Black" );
                    }

                } );
            } ).Start();

            new Thread( () => {
                var processStatus = dataSource.GetProcessStatus();
                var processAllCount = processStatus.Count;
                var processNormal = processStatus.Where( item => item.Status ).Count();
                var processError = processAllCount - processNormal;

                window.Dispatcher.Invoke( () => {
                    window.Process_All.Text = processAllCount.ToString();
                    window.Process_Normal.Text = processNormal.ToString();
                    window.Process_Error.Text = processError.ToString();
                    if( processError > 0 ) {
                        SetColorAs( window.Process_Error_Title, window.Process_Error,"Red" );
                    } else {
                        SetColorAs( window.MySQL_Error_Title, window.MySQL_Error, "Black" );
                    }
                } );

            } ).Start();

            new Thread( () => {
                var urlStatus = dataSource.GetUrlStatus();
                var urlAllCount = urlStatus.Count;
                var urlNormal = urlStatus.Where( item => item.Status ).Count();
                var urlError = urlAllCount - urlNormal;

                window.Dispatcher.Invoke( () => {
                    window.Url_All.Text = urlAllCount.ToString();
                    window.Url_Normal.Text = urlNormal.ToString();
                    window.Url_Error.Text = urlError.ToString();
                    if( urlError > 0 ) {
                        SetColorAs( window.Url_Error_Title, window.Url_Error, "Red" );
                    } else {
                        SetColorAs( window.Url_Error_Title, window.Url_Error, "Black" );
                    }
                } );

            } ).Start();
        }

        public void show() {
            this.HideAllElement();
            window.Overview.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
