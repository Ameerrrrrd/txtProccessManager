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
    public partial class FormSavingName : Form
    {
        public static string savingName { get; private set; } = string.Empty;
        public static bool res { get; private set; } = false;

        private Label lblKeyword;
        private TextBox txtKeyword;
        private Button btnOk;

        public FormSavingName()
        {
            InitializeComponentе();
        }

        private void InitializeComponentе()
        {
            this.Text = "Выбор имени для сохранения";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(300, 175);

            //lblTitle = new Label() { Text = "Сортировка строк по КС", AutoSize = true, Location = new System.Drawing.Point(55, 10) };
            lblKeyword = new Label() { Text = "Введите имя для сохранения файла(ов)", AutoSize = true, Location = new System.Drawing.Point(5, 40) };

            txtKeyword = new TextBox() { Location = new System.Drawing.Point(50, 60), Width = 180 };
            txtKeyword.KeyDown += TxtKeyword_KeyDown;

            btnOk = new Button() { Text = "ОК", Location = new System.Drawing.Point(190, 90), Width = 50 };
            btnOk.Click += BtnOk_Click;

            this.Controls.Add(lblKeyword);
            this.Controls.Add(txtKeyword);
            this.Controls.Add(btnOk);

            this.KeyPreview = true;
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

        private void SaveAndClose()
        {
            res = true;
            savingName = txtKeyword.Text;
            this.Close();
        }
    }
}
