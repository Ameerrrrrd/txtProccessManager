using System;
using System.Windows.Forms;

namespace WindowsFormsApp222
{
    public partial class FormKeyWordDelLineContentAfter : Form
    {
        public static string DelLineContentAfterKeyword { get; private set; } = string.Empty;

        private Label lblTitle;
        private Label lblKeyword;
        private TextBox txtKeyword;
        private Button btnOk;

        public FormKeyWordDelLineContentAfter()
        {
            InitializeComponentе();
        }

        private void InitializeComponentе()
        {
            this.Text = "Удаление содержимого";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(300, 180);

            lblTitle = new Label() { Text = "Удаление содержимого после КС", AutoSize = true, Location = new System.Drawing.Point(30, 10) };
            lblKeyword = new Label() { Text = "Введите ключевое слово", AutoSize = true, Location = new System.Drawing.Point(50, 40) };

            txtKeyword = new TextBox() { Location = new System.Drawing.Point(50, 60), Width = 180 };
            txtKeyword.KeyDown += TxtKeyword_KeyDown;

            btnOk = new Button() { Text = "ОК", Location = new System.Drawing.Point(190, 90), Width = 50 };
            btnOk.Click += BtnOk_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblKeyword);
            this.Controls.Add(txtKeyword);
            this.Controls.Add(btnOk);

            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            SaveAndClose();
        }

        private void TxtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveAndClose();
                e.SuppressKeyPress = true;
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Tab)
            {
                txtKeyword.Focus();
                e.SuppressKeyPress = true;
            }
        }

        private void SaveAndClose()
        {
            DelLineContentAfterKeyword = txtKeyword.Text;
            this.Close();
        }
    }
}
