using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp222
{
    internal class InitDataBase_login
    {

        //public InitDataBase_login() { }
        public static void CheckUserLogin (string identifier, string password, string connectionString)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string selectUserQuery = "SELECT u.user_ID, u.email, u.login, p.hashed_password " +
                                        "FROM Users u " +
                                        "JOIN Users_pwd p ON u.user_ID = p.user_ID " +
                                        "WHERE u.email = @identifier OR u.login = @identifier";
                using (MySqlCommand selectCmd = new MySqlCommand(selectUserQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@identifier", identifier);
                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Такой email или логин не зарегистрирован.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string storedHash = reader["hashed_password"].ToString();
                        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, storedHash);

                        if (!isPasswordValid)
                        {
                            MessageBox.Show("Неправильный пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        Form1.currentUserId = Convert.ToInt32(reader["user_ID"]);
                        Form1.isMethodCompleted = true;
                    }
                }
            }
        }
    }
}
