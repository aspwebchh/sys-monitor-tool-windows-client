using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace sys_monitor_tool {
    public static class AuthMessageManager {
        private static Dictionary<string, int> count = new Dictionary<string, int>();

        private static string GetDomain( string url ) {
            var regex = new Regex(@"(?<=\/\/)[^\/]+");
            var match = regex.Match(url);
            return match.Value;
        }

        public static void Clear(string url) {
            var domain = GetDomain(url);
            if( string.IsNullOrEmpty(domain)) {
                return;
            }
            if( count.ContainsKey(domain)) {
                count [domain] = 0;
            }
        }

        private static void Add( string url) {
            var domain = GetDomain(url);
            if( string.IsNullOrEmpty(domain) ) {
                return;
            }
            if( !count.ContainsKey(domain)) {
                count [domain] = 0;
            }
            count [domain]++;
        }

        public static void ShowMessage( string url, string message ) {
            Add(url);
            var domain = GetDomain(url);
            if( string.IsNullOrEmpty(domain) ) {
                return;
            }
            var val = count [domain];
            if( val == 1 ) {
                if( message.IndexOf("403") != -1 ) {
                    MsgBox.Alert("无法获取数据，与服务器的通信密钥不匹配");
                }
            }
        }
    }
}
