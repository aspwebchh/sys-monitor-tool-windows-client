using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;

namespace sys_monitor_tool {
    enum UpdateStatusTaskType {
        Process,
        MySql,
        HttpUrl,
        Server,
        Overview
    }

    static class UpdateStatusTaskManager {
        private static Dictionary<string, Timer> dic = new Dictionary<string, Timer>();

        private static string Key( Window window, UpdateStatusTaskType ustt ) {
            var hashCode = window.GetHashCode();
            var key = hashCode + "-" + ustt;
            return key;
        }

        public static void Remove( Window window, UpdateStatusTaskType ustt ) {
            var key = Key( window, ustt );
            if( dic.ContainsKey( key ) ) {
                var found = dic[ key ];
                found.Stop();
                dic.Remove( key );
            }
        }

        public static void Execute( Window window, UpdateStatusTaskType ustt, Action action ) {
            var key = Key( window, ustt );
            if( dic.ContainsKey(key)) {
                var found = dic [key];
                found.Stop();
                dic.Remove(key);
            }

            var timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += delegate ( object sender, ElapsedEventArgs e ) {
                action();
            };

            action();
            dic.Add(key, timer);
        }
    }
}
