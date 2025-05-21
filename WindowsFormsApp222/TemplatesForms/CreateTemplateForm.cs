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
    public partial class CreateTemplateForm : Form
    {
        public delegate void TemplateCreatedHandler(string name);
        public event TemplateCreatedHandler OnTemplateCreated;


        public CreateTemplateForm()
        {
            InitializeComponent();
            CreateSortCheckboxes();
            CreateProcessCheckboxes();
            CreateWorkWithFilesCheckboxes();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            TemplateNameForm tn = new TemplateNameForm();
            DialogResult result = tn.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrEmpty(TemplateNameForm.templateName))
            {
                InitDatabase_templates.InsertTemplate(sortOptions, processOptions, workWithFilesOptions, Form1.currentUserId);
                OnTemplateCreated?.Invoke(TemplateNameForm.templateName);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ListView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                string selectedMenu = listView2.SelectedItems[0].Text;
                sortPanel.Visible = selectedMenu == "Сортировки";
                processPanel.Visible = selectedMenu == "Обработки строк";
                workWithFilesPanel.Visible = selectedMenu == "Работа с другими файлами";
            }
        }

        private void CreateSortCheckboxes()
        {
            string[] options = { "По домену A-Z", "По значению Total (100-0)", "Выполнить обратную сортировку" };
            for (int i = 0; i < options.Length; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Text = options[i],
                    Location = new Point(10, 20 * i),
                    AutoSize = true
                };
                cb.CheckedChanged += SortCheckBox_CheckedChanged;
                sortPanel.Controls.Add(cb);
            }
        }

        private void SortCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                if (!sortOptions.Contains(cb.Text))
                    sortOptions.Add(cb.Text);
            }
            else
            {
                sortOptions.Remove(cb.Text);
            }
        }

        private void CreateProcessCheckboxes()
        {
            string[] options = { "Удалить повторы", "Удаление строк по КС", "Удаление строк без КС", "Удаление содержимого до КС", "Удаление содержимого после КС" };
            for (int i = 0; i < options.Length; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Text = options[i],
                    Location = new Point(10, 20 * i),
                    AutoSize = true
                };
                cb.CheckedChanged += ProcessCheckBox_CheckedChanged;
                processPanel.Controls.Add(cb);
            }
        }

        private void ProcessCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                if (!processOptions.Contains(cb.Text))
                    processOptions.Add(cb.Text);
            }
            else
            {
                processOptions.Remove(cb.Text);
            }
        }

        private void CreateWorkWithFilesCheckboxes()
        {
            string[] options = { "Разность текстовиков", "Объединение по имени файла" };
            for (int i = 0; i < options.Length; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Text = options[i],
                    Location = new Point(10, 20 * i),
                    AutoSize = true
                };
                cb.CheckedChanged += WorkWithFilesCheckBox_CheckedChanged;
                workWithFilesPanel.Controls.Add(cb);
            }
        }

        private void WorkWithFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                if (!workWithFilesOptions.Contains(cb.Text))
                    workWithFilesOptions.Add(cb.Text);
            }
            else
            {
                workWithFilesOptions.Remove(cb.Text);
            }
        }
    }
}
