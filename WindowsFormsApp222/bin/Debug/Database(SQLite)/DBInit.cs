using System;
using System.Data.SQLite;

namespace WindowsFormsApp222
{ 
    public static class DBInit
    {
        private static readonly string DbPath = @"Database(SQLite)\UserTemplates.db"; // относительный путь к .db
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
                            Console.WriteLine($"ID: {reader["ID"]}, Name: {reader["Name"]}");
                            // Вывод других полей при необходимости
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с БД: {ex.Message}");
            }
        }
    }
}
