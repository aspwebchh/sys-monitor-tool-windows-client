using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool.entity {
    public class HistoryItem {
        public string FileName {
            get;
            set;
        }

        public string Date {
            get {
                return FileName.Split( '.' )[ 0 ].Replace('_','-');
            }
        }
    }
}
