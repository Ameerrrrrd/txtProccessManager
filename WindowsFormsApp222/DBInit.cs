// File: DatabaseHelper.cs
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Xml.Linq;

namespace WindowsFormsApp222
{
    public static class DBInit
    {
        private static readonly string DbPath = @"Database(SQLite)\UserTemplates.db";
        private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

        public static void ReadTemplates()
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand("SELECT * FROM templates", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(
                                $"ID: {reader["ID"]}, " +
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

        public static void InsertTemplate(List<string> sortOptions, List<string> processOptions, List<string> workWithFilesOptionsOptions)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand(@"
                        INSERT INTO templates (
                            Name, SortOptions, ProcessOptions, WorkWithFilesOptions,
                            KeywordSorting, KeywordDelLines, KeywordWithoutDelLines,
                            KeywordAfterDelContent, KeywordBeforeDelContent, txtDifference
                        ) VALUES (
                            @name, @sort, @process, @workFiles,
                            @kwSort, @kwDelLines, @kwNoDelLines,
                            @kwAfterDelContent, @kwBeforeDelContent, @difference
                    )", connection);

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
    }
}
