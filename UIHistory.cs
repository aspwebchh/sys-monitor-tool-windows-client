using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using sys_monitor_tool.entity;

namespace sys_monitor_tool {
    class UIHistory : UIBase {
        public UIHistory( ServerManager window, DataSource dataSource ) : base( window, dataSource ) {

        }

        public void InitHistoryList() {
            this.HideAllElement();
            this.ShowLoading();

            new Thread( delegate () {
                var data = dataSource.GetHistoryList();
                data.Sort( delegate ( HistoryItem b, HistoryItem a ) {
                    return (int) (DateTime.Parse( a.Date ) - DateTime.Parse( b.Date )).TotalHours;
                } );
                window.Dispatcher.Invoke( (Action)delegate {
                    SetListViewHeight( window.HistoryList );
                    window.HistoryList.Height = window.ContentPanel.ActualHeight;
                    window.HistoryList.DataContext = data;
                    window.HistoryList.Visibility = System.Windows.Visibility.Visible;
                    HideLoading();
                } );
            } ).Start();
        }
    }
}
