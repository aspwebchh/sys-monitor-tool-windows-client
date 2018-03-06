using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using sys_monitor_tool.entity;
using System.Windows.Controls;
using System.Threading;

namespace sys_monitor_tool
{
    class UIUser : UIBase
    {
        private PageMode pageMode;
        private int currentItemId;

        public UIUser( ServerManager window, DataSource dataSource ) : base(window, dataSource){
            window.User_Save_Btn.Click += User_Save_Btn_Click;
        }

        private void User_Save_Btn_Click( object sender, RoutedEventArgs e ) {
            if( pageMode == PageMode.Add) {
                Save(( string name, string mobile, string mail ) => {
                    return dataSource.AddUser(name, mobile, mail);
                });
            } else if( pageMode == PageMode.Edit) {
                Save(( string name, string mobile, string mail ) => {
                    return dataSource.UpdateUser(currentItemId, name, mobile, mail);
                });
            } else {
                MsgBox.Alert("程序出现BUG， 联系开发者");
            }
  
        }

        delegate ServerResult<object> WebServiceCallback( string name, string mobile, string mail ); 

        private void Save(WebServiceCallback callback) {
            var name = window.User_Name.Text.Trim();
            var mobile = window.User_Mobile.Text.Trim();
            var mail = window.User_Mail.Text.Trim();
            var validate = new Validate();
            validate.AddCmd( new ValidateCmd( name, "未输入姓名" ));
            validate.AddCmd( new ValidateCmd( mobile, "未输入手机号码" ) );
            validate.AddCmd( new ValidateCmd( "手机号码格式不正确" ,()=> Validate.IsMobile(mobile) ));
            validate.AddCmd( new ValidateCmd( mail, "未输入邮箱" ) );
            validate.AddCmd( new ValidateCmd( "邮箱格式不正确",()=> Validate.IsMail(mail) ) );
            if( !validate.Execute() ) {
                return;
            }
            var result = callback( name, mobile, mail );
            MsgBox.Alert( result.Message );
            if( result.Code == ServerResult<object>.CODE_SUCCESS ) {
                InitUserList();
                TreeViewReset();
            }
        }

        public void InitUserList() {
            pageMode = PageMode.List;

            this.HideAllElement();
            this.ShowLoading();
            new Thread(() => {
                var data = dataSource.GetUserList();
                window.Dispatcher.Invoke(() => {
                    SetListViewHeight( window.UserList );
                    window.UserList.DataContext = data;
                    window.UserList.Visibility = Visibility.Visible;
                    HideLoading();
                });
            }).Start();
        }


        public void InitUserAdd() {
            pageMode = PageMode.Add;

            window.User_Name.Clear();
            window.User_Mail.Clear();
            window.User_Mobile.Clear();

            this.HideAllElement();
            window.User_Form.Visibility = Visibility.Visible;
        }

        public void EditItemHandle() {
            var dataItem = window.UserList.SelectedItem as User;
            if( dataItem == null ) {
                return;
            }

            this.HideAllElement();

            pageMode = PageMode.Edit;

            window.User_Form.Visibility = Visibility.Visible;

            currentItemId = dataItem.ID;
            var item = dataSource.GetUserItem(currentItemId);

            window.User_Name.Text = item.Name;
            window.User_Mobile.Text = item.Mobile;
            window.User_Mail.Text = item.Email;

            TreeViewReset();
        }

        public void DeleteItemHandle( ) {
            var dataItem = window.UserList.SelectedItem as User;
            if( dataItem == null ) {
                return;
            }

            MessageBoxResult confirmToDel = MsgBox.Comfirn("确认要删除所选记录吗？");
            if( confirmToDel != MessageBoxResult.Yes ) {
                return;
            }
            var result = dataSource.DeleteUser(dataItem.ID);
            if( result.Code == ServerResult<object>.CODE_SUCCESS ) {
                this.InitUserList();
            } else {
                MsgBox.Alert(result.Message);
            }
        }


        private void TreeViewReset() {
            window.TreeViewItem_User.IsSelected = true;
        }
    }
}
