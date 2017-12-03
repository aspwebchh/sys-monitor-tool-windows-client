using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool.entity {
    public class SmtpMailInfo {
        public string Email {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }

        public string SmtpServer {
            get;
            set;
        }

        public int SmtpPServerPort {
            get;
            set;
        }

        public int SSL {
            get;
            set;
        }

        public bool IsValid {
            get {
                return !( string.IsNullOrEmpty( Email ) ||
                    string.IsNullOrEmpty( Password ) ||
                    string.IsNullOrEmpty( SmtpServer ) ||
                    ( SmtpPServerPort <= 0 ));
            }
        }
    }
}
