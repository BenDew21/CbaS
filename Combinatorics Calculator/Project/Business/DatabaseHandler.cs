using Microsoft.Data.Sqlite;

namespace Combinatorics_Calculator.Project.Business
{
    public class DatabaseHandler
    {
        private static DatabaseHandler _instance;
        private static string _connectionString;

        public static DatabaseHandler GetInstance()
        {
            if (_instance == null) _instance = new DatabaseHandler();
            return _instance;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}