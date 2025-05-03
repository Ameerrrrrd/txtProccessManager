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
    public partial class FormTemplates : Form
    {


        public FormTemplates()
        {
            InitializeComponent();
        }

        // Обработчик события для кнопки "Добавить тестовый шаблон"
        private void AddTestTemplateButton_Click(object sender, EventArgs e)
        {
            CreateTemplateForm ct = new CreateTemplateForm();
            if (ct.ShowDialog() == DialogResult.OK)
            {
                AddTemplateButton(DBInit.LastInsertedTemplateName);
            }
        }


        // Метод для добавления новой кнопки в "Мои шаблоны"
        public void AddTemplateButton(string templateName)
        {
            Button newBtn = new Button
            {
                Text = templateName,
                Size = new Size(myTemplatesPanel.Width - SystemInformation.VerticalScrollBarWidth, 40),
                Margin = new Padding(0, 0, 0, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightGray,
                ForeColor = Color.Black
            };
            newBtn.FlatAppearance.BorderColor = Color.Green;
            newBtn.FlatAppearance.BorderSize = 1;
            myTemplatesPanel.Controls.Add(newBtn);
        }
    }
}
