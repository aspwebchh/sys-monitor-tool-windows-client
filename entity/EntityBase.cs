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

        public int Delay {
            get;
            set;
        }

        public string StatusColor { get; set; }


        public string DisableNotice {
            get;set;
        }

        public bool IsDisableNotice {
            get {
                if( string.IsNullOrEmpty( DisableNotice ) ) {
                    return false;
                } else if( DisableNotice == "1" ) {
                    return true;
                } else {
                    return false;
                }
            } 
        }

        public string DisableNoticeDesc {
            get {
                if( !IsDisableNotice ) {
                    return "是";
                } else {
                    return "否";
                }
            }
        }
    }
}
