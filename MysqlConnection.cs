using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;
using MySql.Data.MySqlClient;

namespace ServerCore
{
    class MysqlConnection : Script
    {

        private MySqlConnection mysqlConnection;

        private string host;
        private string user;
        private string database;

        public MysqlConnection() { }

        public MysqlConnection(string host, string user, string password, string database)
        {

            this.host = host;
            this.user = user;
            this.database = database;

            openConnection(password);

        }

        public MysqlConnection(string host, string port, string user, string password, string database)
        {

            string fullHost = host + ":" + port;

            this.host = fullHost;
            this.user = user;
            this.database = database;

            openConnection(password);

        }

        public void openConnection(string password)
        {

            if (!isOpen())
            {

                try
                {

                    mysqlConnection = new MySqlConnection("server=" + host + ";uid=" + user + ";pwd=" + password + ";database=" + database + ";");
                    mysqlConnection.Open();

                }
                catch (MySqlException e)
                {

                    API.consoleOutput(e.Message);

                }

                return;

            }

            API.consoleOutput("Mysql: There's already a connection opened!");

        }

        public void closeConnection()
        {

            if (isOpen())
            {

                mysqlConnection.Close();
                return;

            }

            API.consoleOutput("Mysql: The connection is already closed!");

        }

        public bool isOpen()
        {

            if (mysqlConnection == null)
                return false;

            return mysqlConnection.State == ConnectionState.Open;

        }

        public MySqlDataReader query(string query, MySqlParameter[] args)
        {

            if (!isOpen())
                return null;

            MySqlCommand cmd = new MySqlCommand(query, mysqlConnection);
            cmd.Parameters.AddRange(args);

            return cmd.ExecuteReader();

        }

    }
}
