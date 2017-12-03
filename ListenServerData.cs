using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Data;
using sys_monitor_tool.entity;

namespace sys_monitor_tool
{
    class ListenServerData
    {
        static ListenServerData()
        {
            CreateSourceFile();
        }

        const String SRC_FILE_NAME = "list_server_data.xml";

        public static bool CreateSourceFile()
        {
            if (!File.Exists(SRC_FILE_NAME))
            {
                var fs = File.Create(SRC_FILE_NAME);
                using (var sw = new StreamWriter(fs))
                {
                    var xmlString = @"<?xml version='1.0' encoding='utf-8'?>";
                    xmlString += "<ServerList></ServerList>";
                    sw.Write(xmlString);
                    sw.Flush();
                }
                return true;
            }
            return false;
        }

        private static string GenID(string host, int httpPort)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(host + httpPort, "MD5");
        }

        private static XmlElement GetElementById(XmlDocument doc, string id)
        {
            var eles = doc.GetElementsByTagName("ServerItem");
            var result = eles.Cast<XmlElement>().Where(item => item.GetAttribute("ID") == id);
            return result.FirstOrDefault();
        }

        public static bool AddServer(string name, string host, int httpPort, string key)
        {
            var ID = GenID(host, httpPort);
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(SRC_FILE_NAME);
            if (GetElementById(xmlDoc, ID) != null)
            {
                return false;
            }
            var newName = xmlDoc.CreateElement("Name");
            newName.InnerText = name;
            var newHost = xmlDoc.CreateElement("Host");
            newHost.InnerText = host;
            var newHttpPort = xmlDoc.CreateElement("HttpPort");
            newHttpPort.InnerText = httpPort.ToString();
            var newStatus = xmlDoc.CreateElement("Status");
            newStatus.InnerText = "未知";
            var newKey = xmlDoc.CreateElement( "Key" );
            newKey.InnerText = key;
            var newServerItem = xmlDoc.CreateElement("ServerItem");
            newServerItem.AppendChild(newName);
            newServerItem.AppendChild(newHost);
            newServerItem.AppendChild(newHttpPort);
            newServerItem.AppendChild(newStatus);
            newServerItem.AppendChild( newKey );
            newServerItem.SetAttribute("ID", ID);
            var serverListElement = xmlDoc.GetElementsByTagName("ServerList")[0];
            serverListElement.AppendChild(newServerItem);
            xmlDoc.Save(SRC_FILE_NAME);
            return true;
        }

        public static DataTable GetServerList()
        {
            try
            {
                var dataSet = new DataSet();
                dataSet.ReadXml(SRC_FILE_NAME);
                var dt = dataSet.Tables[0];
                if( !dt.Columns.Contains( "Key" ) ) {
                    dt.Columns.Add( "Key", typeof( string ) );
                }
                var dv = dt.DefaultView;
                dv.Sort = "ID desc";
                return dv.ToTable();
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public static bool Delete(string id )
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(SRC_FILE_NAME);
            var ele = GetElementById(xmlDoc, id);
            if (ele == null) return false;
            ele.ParentNode.RemoveChild(ele);
            xmlDoc.Save(SRC_FILE_NAME);
            return true;
        }

        public static ListenServerItem GetServerItem( string id )
        {
            var result = new ListenServerItem();
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(SRC_FILE_NAME);
            var ele = GetElementById(xmlDoc, id);
            if( ele == null )
            {
                return result;
            }
            var host = ele.GetElementsByTagName("Host")[0].InnerText;
            var httpPort = ele.GetElementsByTagName("HttpPort")[0].InnerText;
            var name = ele.GetElementsByTagName("Name") [0].InnerText;
            var key = ele.GetElementsByTagName( "Key" );
            if( key.Count > 0 ) {
                result.Key = key[ 0 ].InnerText;
            }
            result.Name = name;
            result.Host = host;
            result.HttpPort = httpPort;
            result.ID = id;
            return result;
        }
    }
}
