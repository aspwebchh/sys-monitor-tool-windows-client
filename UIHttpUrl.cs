using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using sys_monitor_tool.entity;
using System.Threading;

namespace sys_monitor_tool
{
    class UIHttpUrl : UIBase
    {
        private NoticeTargetContainer httpUrlNoticeTargetContainer;
        private PageMode pageMode;
        private int currentItemId;

        public UIHttpUrl(ServerManager window, DataSource dataSource) : base(window, dataSource){
            httpUrlNoticeTargetContainer = new NoticeTargetContainer(window.HttpUrlNoticeTarget, dataSource);

            window.HttpUrl_Save_Btn.Click += HttpUrl_Save_Btn_Click;
        }

        delegate ServerResult<object> WebServiceCallback(string name, string method, string url , string noticeTarget);

        private void Save(WebServiceCallback callback)
        {
            var noticeTargets = httpUrlNoticeTargetContainer.GetSelectedValues();
            var name = window.HttpUrl_Name.Text.Trim();
            var method = (window.HttpUrl_Method.SelectedValue as ComboBoxItem).Content.ToString();
            var url = window.HttpUrl_Url.Text.Trim();

            var validate = new Validate();
            validate.AddCmd(new ValidateCmd(name, "名称未输入"));
            validate.AddCmd(new ValidateCmd(url, "URL未输入"));
            validate.AddCmd(new ValidateCmd("URL格式错误", () => Validate.IsHttpUrl(url)));
            validate.AddCmd(new ValidateCmd("通知人员未选择", () => noticeTargets.Count > 0));

            if(!validate.Execute())
            {
                return;
            }

            var result = callback(name, method, url, string.Join(",",noticeTargets));
            MsgBox.Alert(result.Message);
            if( result.Code == ServerResult<object>.CODE_SUCCESS)
            {
                InitHttpUrlList();
                TreeViewReset();
            }
        }

        private void HttpUrl_Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            if( this.pageMode == PageMode.Add)
            {
                Save((string name, string method, string url, string noticeTarget) => {
                    return dataSource.AddUrl(name, method, url, noticeTarget);
                });
            } else if( this.pageMode == PageMode.Edit)
            {
                Save((string name, string method, string url, string noticeTarget) => {
                    return dataSource.updateUrl(currentItemId, name, method, url, noticeTarget);
                });
            } else
            {
                MsgBox.Alert("程序出现BUG， 联系开发者");
            }
        }

        public void InitHttpUrlList()
        {
            pageMode = PageMode.List;

            this.HideAllElement();
            this.ShowLoading();

            new Thread(() => {
                var data = dataSource.GetUrlList();
                window.Dispatcher.Invoke(() => {
                    window.HttpUrlList.DataContext = data;
                    window.HttpUrlList.Visibility = Visibility.Visible;
                    HideLoading();
                });
                this.UpdateStatus(window.HttpUrlList, data.Select(item => item as EntityBase).ToList(), UpdateStatusTaskType.HttpUrl, dataSource.GetUrlStatus);
            }).Start();
        }

        public void InitHttpUrlAdd()
        {
            pageMode = PageMode.Add;

            window.HttpUrl_Name.Clear();
            window.HttpUrl_Method.SelectedIndex = 0;
            window.HttpUrl_Url.Clear();

            this.HideAllElement();
            window.HttpUrl_Form.Visibility = Visibility.Visible;
            httpUrlNoticeTargetContainer.Refresh();
            //httpUrlNoticeTargetContainer.UnSelectAll();
        }

        public void DeleteItemHandle()
        {
            var dataItem = window.HttpUrlList.SelectedItem as HttpUrl;
            if( dataItem == null ) {
                return;
            }
            MessageBoxResult confirmToDel = MsgBox.Comfirn("确认要删除所选记录吗？");
            if (confirmToDel != MessageBoxResult.Yes)
            {
                return;
            }
            var result = dataSource.DeleteUrl(dataItem.ID);
            if (result.Code == ServerResult<object>.CODE_SUCCESS)
            {
                this.InitHttpUrlList();
            }
            else
            {
                MsgBox.Alert(result.Message);
            }
        }

        public void EditItemHandle()
        {
            var dataItem = window.HttpUrlList.SelectedItem as HttpUrl;
            if( dataItem == null ) {
                return;
            }

            this.HideAllElement();

            pageMode = PageMode.Edit;

            window.HttpUrl_Form.Visibility = Visibility.Visible;

            currentItemId = dataItem.ID;
            var item = dataSource.GetUrlItem(currentItemId);

            window.HttpUrl_Name.Text = item.Description;
            window.HttpUrl_Method.SelectedIndex = new string[] { "GET", "POST" }.ToList().IndexOf(item.Method.ToUpper());
            window.HttpUrl_Url.Text = item.Url;

            httpUrlNoticeTargetContainer.Refresh();
            httpUrlNoticeTargetContainer.Select(item.NoticeTargetItems);

            TreeViewReset();
        }

        private void TreeViewReset()
        {
            window.TreeViewItem_HttpUrl.IsSelected = true;
        }


        public void OpenSelectedItemUrl() {
            var dataItem = window.HttpUrlList.SelectedItem as HttpUrl;
            if( dataItem == null ) {
                return;
            }
            try {

                System.Diagnostics.Process.Start(dataItem.Url);
            } catch( Exception ex ) {
                MsgBox.Alert(ex.Message);
            }
        }
    }
}
