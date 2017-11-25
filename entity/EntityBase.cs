using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool.entity {
    public abstract class EntityBase {
        public int ID { get; set; }

        private string status = "未知";
        public string Status {
            get { return status; }
            set { status = value; }
        }

        public string StatusColor { get; set; }
    }
}
