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
    /// HttpUrlView.xaml 的交互逻辑
    /// </summary>
    public partial class HttpUrlView : Window {
        private static Window window;
        public static void NewWindow(Window owner, HttpUrl httpUrl, DataSource dataSource ) {
            if(window != null) {
                window.Close();
                window = null;
            }
            window = new HttpUrlView(httpUrl, dataSource);
            window.Owner = owner;
            window.Show();
        }

        DataSource dataSource;
        HttpUrl httpUrl;
        public HttpUrlView(HttpUrl httpUrl, DataSource dataSource) {
            this.dataSource = dataSource;
            this.httpUrl = httpUrl;

            InitializeComponent();

            Name.Text = httpUrl.Description;
            URL.Text = httpUrl.Url;
            Method.Text = httpUrl.Method;

            new Thread(()=> {
                Dispatcher.Invoke(() => {
                    FillStatus();
                    NoticeTarget.Text = Common.GetNoticeTarget(dataSource, httpUrl.NoticeTarget);
                });
            }).Start();
        }

        private void FillStatus() {
            var urlStatus = dataSource.GetUrlStatus();
            var currStatus = urlStatus.Find(item => item.ID == httpUrl.ID);
            if(currStatus == null) {
                return;
            }
            if( currStatus.Status) {
                Status.Text = currStatus.StatusDesc;
            } else {
                Status.Foreground = Brushes.Red;
                Status.Text = currStatus.Message;
            }
        }

        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            try {
                System.Diagnostics.Process.Start(URL.Text);
            } catch( Exception ex ) {
                MsgBox.Alert(ex.Message);
            }
        }

        private void MenuItem_Click_1( object sender, RoutedEventArgs e ) {
            var text = URL.SelectedText;
            if( string.IsNullOrEmpty(text)) {
                text = URL.Text;
            }
            Clipboard.SetDataObject(text);
        }
    }
}
