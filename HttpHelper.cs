using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace sys_monitor_tool
{
    class HttpHelper
    {

        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数  
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }


        private static string HashPassword( string key, string timestamp ) {
            var express = key + "!single!dog!" + timestamp;
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile( express, "MD5" ).ToLower();
        }


        private static string GetAuthString( string key ) {
            var timestamp = DateTime.Now.Ticks.ToString();
            var secret = HashPassword( key, timestamp );
            var builder = new StringBuilder();
            if( !string.IsNullOrEmpty( key ) ) {
                builder.AppendFormat( "{0}={1}", "timestamp", timestamp );
                builder.Append( "&" );
                builder.AppendFormat( "{0}={1}", "secret", secret );
            }
            return builder.ToString();
        }

        public static string Get(string url, Dictionary<string, string> dic, string key)
        {
            StringBuilder builder = new StringBuilder(url);
            if (dic.Count > 0)
            {
                builder.Append( "?" );
                int i = 0;
                foreach (var item in dic)
                {
                    if( i > 0 ) {
                        builder.Append("&");
                    }
                    builder.AppendFormat("{0}={1}", item.Key, HttpUtility.UrlEncode(item.Value));
                    i++;
                }
            }
            if( dic.Count == 0 ) {
                builder.Append( "?" );
                builder.Append( GetAuthString( key ) );
            } else {
                builder.Append( "&" );
                builder.Append( GetAuthString( key ) );
            }
            try {
                HttpWebRequest req = ( HttpWebRequest ) WebRequest.Create(builder.ToString());
                using( HttpWebResponse resp = ( HttpWebResponse ) req.GetResponse() ) {
                    using( Stream stream = resp.GetResponseStream() ) {
                        using( StreamReader reader = new StreamReader(stream) ) {
                            string result = reader.ReadToEnd();
                            return result;
                        }
                    }
                }
            } catch(WebException wex) {
                AuthMessageManager.ShowMessage(url, wex.Message);
                return "";
            }catch(Exception ex) {  
                return "";
            }
        }

        public static Tuple<bool,string> CheckHttp( string httpUrl) {
            
            HttpWebRequest req = ( HttpWebRequest ) WebRequest.Create(httpUrl);
            try {
                using( HttpWebResponse resp = ( HttpWebResponse ) req.GetResponse() ) {
                   if(resp.StatusCode == HttpStatusCode.OK) {
                        return Tuple.Create(true, "主机和端口通畅");
                    } else {
                        return Tuple.Create(false, "状态码：" + resp.StatusCode.ToString() + "，状态消息：" + resp.StatusDescription);
                    }
                }
            } catch(WebException ex) {
                return Tuple.Create(false, ex.Message);
            }
        }
    }
}
