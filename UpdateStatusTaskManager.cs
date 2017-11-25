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
        Server
    }

    class UpdateStatusTaskManager {
        private static Dictionary<UpdateStatusTaskType, Timer> dic = new Dictionary<UpdateStatusTaskType, Timer>();

        public static void Execute( UpdateStatusTaskType type, Action action ) {
            if( dic.ContainsKey(type)) {
                var found = dic [type];
                found.Stop();
                dic.Remove(type);
            }

            var timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += ( o, e ) => {
                action();
            };

            action();
            dic.Add(type, timer);
        }
    }
}
