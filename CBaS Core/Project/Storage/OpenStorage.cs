using System;
using System.Collections.Generic;
using CBaSCore.Project.Business;
using Microsoft.Data.Sqlite;

namespace CBaSCore.Project.Storage
{
    public class OpenStorage
    {
        public List<StructureModel> GetProjectStructure()
        {
            var items = new List<StructureModel>();
            using (var conn = DatabaseHandler.GetInstance().GetConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM PROJECTSTRUCTURE;";

                using (var command = new SqliteCommand(sql, conn))
                {
                    var reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            var parentID = 0;
                            if (!(reader["PARENT_ID"] is DBNull)) parentID = Convert.ToInt32(reader["PARENT_ID"]);

                            ProjectItemEnum itemType;
                            Enum.TryParse(Convert.ToString(reader["TYPE"]), out itemType);

                            items.Add(new StructureModel
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                ParentID = parentID,
                                Name = Convert.ToString(reader["NAME"]),
                                Type = itemType,
                                FileExtension = Convert.ToString(reader["FILE_EXTENSION"]) is DBNull
                                    ? ""
                                    : Convert.ToString(reader["FILE_EXTENSION"])
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            return items;
        }
    }
}