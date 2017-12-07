using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace sys_monitor_tool.entity {
    public class HistoryDetailItem {
        public string Type {
            get; set;
        }

        public string GlobalID {
            get; set;
        }

        public string TimeString {
            get; set;
        }

        public string Message {
            get;
            set;
        }

        public string TypeDesc {
            get {
                if( Type == "1" ) {
                    return "异常";
                } else if( Type == "0" ) {
                    return "恢复";
                } else {
                    return "未知类型";
                }
            }
        }
        
        public string TypeDescColor {
            get {
                if( Type == "0" ) {
                    return Common.DEFAULT_TEXT_COLOR;
                } else {
                    return "Red";
                }
            }
        }

        public DateTime Time {
            get {
                return DateTime.Parse( TimeString );
            }
        }

        public MonitorType MonitorType {
            get {
                if( GlobalID.IndexOf( "HttpUrlDataItem" ) != -1 ) {
                    return MonitorType.HttpUrl;
                } else if( GlobalID.IndexOf( "MySqlDataItem" ) != -1 ) {
                    return MonitorType.MySql;
                } else if( GlobalID.IndexOf( "ProcessDataItem" ) != -1 ) {
                    return MonitorType.Process;
                } else {
                    return MonitorType.UnKnown;
                }
            }
        }

        public string MonitorTypeDesc {
            get {
                switch( MonitorType ) {
                    case MonitorType.HttpUrl:
                    return "HttpUrl监控";
                    case MonitorType.MySql:
                    return "MySQL监控";
                    case MonitorType.Process:
                    return "进程监控";
                    default:
                    return "未知类型";
                }
            }
        }

        public int ItemID {
            get {
                return int.Parse( Regex.Match( GlobalID, @"\d+$" ).Value );
            }
        }
    }
}
