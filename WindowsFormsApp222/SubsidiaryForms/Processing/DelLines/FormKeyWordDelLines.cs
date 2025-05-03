using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp222
{
    public partial class FormKeyWordDelLines : Form
    {
        public static string[] DelLinesKeyword { get; private set; } = Array.Empty<string>();

        private Label lblTitle;
        private Label lblKeyword;
        private Label lblKeyword1;
        private TextBox txtKeyword;
        private Button btnOk;

        public FormKeyWordDelLines()
        {
            InitializeComponentе();
        }

        private void InitializeComponentе()
        {
            this.Text = "Удаление строк";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(300, 175);

            lblTitle = new Label() { Text = "Удаление строк по КС", AutoSize = true, Location = new System.Drawing.Point(60, 10) };
            lblKeyword = new Label() { Text = "Введите ключевые слова", AutoSize = true, Location = new System.Drawing.Point(50, 40) };
            lblKeyword1 = new Label() { Text = "(несколько через пробел)", AutoSize = true, Location = new System.Drawing.Point(50, 55) };

            txtKeyword = new TextBox() { Location = new System.Drawing.Point(50, 75), Width = 180 };
            txtKeyword.KeyDown += TxtKeyword_KeyDown;

            btnOk = new Button() { Text = "ОК", Location = new System.Drawing.Point(190, 100), Width = 50 };
            btnOk.Click += BtnOk_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblKeyword);
            this.Controls.Add(lblKeyword1);
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
            DelLinesKeyword = txtKeyword.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.Close();
        }
    }
}