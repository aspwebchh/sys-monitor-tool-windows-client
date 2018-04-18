using sys_monitor_tool.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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


        public static Task<string> GetNoticeTargetNamesAsync( DataSource dataSource, string noticeTargetIDs ) {
            return Task.Factory.StartNew( delegate {
                return Common.GetNoticeTarget( dataSource, noticeTargetIDs );
            } );
        }

        public static Task<string> GetStatusAsync( Func<List<EntityStatus>> getStatusSrc, int id ) {
            return Task.Factory.StartNew( delegate {
                var mySqlStatus = getStatusSrc();
                var currStatus = mySqlStatus.Find( item => item.ID == id );
                if( currStatus == null ) {
                    return string.Empty;
                }
                var status = currStatus.Status ? currStatus.StatusDesc : currStatus.Message;
                return status;
            } );
        }


        public static string DataTableToString( DataTable datatable ) {
            return datatable.Rows.Cast<DataRow>().Select( row => {
                var name = row[ "Name" ].ToString();
                var value = row[ "RealValue" ].ToString();
                return name + "：" + value;
            } ).Aggregate( delegate ( string a, string b ) {
                return a + Environment.NewLine + b;
            } );
        }
    }
}
