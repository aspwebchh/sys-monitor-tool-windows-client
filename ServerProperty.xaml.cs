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
    /// ServerProperty.xaml 的交互逻辑
    /// </summary>
    public partial class ServerProperty : Window {
        private string serverId;
        private ListenServerItem serverItem;
        private DataSource dataSource;
        public ServerProperty( string id ) {
            this.serverId = id;
            this.serverItem = ListenServerData.GetServerItem( serverId );
            this.dataSource = new DataSource( serverItem );

            InitializeComponent();
        }

        public void ShowMailTab() {
            Content.SelectedIndex = 1;
        }

        public void Tab_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            var index = Content.SelectedIndex;
            if( index == 0 ) {
                this.FillServerTab();
                CheckServer.Visibility = Visibility.Visible;
            } else if( index == 1 ) {
                CheckServer.Visibility = Visibility.Collapsed;
                new Thread( delegate () {
                    this.FillMailTab();
                } ).Start();
            } 
        }

        private void FillServerTab() {
            Tab1_Host.Text = serverItem.Host;
            Tab1_HttpPort.Text = serverItem.HttpPort;
            Tab1_Name.Text = serverItem.Name;
            Tab1_Key.Text = serverItem.Key;
        }

        private void FillMailTab() {
            if( !serverItem.IsValid ) {
                MsgBox.Alert( "服务器信息配置错误" );
                return;
            }
            var checkResult = HttpHelper.CheckHttp( serverItem.HttpUrl );
            if( !checkResult.Item1 ) {
                MsgBox.Alert( checkResult.Item2 );
                return;
            }
            var mailInfo = dataSource.GetSmtpMailInfo();
            if( mailInfo == null ) {
                return;
            }
            Dispatcher.Invoke( delegate () {
                Mail.Text = mailInfo.Email;
                Password.Password = mailInfo.Password;
                SmtpServer.Text = mailInfo.SmtpServer;
                if( mailInfo.SmtpPServerPort > 0 ) {
                    SmtpServerPort.Text = mailInfo.SmtpPServerPort.ToString();
                }
                SSL.IsChecked = mailInfo.SSL == 0 ? false : true;
            } );
        }

        private bool handleMailTab(  bool isClose ) {
            var mail = Mail.Text.Trim();
            var password = Password.Password.Trim();
            var smtpServer = SmtpServer.Text.Trim();
            var port = SmtpServerPort.Text.Trim();
            var ssl = SSL.IsChecked.Value ? 1 : 0;
            var validate = new Validate();
            validate.AddCmd( new ValidateCmd( mail, "邮箱地址未输入" ) );
            validate.AddCmd( new ValidateCmd( "邮箱格式不正确", delegate () {
                return Validate.IsMail( mail );
            } ) );
            validate.AddCmd( new ValidateCmd( password, "密码未输入" ) );
            validate.AddCmd( new ValidateCmd( port, "端口未输入" ) );
            validate.AddCmd( new ValidateCmd( "端口格式不正确", delegate () {
                return Validate.IsInteger( port );
            } ) );
            if( !validate.Execute() ) {
                return false;
            }
            var mailInfo = new SmtpMailInfo();
            mailInfo.Email = mail;
            mailInfo.Password = password;
            mailInfo.SmtpServer = smtpServer;
            mailInfo.SmtpPServerPort = int.Parse( port );
            mailInfo.SSL = ssl;

            var result = dataSource.ChangeSmtpMailInfo( mailInfo );
            if( result.Code == ServerResult<object>.CODE_ERROR ) {
                MessageBox.Show( result.Message );
                return false;
            }
            if(isClose) {
                Close();
            }
            return true;
            
        }

        private void handleServerTab() {
            var host = Tab1_Host.Text.Trim();
            var httpPort = Tab1_HttpPort.Text.Trim();
            var name = Tab1_Name.Text.Trim();
            var key = Tab1_Key.Text.Trim();
            var success = ListenServerForm.Save( this.serverId, host, httpPort, name, key );
            if( success ) {
                Close();
            }
        }


        private void Button_Click( object sender, RoutedEventArgs e ) {
            this.Close();
        }

        private void Button_Click_1( object sender, RoutedEventArgs e ) {
            if( Content.SelectedIndex == 0 ) {
                handleServerTab();
            } else if( Content.SelectedIndex == 1 ) {
                handleMailTab(true);
            }
        }

        private void RunTest_Click( object sender, RoutedEventArgs e ) {
            var mail = TestMail.Text;

            var validate = new Validate();
            validate.AddCmd( new ValidateCmd( mail, "邮箱地址未输入" ) );
            validate.AddCmd( new ValidateCmd( "邮箱格式不正确", delegate () {
                return Validate.IsMail( mail );
            } ) );
            if( !validate.Execute() ) {
                return;
            }
            if( handleMailTab( false ) ) {
                var result = dataSource.TestSendMail( mail );
                MsgBox.Alert( result.Message );
            }
        }

        private void CheckServer_Click( object sender, RoutedEventArgs e ) {
            var msg = ListenServerForm.CheckHttp( Tab1_Host.Text.Trim(), Tab1_HttpPort.Text.Trim() );
            MsgBox.Alert( msg );
        }
    }
}
