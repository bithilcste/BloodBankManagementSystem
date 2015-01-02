using System.Configuration;
using System.Data.SqlClient;

namespace BloodBankManagementSystem
{
    internal class DatabaseConnection
    {
        public static SqlConnection Connection;

        public static string Constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            Connection = new SqlConnection(Constr);
            Connection.Open();
            return Connection;
        }
    }
}