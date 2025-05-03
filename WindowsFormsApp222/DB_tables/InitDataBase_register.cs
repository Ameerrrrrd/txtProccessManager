using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;

namespace WindowsFormsApp222
{

    internal class InitDataBase_register
    {
        public static void RegisterApprovation(string email, string username, string password, string connectionString)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE email = @email";
                using (MySqlCommand checkCmd = new MySqlCommand(checkEmailQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@email", email);
                    int emailCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (emailCount > 0)
                    {
                        MessageBox.Show("Этот email уже зарегистрирован.");
                        return;
                    }
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

                string insertUserQuery = "INSERT INTO Users (email, login) VALUES (@email, @login)";
                using (MySqlCommand insertUserCmd = new MySqlCommand(insertUserQuery, conn))
                {
                    insertUserCmd.Parameters.AddWithValue("@email", email);
                    insertUserCmd.Parameters.AddWithValue("@login", username);
                    insertUserCmd.ExecuteNonQuery();
                }
                long userId;
                using (MySqlCommand getIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
                {
                    userId = Convert.ToInt64(getIdCmd.ExecuteScalar());
                }

                string insertPwdQuery = "INSERT INTO Users_pwd (user_ID, salt, hashed_password) VALUES (@userId, @salt, @hashedPassword)";
                using (MySqlCommand insertPwdCmd = new MySqlCommand(insertPwdQuery, conn))
                {
                    insertPwdCmd.Parameters.AddWithValue("@userId", userId);
                    insertPwdCmd.Parameters.AddWithValue("@salt", salt);
                    insertPwdCmd.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                    insertPwdCmd.ExecuteNonQuery();
                }

                string selectUserQuery = "SELECT u.user_ID, u.email, u.login, p.salt, p.hashed_password " +
                                         "FROM Users u " +
                                         "JOIN Users_pwd p ON u.user_ID = p.user_ID " +
                                         "WHERE u.user_ID = @userId";
                using (MySqlCommand selectCmd = new MySqlCommand(selectUserQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@userId", userId);
                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"User ID: {reader["user_ID"]}");
                            Console.WriteLine($"Email: {reader["email"]}");
                            Console.WriteLine($"Login: {reader["login"]}");
                            Console.WriteLine($"Salt: {reader["salt"]}");
                            Console.WriteLine($"Hashed Password: {reader["hashed_password"]}");
                        }
                        Form1.currentUserId = Convert.ToInt32(reader["user_ID"]);
                        Form1.isMethodCompleted = true;
                    }
                }

            }
        }
    }
}
