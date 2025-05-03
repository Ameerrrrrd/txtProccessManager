using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp222
{
    public static class InitDatabase_templates
    {
        private static readonly string ConnectionString = "Server=localhost;Database=lol;Port=3306;Uid=root;Pwd=root";

        public static void ReadTemplates()
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM templates", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(
                                $"ID: {reader["ID"]}, " +
                                $"user_id: {reader["user_id"]}, " +
                                $"Name: {reader["Name"]}, " +
                                $"SortOptions: {reader["SortOptions"]}, " +
                                $"ProcessOptions: {reader["ProcessOptions"]}, " +
                                $"WorkWithFilesOptions: {reader["WorkWithFilesOptions"]}, " +
                                $"KeywordSorting: {reader["KeywordSorting"]}, " +
                                $"KeywordDelLines: {reader["KeywordDelLines"]}, " +
                                $"KeywordWithoutDelLines: {reader["KeywordWithoutDelLines"]}, " +
                                $"KeywordAfterDelContent: {reader["KeywordAfterDelContent"]}, " +
                                $"KeywordBeforeDelContent: {reader["KeywordBeforeDelContent"]}, " +
                                $"txtDifference: {reader["txtDifference"]}"
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с БД: {ex.Message}");
            }
        }

        public static string LastInsertedTemplateName { get; private set; } = string.Empty;

        public static void InsertTemplate(List<string> sortOptions,
                                          List<string> processOptions, 
                                          List<string> workWithFilesOptionsOptions,
                                          long userId)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand(@"
                        INSERT INTO templates (
                            user_id, Name, SortOptions, ProcessOptions, WorkWithFilesOptions,
                            KeywordSorting, KeywordDelLines, KeywordWithoutDelLines,
                            KeywordAfterDelContent, KeywordBeforeDelContent, txtDifference
                        ) VALUES (
                            @userId, @name, @sort, @process, @workFiles,
                            @kwSort, @kwDelLines, @kwNoDelLines,
                            @kwAfterDelContent, @kwBeforeDelContent, @difference
                    )", connection);

                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@name", TemplateNameForm.templateName);
                    command.Parameters.AddWithValue("@sort", string.Join(", ", sortOptions));
                    command.Parameters.AddWithValue("@process", string.Join(", ", processOptions));
                    command.Parameters.AddWithValue("@workFiles", string.Join(", ", workWithFilesOptionsOptions));
                    command.Parameters.AddWithValue("@kwSort", FormSortPriority.SortKeyword);
                    command.Parameters.AddWithValue("@kwDelLines", string.Join(", ", FormKeyWordDelLines.DelLinesKeyword));
                    command.Parameters.AddWithValue("@kwNoDelLines", string.Join(", ", FormWithoutKeyWordDelLines.DelLinesWithoutKeyword));
                    command.Parameters.AddWithValue("@kwAfterDelContent", FormKeyWordDelLineContentAfter.DelLineContentAfterKeyword);
                    command.Parameters.AddWithValue("@kwBeforeDelContent", FormKeyWordDelLineContentBefore.DelLineContentBeforeKeyword);
                    command.Parameters.AddWithValue("@difference", TxtDifference.CombineTxtDifference);

                    command.ExecuteNonQuery();

                    LastInsertedTemplateName = TemplateNameForm.templateName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вставке шаблона: {ex.Message}");
            }
        }

        public static Dictionary<string, string> GetTemplateByName(string name)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var cmd = new MySqlCommand("SELECT * FROM templates WHERE Name = @name", connection);
                    cmd.Parameters.AddWithValue("@name", name);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var result = new Dictionary<string, string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result[reader.GetName(i)] = reader[i].ToString();
                            }
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении шаблона: {ex.Message}");
            }

            return null;
        }

        public static List<string> GetAllTemplateNames()
        {
            var names = new List<string>();

            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var cmd = new MySqlCommand("SELECT Name FROM templates WHERE user_id = @user_id", connection);
                    cmd.Parameters.AddWithValue("@user_id", Form1.currentUserId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении всех шаблонов: {ex.Message}");
            }

            return names;
        }


        public static bool DeleteTemplate(string name)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM templates WHERE Name = @name", conn);
                cmd.Parameters.AddWithValue("@name", name);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
