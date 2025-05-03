using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp222
{
    partial class CreateTemplateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Настройка формы
            this.Size = new Size(550, 350);
            this.Text = "Создать шаблон";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Icon = Properties.Resources.cat;

            // Вторая колонка (ListView2)
            listView2 = new ListView
            {
                Location = new Point(10, 40),
                Size = new Size(220, 220),
                View = View.Details,
                FullRowSelect = true,
            };
            listView2.Columns.Add("Меню", 220);
            listView2.Items.Add("Сортировки");
            listView2.Items.Add("Обработки строк");
            listView2.Items.Add("Работа с другими файлами");
            listView2.SelectedIndexChanged += ListView2_SelectedIndexChanged;

            // Контейнер для кнопки "Выполнить" и панелей с чекбоксами
            optionsPanel = new Panel
            {
                Location = new Point(240, 40),
                Size = new Size(370, 510)
            };
            btnOk = new Button
            {
                Text = "ОК",
                Location = new Point(200, 270),
                Size = new Size(100, 30),
            };
            btnOk.Click += btnOk_Click;
            // Панель для чекбоксов сортировки
            sortPanel = new Panel
            {
                Location = new Point(0, 40), // Отступ 10 пикселей от кнопки
                Size = new Size(370, 200),
                Visible = false
            };

            // Панель для чекбоксов обработки строк
            processPanel = new Panel
            {
                Location = new Point(0, 40), // Отступ 10 пикселей от кнопки
                Size = new Size(370, 200),
                Visible = false
            };

            // Панель для чекбоксов работы с другими файлами
            workWithFilesPanel = new Panel
            {
                Location = new Point(0, 40), // Отступ 10 пикселей от кнопки
                Size = new Size(370, 200),
                Visible = false
            };

            optionsPanel.Controls.Add(sortPanel);
            optionsPanel.Controls.Add(processPanel);
            optionsPanel.Controls.Add(workWithFilesPanel);

            this.Controls.Add(btnOk);
            this.Controls.Add(listView2);
            this.Controls.Add(optionsPanel); // Добавляем контейнер на форму
        }



        private ListView listView2;         // Вторая колонка с меню сортировок и обработок
        private Panel optionsPanel;         // Контейнер для кнопки "Сохранить" и панелей с чекбоксами
        private Panel sortPanel;            // Панель для чекбоксов сортировки
        private Panel processPanel;         // Панель для чекбоксов обработки строк
        private Panel workWithFilesPanel;
        private Button btnOk;
        private string currentMenu;         // Текущий выбранный пункт меню

        private List<string> sortOptions = new List<string>();      // Список выбранных опций сортировки
        private List<string> processOptions = new List<string>();   // Список выбранных опций обработки
        private List<string> workWithFilesOptions = new List<string>();
        #endregion
    }
}