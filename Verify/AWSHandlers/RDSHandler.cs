using Amazon.RDS;
using Amazon.RDS.Model;
using Amazon.Runtime;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verify.AWSHandlers
{
    public class RDSHandler : HandlerBase
    {
        public AmazonRDSClient client;
        public NpgsqlConnection conn;


        public void connectToDatabase(String serverName, int port, String userId, String databaseName)
        {
            var AWS_ACCESS_KEY_ID = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var AWS_SECRET_ACCESS_KEY = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            var credentials = new BasicAWSCredentials(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY);
            String pwd = Amazon.RDS.Util.RDSAuthTokenGenerator.GenerateAuthToken(credentials, this.region, serverName, port, userId);
            conn = new NpgsqlConnection($"Server={serverName};User Id={userId};Password={pwd};Database={databaseName}");
            conn.Open();
        }

        public void selectFromTable(String tableName)
        {
            String sqlRequest = "select * from public.\""+tableName+"\"";

            NpgsqlCommand cmd = new NpgsqlCommand(sqlRequest, conn);

            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
                Console.Write("{0}\n", dr[0]);

        }
        public void closeClient()
        {
            conn.Close();
        }
    }
}
