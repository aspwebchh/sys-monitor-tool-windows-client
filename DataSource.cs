using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sys_monitor_tool.entity;
using Newtonsoft.Json;

namespace sys_monitor_tool
{
   public class DataSource
    {
        private readonly string key;

        private readonly string baseUrl;
        private readonly string userListUrl;
        private readonly string addUserUrl;
        private readonly string updateUserUrl;
        private readonly string deleteUserUrl;
        private readonly string userItemUrl;

        private readonly string mySqlListUrl;
        private readonly string addMySqlUrl;
        private readonly string updateMySqlUrl;
        private readonly string deleteMySqlUrl;
        private readonly string mySqlItemUrl;
        private readonly string mySqlStatusUrl;

        private readonly string processListUrl;
        private readonly string addProcessUrl;
        private readonly string updateProcessUrl;
        private readonly string deleteProcessUrl;
        private readonly string processItemUrl;
        private readonly string processStatusUrl;

        private readonly string urlListUrl;
        private readonly string addUrlUrl;
        private readonly string updateUrlUrl;
        private readonly string deleteUrlUrl;
        private readonly string urlItemUrl;
        private readonly string urlStatusUrl;

        private readonly string urlGetMailInfo;
        private readonly string urlChangeMailInfo;
        private readonly string urlTestSendMail;

        private readonly string urlHistoryList;


        public DataSource(ListenServerItem listenServerItem )
        {
            this.key = listenServerItem.Key;
            this.baseUrl =  listenServerItem.HttpUrl;

            this.userListUrl = baseUrl + "/user_list";
            this.addUserUrl = baseUrl + "/user_add";
            this.updateUserUrl = baseUrl + "/user_update";
            this.deleteUserUrl = baseUrl + "/user_remove";
            this.userItemUrl = baseUrl + "/user_item";


            this.mySqlListUrl = baseUrl + "/mysql_list";
            this.addMySqlUrl = baseUrl + "/mysql_add";
            this.updateMySqlUrl = baseUrl + "/mysql_update";
            this.deleteMySqlUrl = baseUrl + "/mysql_remove";
            this.mySqlItemUrl = baseUrl + "/mysql_item";
            this.mySqlStatusUrl = baseUrl + "/mysql_status";

            this.processListUrl = baseUrl + "/process_list";
            this.addProcessUrl = baseUrl + "/process_add";
            this.updateProcessUrl = baseUrl + "/process_update";
            this.deleteProcessUrl = baseUrl + "/process_remove";
            this.processItemUrl = baseUrl + "/process_item";
            this.processStatusUrl = baseUrl + "/process_status";

            this.urlListUrl = baseUrl + "/url_list";
            this.addUrlUrl = baseUrl + "/url_add";
            this.updateUrlUrl = baseUrl + "/url_update";
            this.deleteUrlUrl = baseUrl + "/url_remove";
            this.urlItemUrl = baseUrl + "/url_item";
            this.urlStatusUrl = baseUrl + "/url_status";

            this.urlGetMailInfo = baseUrl + "/get_mail_info";
            this.urlChangeMailInfo = baseUrl + "/change_mail_info";
            this.urlTestSendMail = baseUrl + "/test_send_mail";

            this.urlHistoryList = baseUrl + "/history_file_list";
        }

        private ServerResult<object> CheckResult( ServerResult<object> result ) {
            if( result == null ) {
                result = new ServerResult<object>();
                result.Code = ServerResult<object>.CODE_ERROR;
                result.Message = "设置失败， 可能是服务器与客户端软件版本不一致引起的";
            }
            return result;
        }

        public ServerResult<object> TestSendMail( string email ) {
            var dic = new Dictionary<string, string>();
            dic.Add( "email", email );
            var json = HttpHelper.Get( this.urlTestSendMail,dic, this.key );
            var result = JsonConvert.DeserializeObject<ServerResult<object>>( json );
            return CheckResult( result );
        }

        public SmtpMailInfo GetSmtpMailInfo() {
            var json = HttpHelper.Get( this.urlGetMailInfo, new Dictionary<string, string>(), this.key );
            var mailInfo = JsonConvert.DeserializeObject<SmtpMailInfo>( json );
            return mailInfo;
        }

