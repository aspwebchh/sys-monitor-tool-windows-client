using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace sys_monitor_tool {
    class NoticeTargetContainer {

        private WrapPanel container;
        private DataSource dataSource;
        private Dictionary<CheckBox, int> dic = new Dictionary<CheckBox, int>();

        public NoticeTargetContainer( WrapPanel container, DataSource dataSource) {
            this.container = container;
            this.dataSource = dataSource;
            this.Fill();
        }

        private void Fill() {
            container.Children.Clear();
            var users = dataSource.GetUserList();
            foreach(var user in users) {
                var checkbox = new CheckBox();
                var text = new TextBlock();
                text.Text = user.Name;
                checkbox.Content = text;
                checkbox.Margin = new Thickness(0,0,15,0);
                dic.Add(checkbox, user.ID);
                container.Children.Add(checkbox);
            }
        }

        public void Refresh() {
            this.Fill();
        }

        public List<int> GetSelectedValues() {
            var result = new List<int>();
            foreach(var checkbox in dic.Keys) {
                if( (bool)checkbox.IsChecked ) {
                    result.Add(dic [checkbox]);
                }
            }
            return result.Distinct().ToList();
        }

        public void Select( List<int> selectVals)
        {
            foreach (var checkbox in dic.Keys)
            {
                var val = dic[checkbox];
                if(selectVals.Contains(val))
                {
                    checkbox.IsChecked = true;
                }
            }
        }

        public void UnSelectAll()
        {
            dic.Keys.ToList().ForEach(item => item.IsChecked = false);
        }
    } 
}
