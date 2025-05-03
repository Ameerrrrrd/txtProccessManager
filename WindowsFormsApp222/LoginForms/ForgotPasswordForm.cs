using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp222.LoginForms
{
    public partial class ForgotPasswordForm : Form
    {
        TextBox txtEmail;
        Label lblEmail;
        Button btnSend;
        string connectionString = "Server=localhost;Database=lol;Port=3306;Uid=root;Pwd=root";

        public ForgotPasswordForm()
        {
            InitializeComponente();
        }
        public void InitializeComponente()
        {
            this.Text = "Восстановление пароля";
            this.Size = new Size(430, 190);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblEmail = new Label
            {
                Text = "Введите email для отправки письма с паролем",
                Top = 20,
                Left = 20,
                Width = 400,
                Font = new Font("Segoe UI", 10)
            };

            txtEmail = new TextBox
            {
                Top = lblEmail.Bottom + 10,
                Left = 30,
                Width = 350,
                Font = new Font("Segoe UI", 10)
            };

            btnSend = new Button
            {
                Text = "Отправить письмо",
                Top = txtEmail.Bottom + 10,
                Left = 100,
                Width = 200,
                Height = 30,
                Font = new Font("Segoe UI", 10)
            };
            btnSend.Click += ButtonSend_Click;

            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(btnSend);

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(ForgotPassword_KeyDown);
            this.FormClosing += ForgotPassword_FormClosing;
        }

        public void ButtonSend_Click(object sender, EventArgs e)
        {
            try
            {
                EnterEmailSuccessfully();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }    

        public void EnterEmailSuccessfully ()
        {
            string email = txtEmail.Text;
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Пожалуйста, введите email.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkEmailQuery = "SELECT user_id, login FROM users WHERE email = @email";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkEmailQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@email", email);
                        using (MySqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Email не найден.");
                                return;
                            }

                            int userId = reader.GetInt32("user_id");
                            string login = reader.GetString("login");
                            reader.Close();

                            string newPassword = GenerateRandomPassword(8);

                            string salt = BCrypt.Net.BCrypt.GenerateSalt();
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, salt);

                            string updatePwdQuery = "UPDATE users_pwd SET salt = @salt, hashed_password = @hashedPassword WHERE user_ID = @userId";
                            using (MySqlCommand updateCmd = new MySqlCommand(updatePwdQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@salt", salt);
                                updateCmd.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                                updateCmd.Parameters.AddWithValue("@userId", userId);
                                updateCmd.ExecuteNonQuery();
                            }

                            SendEmail(email, login, newPassword);
                            //MessageBox.Show("Новый пароль отправлен на ваш email.");
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }
            return new string(password);
        }

        private void SendEmail(string toEmail, string login, string password)
        {
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "tutameer@gmail.com"; 
            string smtpPassword = "gdzc clvv jrbe aaov";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(smtpUsername);
            mail.To.Add(toEmail);
            mail.Subject = "Ваши учетные данные";
            mail.Body = $"Логин: {login}\nПароль: {password}";

            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mail);
                MessageBox.Show("Письмо отправлено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка отправки: " + ex.Message);
            }
        }

        private void ForgotPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Hide();
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    EnterEmailSuccessfully();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
                
        }
        private void ForgotPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                this.Hide();
        }
    }
}
