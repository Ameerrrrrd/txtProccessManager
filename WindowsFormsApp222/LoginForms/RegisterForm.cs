using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using Mysqlx.Expr;

namespace KP_KAZLOVSKIY
{
    public partial class RegisterForm : Form
    {
        string connectionString = "Server=localhost;Database=lol;Port=3306;Uid=root;Pwd=root";
        private TextBox txtEmail;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Button btnRegister;
        private LinkLabel linkToLogin;

        public RegisterForm()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Регистрация";
            this.Size = new Size(420, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.Icon = Properties.Resources.cat;

            Label titleLabel = new Label
            {
                Text = "Регистрация",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Top = 20,
                Left = 20
            };

            Label lblEmail = new Label { Text = "Почта", Top = 70, Left = 20, Width = 100, Font = new Font("Segoe UI", 10) };
            txtEmail = new TextBox { Top = lblEmail.Bottom + 5, Left = 20, Width = 360, Font = new Font("Segoe UI", 10) };

            Label lblUsername = new Label { Text = "Имя пользователя", Top = txtEmail.Bottom + 10, Left = 20, Width = 150, Font = new Font("Segoe UI", 10) };
            txtUsername = new TextBox { Top = lblUsername.Bottom + 5, Left = 20, Width = 360, Font = new Font("Segoe UI", 10) };

            Label lblPassword = new Label { Text = "Пароль", Top = txtUsername.Bottom + 10, Left = 20, Width = 100, Font = new Font("Segoe UI", 10) };
            txtPassword = new TextBox { Top = lblPassword.Bottom + 5, Left = 20, Width = 360, UseSystemPasswordChar = true, Font = new Font("Segoe UI", 10) };

            chkShowPassword = new CheckBox { Text = "Показать пароль", Width = 600, Top = txtPassword.Bottom + 5, Left = 20, Font = new Font("Segoe UI", 9) };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            btnRegister = new Button
            {
                Text = "Зарегистрироваться",
                Top = chkShowPassword.Bottom + 20,
                Left = 20,
                Width = 360,
                Height = 40,
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;

            linkToLogin = new LinkLabel
            {
                Text = "Уже есть аккаунт? Войти",
                Top = btnRegister.Bottom + 15,
                Left = 20,
                AutoSize = true,
                LinkColor = Color.SteelBlue,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };
            linkToLogin.Click += (s, e) =>
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            };

            this.Controls.AddRange(new Control[]
            {
                titleLabel,
                lblEmail, txtEmail,
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                chkShowPassword,
                btnRegister,
                linkToLogin
            });

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(RegisterForm_KeyDown);
        }

        private void RegisterForm_KeyDown (object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                CheckRegister();
        }
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            CheckRegister();
        }

        private void CheckRegister()
        {
            string email = txtEmail.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (!Regex.IsMatch(email, @"^[\w\.-]+@[\w\.-]+\.[a-z]{2,4}$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Введите корректный адрес электронной почты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                InitDataBase_register.RegisterApprovation(email, username, password, connectionString);
                if (Form1.isMethodCompleted)
                {
                    MessageBox.Show("Вход выполнен успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Form1 f1 = new Form1();
                    f1.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
