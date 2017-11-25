using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace sys_monitor_tool {
    class Validate {

        public static bool IsMobile(string mobile) {
            return Regex.IsMatch( mobile, @"^\d{11}$" );
        }

        public static bool IsMail( string mail ) {
            return Regex.IsMatch( mail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" );
        }

        public static bool IsHttpUrl( string url ) {
            return Regex.IsMatch( url, @"^http(s)?://([\w-]+\.)+[\w-]+(:\d+)?(/[\w- ./?%&amp;=]*)?$", RegexOptions.IgnoreCase );
        }

        public static bool IsHost( String host ) {
            return Regex.IsMatch( host, @"^[0-9a-z\.]+$", RegexOptions.IgnoreCase );
        }

        public static bool IsInteger( String val ) {
            int result;
            return int.TryParse( val, out result );
        }

        private List<ValidateCmd> cmds = new List<ValidateCmd>();

        public void AddCmd( ValidateCmd cmd ) {
            cmds.Add( cmd );
        }

        public bool Execute() {
            foreach( var cmd in cmds ) {
                if( !cmd.Execute() ) {
                    MsgBox.Alert( cmd.Message );
                    return false;
                }
            }
            return true;
        }
    }
}
