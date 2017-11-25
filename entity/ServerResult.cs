using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool.entity
{
   public class ServerResult<T>
    {
        public const int CODE_SUCCESS = 0;
        public const int CODE_ERROR = 1;

        public int Code { get; set; }
        public String Message { get; set; }
        public T Data { get; set; }
    }
}