        public ServerResult<object> ChangeSmtpMailInfo( SmtpMailInfo mailInfo ) {
            var dic = new Dictionary<string, string>();
            dic.Add( "email", mailInfo.Email );
            dic.Add( "password", mailInfo.Password );
            dic.Add( "smtp_server", mailInfo.SmtpServer );
            dic.Add( "smtp_server_port", mailInfo.SmtpPServerPort.ToString() );
            dic.Add( "ssl", mailInfo.SSL.ToString() );
            var json = HttpHelper.Get( this.urlChangeMailInfo, dic, key );
            var result = JsonConvert.DeserializeObject<ServerResult<object>>( json );
            return CheckResult( result );
        }

        #region
        public List<User> GetUserList()
        {
            var json = HttpHelper.Get(this.userListUrl,new Dictionary<string, string>(), this.key);
            List<User> userList = JsonConvert.DeserializeObject<List<User>>(json);
            if( userList == null) {
                return new List<User>();
            }
            userList.Sort( ( a, b ) => b.ID - a.ID );
            return userList;
        }

        public ServerResult<Object> AddUser( string name, string mobile, string email)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("name", name);
            dic.Add("mobile", mobile);
            dic.Add("email", email);
            var result = HttpHelper.Get(this.addUserUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> UpdateUser(int id, string name, string mobile, string email)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("name", name);
            dic.Add("mobile", mobile);
            dic.Add("email", email);
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.updateUserUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> DeleteUser(int id)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.deleteUserUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public User GetUserItem(int id)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.userItemUrl, dic, this.key );
            return JsonConvert.DeserializeObject<User>(result);
        }
        #endregion

        #region
        public List<MySql> GetMySqlList() {
            var json = HttpHelper.Get(this.mySqlListUrl, new Dictionary<string, string>(), this.key );
            List<MySql> mySqlList = JsonConvert.DeserializeObject<List<MySql>>(json);
            if( mySqlList == null) {
                mySqlList = new List<MySql>();
            }
            var status = GetMySqlStatus();
            mySqlList = mySqlList.Select( item => {
                var statusItem = status.Find( sItem => sItem.ID == item.ID );
                if( statusItem != null ) {
                    item.Delay = statusItem.Delay;
                }
                return item;
            } ).ToList();
            mySqlList.Sort(( a, b ) => b.ID - a.ID);
            return mySqlList;
        }

