using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sys_monitor_tool
{
    static class MsgBox
    {
        private const string caption = "提示";
        public static void Alert( string msg )
        {
            MessageBox.Show(msg,caption);
        }

        public static MessageBoxResult Comfirn(string msg)
        {
            return MessageBox.Show(msg, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
