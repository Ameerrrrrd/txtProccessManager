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
    public partial class FormSortPriority : Form
    {
        public static string SortKeyword { get; private set; } = string.Empty;

        private Label lblTitle;
        private Label lblKeyword;
        private TextBox txtKeyword;
        private Button btnOk;

        public FormSortPriority()
        {
            InitializeComponentе();
        }

        private void InitializeComponentе()
        {
            this.Text = "Сортировка по домену";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(300, 175);

            lblTitle = new Label() { Text = "Сортировка по домену", AutoSize = true, Location = new System.Drawing.Point(47, 10) };
            lblKeyword = new Label() { Text = "Приоритет доменов через пробел (не обязательно)", AutoSize = true, Location = new System.Drawing.Point(5, 40) };

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
            SortKeyword = txtKeyword.Text;
            this.Close();
        }
    }
}
