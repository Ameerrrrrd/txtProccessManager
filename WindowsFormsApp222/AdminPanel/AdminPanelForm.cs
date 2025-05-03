using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp222
{
    public partial class AdminPanelForm : Form
    {
        private Panel scrollPanel;
        private const int UserBlockWidth = 200;
        private const int UserBlockHeight = 220;
        private const int MarginSize = 10;
        private const int BlocksPerRow = 4;

        private const string ConnectionString = "Server=localhost;Database=lol;Port=3306;Uid=root;Pwd=root";

        public AdminPanelForm()
        {
            InitializeComponents();
            LoadUsersFromDatabase();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void InitializeComponents()
        {
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.FormClosing += Form1_FormClosing;

            this.Text = "Admin Panel";
            this.Width = 900;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Icon = Properties.Resources.cat;

            scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };

            this.Controls.Add(scrollPanel);
        }

        private void LoadUsersFromDatabase()
        {
            var users = new List<(long UserId, string Email, string Login, List<string> Templates)>();

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string userQuery = "SELECT user_id, email, login FROM users";

                using (var userCmd = new MySqlCommand(userQuery, conn))
                using (var reader = userCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long userId = reader.GetInt64("user_id");
                        string email = reader.GetString("email");
                        string login = reader.GetString("login");
                        users.Add((userId, email, login, new List<string>()));
                    }
                }

                foreach (var (UserId, Email, Login, Templates) in users)
                {
                    string templateQuery = "SELECT Name FROM templates WHERE user_id = @uid";
                    using (var tmplCmd = new MySqlCommand(templateQuery, conn))
                    {
                        tmplCmd.Parameters.AddWithValue("@uid", UserId);
                        using (var tmplReader = tmplCmd.ExecuteReader())
                        {
                            while (tmplReader.Read())
                            {
                                Templates.Add(tmplReader.GetString("Name"));
                            }
                        }
                    }
                }
            }

            RenderUserBlocks(users);
        }

        private void RenderUserBlocks(List<(long UserId, string Email, string Login, List<string> Templates)> users)
        {
            scrollPanel.Controls.Clear();

            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                Panel userBlock = CreateUserBlock(user.UserId, user.Email, user.Login, user.Templates);

                int row = i / BlocksPerRow;
                int col = i % BlocksPerRow;

                userBlock.Location = new Point(
                    MarginSize + col * (UserBlockWidth + MarginSize),
                    MarginSize + row * (UserBlockHeight + MarginSize)
                );

                scrollPanel.Controls.Add(userBlock);
            }
        }

        private Panel CreateUserBlock(long userId, string email, string login, List<string> templates)
        {
            Panel panel = new Panel
            {
                Size = new Size(UserBlockWidth, UserBlockHeight),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.MistyRose,
            };

            Label emailLabel = new Label
            {
                Text = $"Email:  {email}",
                AutoSize = true,
                Location = new Point(5, 5),
            };
            panel.Controls.Add(emailLabel);

            Label loginLabel = new Label
            {
                Text = $"Логин:  {login}",
                AutoSize = true,
                Location = new Point(5, 25),
            };
            panel.Controls.Add(loginLabel);

            Label templatesLabel = new Label
            {
                Text = "Шаблоны:",
                AutoSize = true,
                Location = new Point(5, 45),
            };
            panel.Controls.Add(templatesLabel);

            Panel templateScrollPanel = new Panel
            {
                AutoScroll = true,
                Location = new Point(5, 65),
                Size = new Size(UserBlockWidth - 10, 120),
                BorderStyle = BorderStyle.None
            };

            int xOffset = 0;
            int yOffset = 0;
            int boxWidth = 85;
            int boxHeight = 30;
            int templatePerRow = 2;

            for (int i = 0; i < templates.Count; i++)
            {
                Button tempButton = new Button
                {
                    Text = templates[i],
                    Size = new Size(boxWidth, boxHeight),
                    Location = new Point(
                        xOffset + (i % templatePerRow) * (boxWidth + 5),
                        yOffset + (i / templatePerRow) * (boxHeight + 5)
                    )
                };
                tempButton.Click += TemplateButton_Click;
                templateScrollPanel.Controls.Add(tempButton);
            }

            panel.Controls.Add(templateScrollPanel);

            Button banButton = new Button
            {
                Text = "Забанить?",
                Size = new Size(UserBlockWidth - 20, 30),
                Location = new Point(10, UserBlockHeight - 35),
                Tag = userId
            };
            banButton.Click += BanButton_Click;
            panel.Controls.Add(banButton);

            return panel;
        }

        private void TemplateButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string templateName = button.Text;

                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM templates WHERE Name = @name LIMIT 1";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", templateName);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("Его шаблоны:");
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string colName = reader.GetName(i);
                                    object val = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i);
                                    sb.AppendLine($"{colName}: {val}");
                                }
                                MessageBox.Show(sb.ToString(), "Информация о шаблоне");
                            }
                            else
                            {
                                MessageBox.Show("Шаблон не найден", "Ошибка");
                            }
                        }
                    }
                }
            }
        }

        private void BanButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is long userId)
            {
                var confirm = MessageBox.Show($"Вы уверены, что хотите удалить пользователя с ID {userId}?", "Подтверждение", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    using (var conn = new MySqlConnection(ConnectionString))
                    {
                        conn.Open();

                        using (var cmd = new MySqlCommand("DELETE FROM templates WHERE user_id = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new MySqlCommand("DELETE FROM users_pwd WHERE user_ID = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new MySqlCommand("DELETE FROM users WHERE user_id = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadUsersFromDatabase();
                }
            }
        }
    }
}
