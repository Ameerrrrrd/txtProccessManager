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
    public interface ITemplateUIHandler  //реализован интерфейс
    {
        void AddTemplateButton(string templateName);
        string SelectedTemplateName { get; }
    }

    public partial class FormTemplates : Form, ITemplateUIHandler
    {
        public FormTemplates()
        {
            InitializeComponent();
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            var names = InitDatabase_templates.GetAllTemplateNames();
            foreach (var name in names)
            {
                if (!string.IsNullOrWhiteSpace(name))
                    AddTemplateButton(name);
            }
        }

        private void AddTestTemplateButton_Click(object sender, EventArgs e)
        {
            CreateTemplateForm ct = new CreateTemplateForm();
            if (ct.ShowDialog() == DialogResult.OK)
            {
                var name = InitDatabase_templates.LastInsertedTemplateName;
                if (!string.IsNullOrWhiteSpace(name))
                    AddTemplateButton(name);
                else
                    MessageBox.Show("Не удалось получить имя нового шаблона.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void AddTemplateButton(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName)) return;

            Button newBtn = new Button
            {
                Text = templateName,
                Size = new Size(myTemplatesPanel.Width - SystemInformation.VerticalScrollBarWidth, 40),
                Margin = new Padding(0, 0, 0, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Tag = templateName
            };
            newBtn.FlatAppearance.BorderColor = Color.Green;
            newBtn.FlatAppearance.BorderSize = 1;

            var contextMenu = new ContextMenuStrip();
            var deleteItem = new ToolStripMenuItem("Удалить");
            deleteItem.Click += (s, e) =>
            {
                string name = newBtn.Tag.ToString();

                if (MessageBox.Show($"Удалить шаблон '{name}'?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (InitDatabase_templates.DeleteTemplate(name))
                    {
                        myTemplatesPanel.Controls.Remove(newBtn);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить шаблон.");
                    }
                }
            };
            contextMenu.Items.Add(deleteItem);
            newBtn.ContextMenuStrip = contextMenu;

            newBtn.Click += TemplateButton_Click;
            myTemplatesPanel.Controls.Add(newBtn);
        }

        public string SelectedTemplateName { get; private set; }

        private void TemplateButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is string templateName)
            {
                if (!string.IsNullOrWhiteSpace(templateName))
                {
                    SelectedTemplateName = templateName;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
