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
using System.Timers;

namespace sys_monitor_tool {
    class UIOverview :UIBase{
        public UIOverview( ServerManager window, DataSource dataSource ) : base(window, dataSource){
            var timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += ( o, e ) => {
                this.FillData();
            };
            this.FillData();    
        }

        private void SetColorAsRed( TextBlock titleField, TextBlock valField  ) {
            var red = new SolidColorBrush( ( (Color)ColorConverter.ConvertFromString( "Red" ) ) );
            titleField.Foreground = red;
            valField.Foreground = red;
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
                        SetColorAsRed( window.MySQL_Error_Title, window.MySQL_Error );
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
                        SetColorAsRed( window.Process_Error_Title, window.Process_Error );

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
                        SetColorAsRed( window.Url_Error_Title,  window.Url_Error );
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