        public ServerResult<Object> AddMySql( string host, string port, string user, string password,  string db_name, string name,  string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("host", host);
            dic.Add("port", port);
            dic.Add("user", user);
            dic.Add("password", password);
            dic.Add("db_name", db_name);
            dic.Add("name", name);
            dic.Add("notice_target", notice_target);
            var result = HttpHelper.Get(this.addMySqlUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> UpdateMySql( int id, string host, string port, string user, string password, string db_name, string name, string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("host", host);
            dic.Add("port", port);
            dic.Add("user", user);
            dic.Add("password", password);
            dic.Add("db_name", db_name);
            dic.Add("name", name);
            dic.Add("notice_target", notice_target);
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.updateMySqlUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> DeleteMySql( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.deleteMySqlUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }


        public MySql GetMySqlItem( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.mySqlItemUrl, dic, this.key );
            var rt = JsonConvert.DeserializeObject<MySql>( result );
            if( rt == null ) {
                return new MySql();
            }
            var status = GetMySqlStatus();
            var currStatus = status.Find( item => item.ID == rt.ID );
            if( currStatus != null ) {
                rt.Delay = currStatus.Delay;
            }
            return rt;
        }

        public List<EntityStatus> GetMySqlStatus() {
            var dic = new Dictionary<string, string>();
            var result = HttpHelper.Get(this.mySqlStatusUrl, dic, this.key );
            if( string.IsNullOrEmpty( result ) ) {
                return new List<EntityStatus>();
            }
            return JsonConvert.DeserializeObject<List<EntityStatus>>(result);
        }


        #endregion

        #region

        public List<Process> GetProcessList() {
            var json = HttpHelper.Get(this.processListUrl, new Dictionary<string, string>(), this.key );
            List<Process> result = JsonConvert.DeserializeObject<List<Process>>(json);
            if( result == null ) {
                return new List<Process>();
            }
            result.Sort((a, b) => b.ID - a.ID);
            return result;
        }

        public ServerResult<Object> AddProcess(  string processName, string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("process_name", processName);
            dic.Add("notice_target", notice_target);
            var result = HttpHelper.Get(this.addProcessUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> UpdateProcess( int id, string processName, string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("process_name", processName);
            dic.Add("notice_target", notice_target);
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.updateProcessUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> DeleteProcess( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.deleteProcessUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public Process GetProcessItem( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.processItemUrl, dic, this.key );
            return JsonConvert.DeserializeObject<Process>(result);
        }

       
        public List<EntityStatus> GetProcessStatus() {
            var dic = new Dictionary<string, string>();
            var result = HttpHelper.Get(this.processStatusUrl,dic, this.key );
            if( string.IsNullOrEmpty( result ) ) {
                return new List<EntityStatus>();
            }
            return JsonConvert.DeserializeObject<List<EntityStatus>>(result);
        }


        #endregion

        #region
        public List<HttpUrl> GetUrlList() {
            var json = HttpHelper.Get(this.urlListUrl, new Dictionary<string, string>(), this.key );
            var result = JsonConvert.DeserializeObject<List<HttpUrl>>(json);
            if( result == null ) {
                return new List<HttpUrl>();
            }
            var status = GetUrlStatus();
            result = result.Select( item => {
                var statusItem = status.Find( sItem => sItem.ID == item.ID );
                if( statusItem != null ) {
                    item.Delay = statusItem.Delay;
                }
                return item;
            } ).ToList();
            result.Sort((a, b) => b.ID - a.ID);
            return result;
        }

        public ServerResult<Object> AddUrl( string name, string method, string url, string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("name", name);
            dic.Add("method", method);
            dic.Add("url", url);
            dic.Add("notice_target", notice_target);
            var result = HttpHelper.Get(this.addUrlUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> updateUrl( int id, string name, string method, string url, string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("name", name);
            dic.Add("method", method);
            dic.Add("url", url);
            dic.Add("notice_target", notice_target);
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.updateUrlUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> DeleteUrl( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.deleteUrlUrl, dic, this.key );
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public HttpUrl GetUrlItem( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.urlItemUrl, dic, this.key );
            var rt = JsonConvert.DeserializeObject<HttpUrl>(result);
            if( rt == null ) {
                return new HttpUrl();
            }
            var status = GetUrlStatus();
            var currStatus = status.Find( item => item.ID == rt.ID );
            if( currStatus != null ) {
                rt.Delay = currStatus.Delay;
            }
            return rt;
        }


        public List<EntityStatus> GetUrlStatus() {
            var dic = new Dictionary<string, string>();
            var result = HttpHelper.Get(this.urlStatusUrl, dic, this.key );
            if( string.IsNullOrEmpty( result ) ) {
                return new List<EntityStatus>();
            }
            return JsonConvert.DeserializeObject<List<EntityStatus>>(result);
        }

        public List<HistoryItem> GetHistoryList() {
            var dic = new Dictionary<string, string>();
            var result = HttpHelper.Get( this.urlHistoryList, dic, this.key );
            if( string.IsNullOrEmpty( result ) ) {
                return new List<HistoryItem>();
            }
            return JsonConvert.DeserializeObject<List<HistoryItem>>( result );
        }

        public List<HistoryDetailItem> GetHistoryDetail(HistoryItem historyItem) {
            var historyContentUrl = this.baseUrl + "/history/" + historyItem.FileName;
            var historyContent = HttpHelper.Get( historyContentUrl, new Dictionary<string, string>(), this.key );
            var result = historyContent.Split( '\n' ).Select( item => item.Split( '|' ) ).Select( item => {
                if( item.Length >= 4 ) {
                    var historyInfo = new HistoryDetailItem();
                    historyInfo.TimeString = item[ 0 ];
                    historyInfo.Type = item[ 1 ];
                    historyInfo.GlobalID = item[ 2 ];
                    historyInfo.Message = item[ 3 ];
                    return historyInfo;
                } else {
                    return null;
                }
            } ).Where( item => item != null ).ToList();

            result.Sort( ( a, b ) => {
                return (int)( b.Time - a.Time ).TotalMilliseconds;
            } );
            return result;
        }

        #endregion
    }
}
