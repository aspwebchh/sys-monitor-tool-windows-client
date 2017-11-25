using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys_monitor_tool
{
    class ValidateCmd
    {
        private string content;
        private string message;
        private Func<bool> validate;

        public ValidateCmd(string content, string message)
        {
            this.content = content;
            this.message = message;
            this.validate = () =>
            {
                if( string.IsNullOrEmpty(this.content))  
                {
                    return false;
                }
                return this.content.Trim() != "";
            };
        }

        public ValidateCmd( string message, Func<bool> validate) : this("",message)
        {
            this.validate = validate;
        }

        public string Message { get { return message; } }

        public bool Execute()
        {
            return validate();
        }
    }
}
