using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using sys_monitor_tool.entity;
using System.Windows.Controls;

namespace sys_monitor_tool {
    class UIMySQL : UIBase{

        private NoticeTargetContainer mySqlNoticeTargetContainer;
        private PageMode pageMode;
        private int currentItemId;


        public UIMySQL( ServerManager window, DataSource dataSource ) : base(window, dataSource){
            mySqlNoticeTargetContainer = new NoticeTargetContainer(window.MySqlNoticeTarget, dataSource,window);
            window.MySql_Save_Btn.Click += MySql_Save_Btn_Click;
        }


        delegate ServerResult<object> CallWebService(string host, string port, string user, string password, string db_name, string name, string notice_target);

        private void Save(CallWebService callWebService)
        {
            var noticeTargets = mySqlNoticeTargetContainer.GetSelectedValues();
            var name = window.MySql_Name.Text.Trim();
            var host = window.MySql_Host.Text.Trim();
            var port = window.MySql_Port.Text.Trim();
            var user = window.MySql_User.Text.Trim();
            var password = window.MySql_Password.Text.Trim();
            var database = window.MySql_Database.Text.Trim();

            var validate = new Validate();
            validate.AddCmd(new ValidateCmd(name, "监控名称为空"));
            validate.AddCmd(new ValidateCmd(host, "主机地址为空"));
            validate.AddCmd(new ValidateCmd(port, "端口号为空"));
            validate.AddCmd(new ValidateCmd("端口号必须为数字", () => Validate.IsInteger(port)));
            validate.AddCmd(new ValidateCmd(user, "用户为空"));
            validate.AddCmd(new ValidateCmd(password, "密码为空"));
            validate.AddCmd(new ValidateCmd(database, "数据库名为空"));
            validate.AddCmd(new ValidateCmd("通知人员未选择", () => noticeTargets.Count > 0));

            if (!validate.Execute())
            {
                return;
            }

            var result = callWebService(host, port, user, password, database, name, string.Join(",", noticeTargets));

            MsgBox.Alert(result.Message);
            if (result.Code == ServerResult<object>.CODE_SUCCESS)
            {
                this.InitMySqlList();
                TreeViewReset();
            }
        }
 
        private void MySql_Save_Btn_Click( object sender, RoutedEventArgs e ) {
           switch(pageMode)
            {
                case PageMode.Add:
                    this.Save((string host, string port, string user, string password, string database, string name, string notice_target) => {
                        return dataSource.AddMySql(host, port, user, password, database, name, notice_target);
                    });
                    break;
                case PageMode.Edit:
                    this.Save((string host, string port, string user, string password, string database, string name, string notice_target) => {
                        return dataSource.UpdateMySql(currentItemId, host, port, user, password, database, name, notice_target);
                    });
                    break;
                default:
                    MsgBox.Alert("程序出现BUG， 联系开发者");
                    break;
            }   
        }

        public void InitMySqlList() {
            pageMode = PageMode.List;

            this.HideAllElement();
            this.ShowLoading();

            ThreadPool.QueueUserWorkItem( delegate {
                var data = dataSource.GetMySqlList();
                window.Dispatcher.Invoke( (Action)delegate {
                    SetListViewHeight( window.MySqlList );
                    window.MySqlList.DataContext = data;
                    window.MySqlList.Visibility = Visibility.Visible;
                    HideLoading();
                } );
                this.UpdateStatus( window.MySqlList, data.Select( item => item as EntityBase ).ToList(), UpdateStatusTaskType.MySql, dataSource.GetMySqlStatus );
            } );
           
        }

        public void InitMySqlAdd() {
            pageMode = PageMode.Add;

            window.MySql_Name.Clear();
            window.MySql_Host.Clear();
            window.MySql_Port.Clear();
            window.MySql_User.Clear();
            window.MySql_Password.Clear();
            window.MySql_Database.Clear();

            this.HideAllElement();
            window.MySql_Form.Visibility = Visibility.Visible;
            mySqlNoticeTargetContainer.Refresh();
            //mySqlNoticeTargetContainer.UnSelectAll();
        }

        public void DeleteItemHandle() {
            var dataItem = window.MySqlList.SelectedItem as MySql;
            if( dataItem == null) {
                return;
            }
            MessageBoxResult confirmToDel = MsgBox.Comfirn("确认要删除所选记录吗？");
            if( confirmToDel != MessageBoxResult.Yes ) {
                return;
            }
            var result = dataSource.DeleteMySql(dataItem.ID);
            if( result.Code == ServerResult<object>.CODE_SUCCESS) {
                this.InitMySqlList();
            } else
            {
                MsgBox.Alert(result.Message);
            }
        }

        public void EditItemHandle()
        {
            var dataItem = window.MySqlList.SelectedItem as MySql;
            if( dataItem == null ) {
                return;
            }

            this.HideAllElement();

            pageMode = PageMode.Edit;

            window.MySql_Form.Visibility = Visibility.Visible;

            currentItemId = dataItem.ID;
            var item = dataSource.GetMySqlItem(currentItemId);

            window.MySql_Name.Text = item.Description;
            window.MySql_Host.Text = item.Host;
            window.MySql_Port.Text = item.Port;
            window.MySql_User.Text = item.User;
            window.MySql_Password.Text = item.Password;
            window.MySql_Database.Text = item.Database;

            mySqlNoticeTargetContainer.Refresh();
            mySqlNoticeTargetContainer.Select(item.NoticeTargetItems);

            TreeViewReset();
        }

        public void DisableNotice( bool status ) {
            var selectItem = window.MySqlList.SelectedItem as MySql;
            if( selectItem == null ) {
                return;
            }
            var result = dataSource.DisableMySqlNotice( selectItem.ID, status );
            if( result.Code == ServerResult<object>.CODE_SUCCESS ) {
                this.InitMySqlList();
            } else {
                MsgBox.Alert( result.Message );
            }
        }

        private void TreeViewReset()
        {
            window.TreeViewItem_MySql.IsSelected = true;
        }
    }
}
