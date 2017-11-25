﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool.entity
{
    public class ListenServerItem
    {
        public string ID { get; set; }

        public string Name { get; set; }
        public string Host { get; set; }
        public string HttpPort { get; set; }
        public string HttpUrl {
            get {
                return "http://" + Host + ":" + HttpPort;
            }
        }
    }
}
