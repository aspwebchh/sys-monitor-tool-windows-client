using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool.entity {
   public class EntityStatus {
        public int ID { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }

        public int Delay {
            get;
            set;
        }

        public string StatusDesc {
            get {
                return Status ? "正常" : "异常";
            }
        }

        public string StatusTextColor {
            get {
                return Status ? Common.DEFAULT_TEXT_COLOR : "Red";
            }
        }
    }
}
