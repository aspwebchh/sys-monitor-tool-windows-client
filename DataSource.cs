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



        public DataSource(string baseUrl)
        {
            this.baseUrl = baseUrl;

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

        }

        #region
        public List<User> GetUserList()
        {
            var json = HttpHelper.Get(this.userListUrl,new Dictionary<string, string>());
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
            var result = HttpHelper.Get(this.addUserUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> UpdateUser(int id, string name, string mobile, string email)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("name", name);
            dic.Add("mobile", mobile);
            dic.Add("email", email);
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.updateUserUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> DeleteUser(int id)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.deleteUserUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public User GetUserItem(int id)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.userItemUrl, dic);
            return JsonConvert.DeserializeObject<User>(result);
        }
        #endregion

        #region
        public List<MySql> GetMySqlList() {
            var json = HttpHelper.Get(this.mySqlListUrl, new Dictionary<string, string>());
            List<MySql> mySqlList = JsonConvert.DeserializeObject<List<MySql>>(json);
            if( mySqlList == null) {
                mySqlList = new List<MySql>();
            }
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
            var result = HttpHelper.Get(this.addMySqlUrl, dic);
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
            var result = HttpHelper.Get(this.updateMySqlUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> DeleteMySql( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.deleteMySqlUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }


        public MySql GetMySqlItem( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.mySqlItemUrl, dic);
            return JsonConvert.DeserializeObject<MySql>(result);
        }

        public List<EntityStatus> GetMySqlStatus() {
            var dic = new Dictionary<string, string>();
            var result = HttpHelper.Get(this.mySqlStatusUrl, dic);
            return JsonConvert.DeserializeObject<List<EntityStatus>>(result);
        }


        #endregion

        #region

        public List<Process> GetProcessList() {
            var json = HttpHelper.Get(this.processListUrl, new Dictionary<string, string>());
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
            var result = HttpHelper.Get(this.addProcessUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> UpdateProcess( int id, string processName, string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("process_name", processName);
            dic.Add("notice_target", notice_target);
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.updateProcessUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> DeleteProcess( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.deleteProcessUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public Process GetProcessItem( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.processItemUrl, dic);
            return JsonConvert.DeserializeObject<Process>(result);
        }

       
        public List<EntityStatus> GetProcessStatus() {
            var dic = new Dictionary<string, string>();
            var result = HttpHelper.Get(this.processStatusUrl,dic);
            return JsonConvert.DeserializeObject<List<EntityStatus>>(result);
        }


        #endregion

        #region
        public List<HttpUrl> GetUrlList() {
            var json = HttpHelper.Get(this.urlListUrl, new Dictionary<string, string>());
            List<HttpUrl> result = JsonConvert.DeserializeObject<List<HttpUrl>>(json);
            if( result == null ) {
                return new List<HttpUrl>();
            }
            result.Sort((a, b) => b.ID - a.ID);
            return result;
        }

        public ServerResult<Object> AddUrl( string name, string method, string url, string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("name", name);
            dic.Add("method", method);
            dic.Add("url", url);
            dic.Add("notice_target", notice_target);
            var result = HttpHelper.Get(this.addUrlUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> updateUrl( int id, string name, string method, string url, string notice_target ) {
            var dic = new Dictionary<string, string>();
            dic.Add("name", name);
            dic.Add("method", method);
            dic.Add("url", url);
            dic.Add("notice_target", notice_target);
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.updateUrlUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public ServerResult<Object> DeleteUrl( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.deleteUrlUrl, dic);
            return JsonConvert.DeserializeObject<ServerResult<Object>>(result);
        }

        public HttpUrl GetUrlItem( int id ) {
            var dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            var result = HttpHelper.Get(this.urlItemUrl, dic);
            return JsonConvert.DeserializeObject<HttpUrl>(result);
        }


        public List<EntityStatus> GetUrlStatus() {
            var dic = new Dictionary<string, string>();
            var result = HttpHelper.Get(this.urlStatusUrl, dic);
            return JsonConvert.DeserializeObject<List<EntityStatus>>(result);
        }
        #endregion
    }
}
