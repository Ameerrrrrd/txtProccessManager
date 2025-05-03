using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp222
{
    public partial class LoginForm : Form
    {
        string connectionString = "Server=localhost;Database=lol;Port=3306;Uid=root;Pwd=root";
        private TextBox txtEmail;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Button btnLogin;
        private LinkLabel linkToRegister;

        public LoginForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Вход";
            this.Size = new Size(420, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.Icon = Properties.Resources.cat;

            Label titleLabel = new Label
            {
                Text = "Войти в аккаунт",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Top = 20,
                Left = 20
            };

            Label lblEmail = new Label { Text = "Логин/почта", Top = 70, Left = 20, Width = 100, Font = new Font("Segoe UI", 10) };
            txtEmail = new TextBox { Top = lblEmail.Bottom + 5, Left = 20, Width = 360, Font = new Font("Segoe UI", 10) };

            Label lblPassword = new Label { Text = "Пароль", Top = txtEmail.Bottom + 10, Left = 20, Width = 100, Font = new Font("Segoe UI", 10) };
            txtPassword = new TextBox { Top = lblPassword.Bottom + 5, Left = 20, Width = 360, UseSystemPasswordChar = true, Font = new Font("Segoe UI", 10) };

            chkShowPassword = new CheckBox { Text = "Показать пароль", Top = txtPassword.Bottom + 5, Left = 20, Font = new Font("Segoe UI", 9) };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            btnLogin = new Button
            {
                Text = "Войти",
                Top = chkShowPassword.Bottom + 20,
                Left = 20,
                Width = 360,
                Height = 40,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            linkToRegister = new LinkLabel
            {
                Text = "Нет аккаунта? Зарегистрироваться",
                Top = btnLogin.Bottom + 15,
                Left = 20,
                AutoSize = true,
                LinkColor = Color.MediumSeaGreen,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };
            linkToRegister.Click += (s, e) =>
            {
                this.Hide();
                var registerForm = new RegisterForm();
                registerForm.Show();
            };

            this.Controls.AddRange(new Control[]
            {
                titleLabel,
                lblEmail, txtEmail,
                lblPassword, txtPassword,
                chkShowPassword,
                btnLogin,
                linkToRegister
            });

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(LoginForm_Keydown);
            this.FormClosing += LoginForm_FormClosing;
        }


        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                Application.Exit();
        }
        private void LoginForm_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) CheckLogin();
            if (e.KeyCode == Keys.Escape) Application.Exit();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            CheckLogin();
        }

        private void CheckLogin()
        {
            string identifier = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите почту или логин и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtEmail.Text == "admin" && txtPassword.Text == "admin")
            {
                this.Hide();
                AdminPanelForm af = new AdminPanelForm();
                af.ShowDialog();
                return;
            }

            try
            {
                InitDataBase_login.CheckUserLogin(identifier, password, connectionString);
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
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
