using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;
using MySql.Data.MySqlClient;

namespace ServerCore
{
    class ServerCore : Script
    {

        private MysqlConnection connection;
        private XmlGroup databaseConfiguration;

        public ServerCore()
        {

            API.onResourceStart += onResourceStart;

        }

        public void onResourceStart()
        {

            databaseConfiguration = API.loadConfig("database.xml");

            string host = null;
            string port = null;
            string user = null;
            string password = null;
            string database = null;

            foreach (xmlElement element in databaseConfiguration.getElementsByType("setting"))
            {

                string settingName = element.getElementData<string>("name");
                string settingContent = element.getElementData<string>("content");

                if (settingName.ToLower().Equals("host"))
                    host = settingContent;
                else if (settingName.ToLower().Equals("port"))
                    port = settingContent;
                else if (settingName.ToLower().Equals("user"))
                    user = settingContent;
                else if (settingName.ToLower().Equals("password"))
                    password = settingContent;
                else if (settingName.ToLower().Equals("database"))
                    database = settingContent;
            }

            if (port == null)
                connection = new MysqlConnection(host, user, password, database);
            else
                connection = new MysqlConnection(host, port, user, password, database);

        }

        public MysqlConnection getMysqlConnection() { return connection; }

        public MySqlDataReader executeQuery(string query, Dictionary<string, string> args)
        {

            List<MySqlParameter> mysqlArgs = new List<MySqlParameter>();

            foreach (string argName in args.Keys)
            {

                MySqlParameter param = new MySqlParameter(argName, args.Get(argName));
                mysqlArgs.Add(param);

            }

            return connection.query(query, mysqlArgs.ToArray());

        }

    }
}
