using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Euroland.NetCore.ToolsFramework.Data.Test
{
    public class InitialStoreProcedure
    {
        private string connectionString;
        public InitialStoreProcedure(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void BuildStore(string storeName, string storeBody, string storePara = null)
        {
            if (string.IsNullOrEmpty(storePara))
            {
                storePara = string.Empty;
            }
            string headerStoreTemplate = $"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{storeName}]') AND type in (N'P', N'PC')) \n DROP PROCEDURE [dbo].[{storeName}]";
            string bodyStoreTemplate = $"CREATE PROCEDURE [dbo].[{storeName}] \n {storePara} \n AS \n BEGIN \n {storeBody} \n END";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Exec(headerStoreTemplate, bodyStoreTemplate, connection);
            }
        }

        private static void Exec(string headerStoreTemplate, string bodyStoreTemplate, SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                connection.Open();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = headerStoreTemplate;
                cmd.ExecuteNonQuery();
                cmd.CommandText = bodyStoreTemplate;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
