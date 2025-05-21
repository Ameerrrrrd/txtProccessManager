using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_KAZLOVSKIY
{
    public partial class TemplateNameForm : Form
    {
        public static string templateName;
        CreateTemplateForm ct = new CreateTemplateForm();

        private Label lblKeyword;
        private TextBox txtKeyword;
        private Button btnOk;

        public TemplateNameForm()
        {
            InitializeComponentе();
        }

        private void InitializeComponentе()
        {
            this.Text = "Название шаблона";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(300, 175);
            this.Icon = Properties.Resources.cat;

            lblKeyword = new Label() { Text = "Введите название шаблона", AutoSize = true, Location = new System.Drawing.Point(50, 40) };

            txtKeyword = new TextBox() { Location = new System.Drawing.Point(50, 75), Width = 180 };
            txtKeyword.KeyDown += TxtKeyword_KeyDown;

            btnOk = new Button() { Text = "ОК", Location = new System.Drawing.Point(190, 100), Width = 50 };
            btnOk.Click += BtnOk_Click;

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
            templateName = txtKeyword.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}