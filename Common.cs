using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool
{
   public  static class Common
    {
        public const String DEFAULT_TEXT_COLOR = "#042271";

        public static List<int> ToIntList( string intListString )
        {
            if( string.IsNullOrEmpty(intListString))
            {
                return new List<int>();
            }
            return intListString.Split(',').Select(item => Convert.ToInt32(item)).ToList();
        }

        public static String GetNoticeTarget( DataSource dataSource, string userIdString ) {
            if( string.IsNullOrEmpty(userIdString) ) {
                return "";
            }
            var users = dataSource.GetUserList();
            var names = userIdString.Split(',').ToList().Select(id => {
                return users.Find(item => item.ID == Convert.ToInt32(id));
            }).Where(item => item != null).Select(item => {
                return item != null ? item.Name : "";
            }).Distinct();
            return string.Join("，", names); 
        }
    }
}
