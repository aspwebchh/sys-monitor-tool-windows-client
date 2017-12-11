
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace sys_monitor_tool.entity
{
   public  class MySql : EntityBase
    {
        public string NoticeTarget { get; set; }
        public string Description { get; set; }
        public string ConnectionString { get; set; }

        private string GetValue(int index)
        {
            if (index > 5)
            {
                return "";
            }
            var regex = new Regex(@"(.+?):(.+?)@tcp\(([\w.]+):(\d+)\)\/(\w+)\?.+");
            var match = regex.Match(ConnectionString);
            if (match.Success)
            {
                var group = match.Groups[index];
                if (group.Success)
                {
                    return group.Value;
                }
            }
            return "";
        }


        public string Host {
            get {
                return GetValue(3);
            }
        }

        public string Port {
            get {
                return GetValue(4);
            }
        }

        public string User {
            get {
                return GetValue(1);
            }
        }

        public string Password {
            get {
                return GetValue(2);
            }
        }

        public string Database {
            get {
                return GetValue(5);
            }
        }

        public List<int> NoticeTargetItems {
            get {
                return Common.ToIntList(NoticeTarget);
            }
        }

    }
}
