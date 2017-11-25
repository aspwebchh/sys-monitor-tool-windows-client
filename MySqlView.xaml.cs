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
    /// MySqlView.xaml 的交互逻辑
    /// </summary>
    public partial class MySqlView : Window {
        private static Window window;
        public static void NewWindow( Window owner, MySql mySql, DataSource dataSource ) {
            if( window != null ) {
                window.Close();
                window = null;
            }
            window = new MySqlView(mySql, dataSource);
            window.Owner = owner;
            window.Show();
        }

        private MySql mysql;
        private DataSource dataSource;

        public MySqlView(MySql mysql, DataSource dataSource) {
            InitializeComponent();

            this.mysql = mysql;
            this.dataSource = dataSource;

            Name.Text = mysql.Description;
            Host.Text = mysql.Host;
            Port.Text = mysql.Port;
            User.Text = mysql.User;
            Password.Text = mysql.Password;
            Database.Text = mysql.Database;

            new Thread(() => {
                Dispatcher.Invoke(() => {
                    FillStatus();
                    NoticeTarget.Text = Common.GetNoticeTarget(dataSource, mysql.NoticeTarget);
                });
            }).Start();
        }

        private void FillStatus() {
            var mySqlStatus = dataSource.GetMySqlStatus();
            var currStatus = mySqlStatus.Find(item => item.ID == mysql.ID);
            if( currStatus == null ) {
                return;
            }
            if( currStatus.Status ) {
                Status.Text = currStatus.StatusDesc;
            } else {
                Status.Foreground = Brushes.Red;
                Status.Text = currStatus.Message;
            }
        }
    }
}
