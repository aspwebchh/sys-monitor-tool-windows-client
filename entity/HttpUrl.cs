using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool.entity {
    public class HttpUrl : EntityBase{
        public string NoticeTarget { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }


        public List<int> NoticeTargetItems {
            get {
                return Common.ToIntList(NoticeTarget);
            }
        }
    }
}
