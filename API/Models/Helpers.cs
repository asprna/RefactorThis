using System.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace API.Models
{
    public class Helpers
    {
        private const string ConnectionString = "Data Source=D:/Personal Projects/RefactorThis/API/App_Data/products.db";

        public static SqliteConnection NewConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}