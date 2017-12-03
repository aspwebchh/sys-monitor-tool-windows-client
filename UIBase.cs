using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sys_monitor_tool {
    abstract class UIBase {
        protected enum PageMode { List, Add, Edit }

        protected delegate List<entity.EntityStatus> GetStatus();

        protected ServerManager window;
        protected DataSource dataSource;
        public UIBase( ServerManager window, DataSource dataSource ) {
            this.window = window;
            this.dataSource = dataSource;
        }

        protected void HideAllElement() {
            window.Overview.Visibility = Visibility.Collapsed;
            window.MySqlList.Visibility = Visibility.Collapsed;
            window.MySql_Form.Visibility = Visibility.Collapsed;
            window.ProcessList.Visibility = Visibility.Collapsed;
            window.Process_Form.Visibility = Visibility.Collapsed;
            window.HttpUrlList.Visibility = Visibility.Collapsed;
            window.HttpUrl_Form.Visibility = Visibility.Collapsed;
            window.UserList.Visibility = Visibility.Collapsed;
            window.User_Form.Visibility = Visibility.Collapsed;
        }

        protected void UpdateStatus(ListView listView, List<entity.EntityBase> data, UpdateStatusTaskType type, GetStatus getStatus) {
            UpdateStatusTaskManager.Execute(window, type, () => {
                data = data.Select(item => {
                    var status = getStatus();
                    var result = status.Where(n => n.ID == item.ID);
                    if( result.Count() > 0 ) {
                        var first = result.First();
                        var desc = first.StatusDesc;
                        item.Status = desc;
                        item.StatusColor = first.StatusTextColor;
                        item.Delay = first.Delay;
                    }
                    return item;
                }).ToList();
                window.Dispatcher.Invoke(() => {
                    listView.DataContext = data;
                });
            });
        }

        protected void HideLoading() {
            window.Loading.Visibility = Visibility.Collapsed;
        }

        protected void ShowLoading() {
            window.Loading.Visibility = Visibility.Visible;
        }
    }
}
