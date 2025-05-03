using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp222
{
    public partial class CreateTemplateForm : Form
    {
        public static string nameTemplate; 
        public CreateTemplateForm()
        {
            InitializeComponent();
            CreateSortCheckboxes();
            CreateProcessCheckboxes();
            CreateWorkWithFilesCheckboxes();

        }

        private void btnOk_Click (object sender, EventArgs e)
        {
            TemplateNameForm tn = new TemplateNameForm();
            DialogResult result = tn.ShowDialog();

            if (result == DialogResult.OK)
            {
                DBInit.InsertTemplate(sortOptions, processOptions, workWithFilesOptions);
                this.Close();
            }
        }

        // Обработка выбора пункта меню во второй колонке
        private void ListView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0) // Проверяем, есть ли выбранные элементы
            {
                string selectedMenu = listView2.SelectedItems[0].Text;
                if (selectedMenu == "Сортировки")
                {
                    sortPanel.Visible = true;
                    processPanel.Visible = false;
                    workWithFilesPanel.Visible = false;
                    currentMenu = "Сортировки";
                }
                else if (selectedMenu == "Обработки строк")
                {
                    sortPanel.Visible = false;
                    workWithFilesPanel.Visible = false;
                    processPanel.Visible = true;
                    currentMenu = "Обработки строк";
                }
                else if (selectedMenu == "Работа с другими файлами")
                {
                    sortPanel.Visible = false;
                    processPanel.Visible = false;
                    workWithFilesPanel.Visible = true;
                    currentMenu = "Работа с другими файлами";
                }
          
            }
            // Необязательно: можно добавить обработку случая, когда ничего не выбрано
            // Например: else { MessageBox.Show("Выберите пункт меню!"); }
        }


        // Создание чекбоксов для сортировки
        private void CreateSortCheckboxes()
        {
            string[] sortOptions = { "По домену A-Z", "По значению Total (100-0)", "Выполнить обратную сортировку" };
            for (int i = 0; i < sortOptions.Length; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Text = sortOptions[i],
                    Location = new Point(10, 20 * i),
                    AutoSize = true
                };
                cb.CheckedChanged += SortCheckBox_CheckedChanged;
                sortPanel.Controls.Add(cb);
            }
        }

        // Обработчик для чекбоксов сортировки
        private void SortCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                if (!sortOptions.Contains(cb.Text))
                    sortOptions.Add(cb.Text);

                if (cb.Text == "По домену A-Z")
                {
                    FormSortPriority form = new FormSortPriority();
                    form.ShowDialog();
                }
            }
            else
            {
                sortOptions.Remove(cb.Text);
            }
        }

        // Создание чекбоксов для обработки строк
        private void CreateProcessCheckboxes()
        {
            string[] processOptions = { "Удалить повторы", "Удаление строк по КС", "Удаление строк без КС", "Удаление содержимого до КС", "Удаление содержимого после КС" };
            for (int i = 0; i < processOptions.Length; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Text = processOptions[i],
                    Location = new Point(10, 20 * i),
                    AutoSize = true
                };
                cb.CheckedChanged += ProcessCheckBox_CheckedChanged;
                processPanel.Controls.Add(cb);
            }
        }

        // Обработчик для чекбоксов обработки строк
        private void ProcessCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                if (!processOptions.Contains(cb.Text))
                    processOptions.Add(cb.Text);

                if (cb.Text == "Удаление строк по КС")
                {
                    FormKeyWordDelLines form = new FormKeyWordDelLines();
                    form.ShowDialog();

                    if (FormKeyWordDelLines.DelLinesKeyword == null || FormKeyWordDelLines.DelLinesKeyword.Length == 0)
                    {
                        cb.Checked = false;
                        processOptions.Remove(cb.Text);
                    }
                }
                else if (cb.Text == "Удаление строк без КС")
                {
                    FormWithoutKeyWordDelLines form = new FormWithoutKeyWordDelLines();
                    form.ShowDialog();

                    if (FormWithoutKeyWordDelLines.DelLinesWithoutKeyword == null || FormWithoutKeyWordDelLines.DelLinesWithoutKeyword.Length == 0)
                    {
                        cb.Checked = false;
                        processOptions.Remove(cb.Text);
                    }
                }
                else if (cb.Text == "Удаление содержимого до КС")
                {
                    FormKeyWordDelLineContentBefore form = new FormKeyWordDelLineContentBefore();
                    form.ShowDialog();

                    if (FormKeyWordDelLineContentBefore.DelLineContentBeforeKeyword == null || FormKeyWordDelLineContentBefore.DelLineContentBeforeKeyword.Length == 0)
                    {
                        cb.Checked = false;
                        processOptions.Remove(cb.Text);
                    }
                }
                else if (cb.Text == "Удаление содержимого после КС")
                {
                    FormKeyWordDelLineContentAfter form = new FormKeyWordDelLineContentAfter();
                    form.ShowDialog();

                    if (FormKeyWordDelLineContentAfter.DelLineContentAfterKeyword == null || FormKeyWordDelLineContentAfter.DelLineContentAfterKeyword.Length == 0)
                    {
                        cb.Checked = false;
                        processOptions.Remove(cb.Text);
                    }
                }
            }
            else
            {
                processOptions.Remove(cb.Text);
            }
        }

        // Создание чекбоксов для работы с другими файлами
        private void CreateWorkWithFilesCheckboxes()
        {
            string[] workWithFilesOptions = { "Разность текстовиков", "Объединение по имени файла" };
            for (int i = 0; i < workWithFilesOptions.Length; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Text = workWithFilesOptions[i],
                    Location = new Point(10, 20 * i),
                    AutoSize = true
                };
                cb.CheckedChanged += WorkWithFilesCheckBox_CheckedChanged;
                workWithFilesPanel.Controls.Add(cb);
            }
        }

        // Обработчик для чекбоксов работы с другими файлами
        private void WorkWithFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {

                if (!workWithFilesOptions.Contains(cb.Text))
                    workWithFilesOptions.Add(cb.Text);

                if (cb.Text == "Разность текстовиков")
                {
                    TxtDifference form = new TxtDifference();
                    form.ShowDialog();

                    if (string.IsNullOrWhiteSpace(TxtDifference.CombineTxtDifference))
                    {
                        cb.Checked = false;
                        workWithFilesOptions.Remove(cb.Text);
                    }
                }
                //else if (cb.Text == "Объединение по имени файла")
                //{
                //    if (/*selectedFiles.Count < 1*/ 1 < 3)
                //    {
                //        MessageBox.Show("Вы выбрали недостаточное количество файлов для объединения");
                //        cb.Checked = false;
                //        workWithFilesOptions.Remove(cb.Text);
                //    }
                //    else
                //    {
                //        FileNameCombine form = new FileNameCombine();
                //        form.ShowDialog();
                //        if (string.IsNullOrWhiteSpace(FileNameCombine.combineByFileName))
                //        {
                //            cb.Checked = false;
                //            workWithFilesOptions.Remove(cb.Text);
                //        }
                //    }
                //}
            }
            else
            {
                workWithFilesOptions.Remove(cb.Text);
            }
        }
    }
}
