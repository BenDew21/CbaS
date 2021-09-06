using CBaSCore.Project.Business;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace CBaSCore.Project.Storage
{
    public class AddStorage
    {
        public void CreateDatabase(string filePath)
        {
            //SQLiteConnection.CreateFile(filePath);
            //DatabaseHandler.GetInstance().SetConnectionString(@"Data Source=" + filePath);

            //using (var connection = DatabaseHandler.GetInstance().GetConnection())
            //{
            //    connection.Open();
            //    var query = ProjectDatabaseResources.CreateDatabaseQuery;
            //    using (var command = new SqliteCommand(query, connection))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //}
        }

        public int AddItem(StructureModel model)
        {
            using (var connection = DatabaseHandler.GetInstance().GetConnection())
            {
                connection.Open();
                var query = "INSERT INTO PROJECTSTRUCTURE(PARENT_ID, NAME, TYPE, FILE_EXTENSION) VALUES " +
                    "(@ParentID, @Name, @Type, @FileExtension)";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ParentID", (model.ParentID != 0 ? model.ParentID : (object)DBNull.Value));
                    command.Parameters.AddWithValue("@Name", model.Name);
                    command.Parameters.AddWithValue("@Type", model.Type.ToString());
                    command.Parameters.AddWithValue("@FileExtension", model.FileExtension != null ? model.FileExtension : (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
                var getLastIDQuery = "SELECT last_insert_rowid()";
                using (var command = new SqliteCommand(getLastIDQuery, connection))
                {
                    long x = (long)command.ExecuteScalar();
                    return Convert.ToInt32(x);
                }
            }
        }
    }
}
