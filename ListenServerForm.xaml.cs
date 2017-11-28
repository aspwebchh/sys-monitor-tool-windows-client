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
using sys_monitor_tool.entity;
using System.Threading;

namespace sys_monitor_tool
{
    /// <summary>
    /// ListenServerAdd.xaml 的交互逻辑
    /// </summary>
    public partial class ListenServerForm : Window
    {
        private string id;

        public ListenServerForm()
        {
            InitializeComponent();
        }

        public ListenServerForm( ListenServerItem listenServerItem)
        {
            InitializeComponent();

            Host.Text = listenServerItem.Host;
            HttpPort.Text = listenServerItem.HttpPort;
            Name.Text = listenServerItem.Name;
            Key.Text = listenServerItem.Key;
            this.id = listenServerItem.ID;

        }

        public string CheckHttp( string host, string port ) {
            if( string.IsNullOrEmpty(host)) {
                return "主机地址为空";
            }
            if( string.IsNullOrEmpty(port)) {
                return "端口号为空";
            }
            if(!Validate.IsHost(host)) {
                return "主机地址格式不正确";
            }
            if(! Validate.IsInteger(port)) {
                return "端口号格式不正确";
            }

            var serverItem = new ListenServerItem();
            serverItem.Host = host;
            serverItem.HttpPort = port;
            var url = serverItem.HttpUrl;

            var checkResult = HttpHelper.CheckHttp(url);
            return checkResult.Item2;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var host = Host.Text.Trim();
            var httpPort = HttpPort.Text.Trim() ;
            var name = Name.Text.Trim();
            var key = Key.Text.Trim();
            var validate = new Validate();
            validate.AddCmd(new ValidateCmd(name, "名称未输入"));
            validate.AddCmd(new ValidateCmd(host, "主机地址未输入"));
            validate.AddCmd(new ValidateCmd(httpPort, "http端口未输入"));
            validate.AddCmd(new ValidateCmd("主机地址格式不正确", () => Validate.IsHost(host)));
            validate.AddCmd(new ValidateCmd("http端口格式不正确", () => Validate.IsInteger(httpPort)));
            validate.AddCmd( new ValidateCmd( key, "通信密钥未输入" ) );
            if( !validate.Execute()) {
                return;
            }
            

            if( !string.IsNullOrEmpty(this.id))
            {
                ListenServerData.Delete(id);
            }
            var success =  ListenServerData.AddServer(name, host, int.Parse( httpPort ), key);
            if( !success )
            {
                MsgBox.Alert("添加失败，主机和端口已存在");
                return;
            } else {
                MsgBox.Alert("操作完成");
            }
            (this.Owner as MainWindow).Refresh();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var msg = CheckHttp(Host.Text.Trim(), HttpPort.Text.Trim());
            MsgBox.Alert(msg);

        }
    }
}
