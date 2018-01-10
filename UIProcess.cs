using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using sys_monitor_tool.entity;
using System.Windows.Controls;

namespace sys_monitor_tool {
    class UIProcess : UIBase {

        private NoticeTargetContainer processNoticeTargetContainer;
        private PageMode pageMode;


        public UIProcess( ServerManager window, DataSource dataSource ) : base(window, dataSource) {
            processNoticeTargetContainer = new NoticeTargetContainer(window.ProcessNoticeTarget, dataSource);

            window.Procress_Save_Btn.Click += Procress_Save_Btn_Click;
        }

        private void Procress_Save_Btn_Click( object sender, RoutedEventArgs e ) {
            var processName = window.Process_Name.Text.Trim();
            var noticeTargets = processNoticeTargetContainer.GetSelectedValues();
            var validate = new Validate();
            validate.AddCmd(new ValidateCmd(processName, "进程名称未输入"));
            validate.AddCmd(new ValidateCmd("通知人员未选择", () => noticeTargets.Count > 0));
            if( !validate.Execute() ) {
                return;
            }
            var result = dataSource.AddProcess(processName, string.Join(",", noticeTargets));
            MsgBox.Alert(result.Message);
            if( result.Code == ServerResult<object>.CODE_SUCCESS ) {
                InitProcessList();
                TreeViewReset();
            }
        }

        public void InitProcessList() {
            pageMode = PageMode.List;

            this.HideAllElement();
            this.ShowLoading();

            new Thread(() => {
                var data = dataSource.GetProcessList();
                window.Dispatcher.Invoke(() => {
                    window.ProcessList.DataContext = data;
                    window.ProcessList.Visibility = Visibility.Visible;
                    HideLoading();
                });
                this.UpdateStatus(window.ProcessList, data.Select(item => item as EntityBase).ToList(), UpdateStatusTaskType.Process, dataSource.GetProcessStatus);
            }).Start();
        }

        public void InitProcessAdd() {
            pageMode = PageMode.Add;
            window.Process_Name.Clear();
            this.HideAllElement();
            window.Process_Form.Visibility = Visibility.Visible;
            processNoticeTargetContainer.Refresh();
            //processNoticeTargetContainer.UnSelectAll();
        }

        public void DeleteItemHandle() {
            var selectItem = window.ProcessList.SelectedItem as Process;
            if(selectItem == null) {
                return;
            }
            MessageBoxResult confirmToDel = MsgBox.Comfirn("确认要删除所选记录吗？");
            if( confirmToDel != MessageBoxResult.Yes ) {
                return;
            }
            var result = dataSource.DeleteProcess(selectItem.ID);
            if( result.Code == ServerResult<object>.CODE_SUCCESS ) {
                this.InitProcessList();
            } else {
                MsgBox.Alert(result.Message);
            }
        }

        public void DisableNotice( bool status ) {
            var selectItem = window.ProcessList.SelectedItem as Process;
            if( selectItem == null ) {
                return;
            }
            var result = dataSource.DisableProcessNotice( selectItem.ID, status );
            if( result.Code == ServerResult<object>.CODE_SUCCESS ) {
                this.InitProcessList();
            } else {
                MsgBox.Alert( result.Message );
            }
        }

        private void TreeViewReset() {
            window.TreeViewItem_Process.IsSelected = true;
        }
    }
}
