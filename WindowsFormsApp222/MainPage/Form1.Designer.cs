using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace KP_KAZLOVSKIY
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>

        private void InitializeComponent()
        {
            // Настройка формы
            this.Size = new Size(830, 600);
            this.Text = "Проводник";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Icon = Properties.Resources.cat;

            // Кнопка "Открыть проводник"
            btnOpenExplorer = new Button
            {
                Location = new Point(40, 10),
                Size = new Size(25, 25),
                BackColor = Color.Blue,
                BackgroundImage = global::KP_KAZLOVSKIY.Properties.Resources.explorer
            };
            btnOpenExplorer.Click += BtnOpenExplorer_Click;

            // Кнопка "Выбрать"
            btnSelectFiles = new Button
            {
                Location = new Point(70, 10),
                Size = new Size(100, 25),
                Text = "Выбрать",
                BackColor = Color.Green
            };
            btnSelectFiles.Click += BtnSelectFiles_Click;

            ToolTip toolTip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 500,
                ReshowDelay = 500,
                ShowAlways = true 
            };
            toolTip.SetToolTip(btnSelectFiles, "ну тут допустим подсказка");
            // Поле для пути
            textBox1 = new TextBox
            {
                Location = new Point(180, 11),
                Size = new Size(280, 25),
                ReadOnly = true
            };

            // Кнопка "Шаблоны"
            btnTemplates = new Button
            {
                Location = new Point(720, 10),
                Size = new Size(80, 25),
                Text = "Шаблоны",

            };
            btnTemplates.Click += BtnTemplates_Click;

            // Первая колонка (ListView1)
            listView1 = new ListView
            {
                Location = new Point(10, 40),
                Size = new Size(280, 510),
                View = View.Details,
                AllowDrop = true,
                FullRowSelect = true
            };
            listView1.Columns.Add("Имя", 280);
            listView1.DragEnter += ListView1_DragEnter;
            listView1.DragDrop += ListView1_DragDrop;
            listView1.Click += ListView1_Click;

            // Вторая колонка (ListView2)
            listView2 = new ListView
            {
                Location = new Point(300, 40),
                Size = new Size(220, 510),
                View = View.Details,
                FullRowSelect = true,
                Enabled = false
            };
            listView2.Columns.Add("Меню", 220);
            listView2.Items.Add("Сортировки");
            listView2.Items.Add("Обработки строк");
            listView2.Items.Add("Работа с другими файлами");            
            listView2.Items.Add("Работа с одним файлом");
            listView2.SelectedIndexChanged += ListView2_SelectedIndexChanged;

            // Контейнер для кнопки "Выполнить" и панелей с чекбоксами
            optionsPanel = new Panel
            {
                Location = new Point(530, 40),
                Size = new Size(370, 510)
            };

            // Кнопка "Выполнить"
            btnExecute = new Button
            {
                Location = new Point(0, 0),
                Size = new Size(270, 30),
                Text = "Сохранить"
            };
            btnExecute.Click += BtnExecute_Click;

            // Панель для чекбоксов сортировки
            sortPanel = new Panel
            {
                Location = new Point(0, 40),
                Size = new Size(370, 200),
                Visible = false
            };

            // Панель для чекбоксов обработки строк
            processPanel = new Panel
            {
                Location = new Point(0, 40),
                Size = new Size(370, 200),
                Visible = false
            };

            // Панель для чекбоксов работы с другими файлами
            workWithFilesPanel = new Panel
            {
                Location = new Point(0, 40),
                Size = new Size(370, 200),
                Visible = false
            };

            // Панель чекбоксов работы с одним файлом
            workWith1FilePanel = new Panel
            {
                Location = new Point(0, 40),
                Size = new Size(370, 200),
                Visible = false
            };

            menuButton = new Button()
            {
                Text = "≡",
                Size = new Size(25, 25),
                Location = new System.Drawing.Point(10, 10),
            };
            menuButton.Click += MenuButton_Click;

            // Create the context menu
            contextMenu = new ContextMenuStrip();

            var aboutHelpItem = new ToolStripMenuItem("About / Help");
            aboutHelpItem.Click += (sender, e) =>
            {
                try
                {
                    MessageBox.Show("прочитайте readme.md с инфой о приложении", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    System.Diagnostics.Process.Start("https://github.com/Ameerrrrrd/txtProccessManager/tree/MySQL");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось открыть ссылку: " + ex.Message);
                }
            };

            var logoutItem = new ToolStripMenuItem("Logout");
            logoutItem.Click += (sender, e) =>
            {
                this.Hide();
               RegisterForm rf = new RegisterForm();
                rf.ShowDialog();
            };

            contextMenu.Items.Add(aboutHelpItem);
            contextMenu.Items.Add(logoutItem);

            // Add controls to the form
            this.Controls.Add(menuButton);

            // ОБработка Ctrl+A
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.FormClosing += Form1_FormClosing;

            // Добавление элементов в контейнер
            optionsPanel.Controls.Add(btnExecute);
            optionsPanel.Controls.Add(sortPanel);
            optionsPanel.Controls.Add(processPanel);
            optionsPanel.Controls.Add(workWithFilesPanel);            
            optionsPanel.Controls.Add(workWith1FilePanel);

            // Добавление элементов на форму
            this.Controls.Add(btnSelectFiles);
            this.Controls.Add(btnOpenExplorer);
            this.Controls.Add(textBox1);
            this.Controls.Add(btnTemplates);
            this.Controls.Add(listView1);
            this.Controls.Add(listView2);
            this.Controls.Add(optionsPanel);

            // Инициализация списка выбранных файлов
            selectedFiles = new List<string>();

        }
        #endregion

        private Button btnTemplates;
        private string selectedPath = string.Empty;
        private ListView listView1;         // Первая колонка для отображения дисков, папок или файлов
        private TextBox textBox1;           // Поле для отображения текущего пути
        private Button btnOpenExplorer;     // Кнопка "Открыть проводник"
        private Button btnSelectFiles;      // Кнопка "Выбрать"
        private string currentPath;         // Текущий путь
        private bool isDriveList;           // Флаг: отображается список дисков
        private bool isFileList;            // Флаг: отображается список выбранных файлов
        private List<string> selectedFiles; // Список выбранных файлов

        private ListView listView2;         // Вторая колонка с меню сортировок и обработок
        private Panel optionsPanel;         // Контейнер для кнопки "Сохранить" и панелей с чекбоксами
        private Panel sortPanel;            // Панель для чекбоксов сортировки
        private Panel processPanel;         // Панель для чекбоксов обработки строк
        private Panel workWithFilesPanel;
        private Panel workWith1FilePanel;
        private Button btnExecute;          // Кнопка "Сохранить"
        private string currentMenu;         // Текущий выбранный пункт меню

        private List<string> sortOptions = new List<string>();      // Список выбранных опций сортировки
        private List<string> processOptions = new List<string>();   // Список выбранных опций обработки
        private List<string> workWithFilesOptions = new List<string>();        
        private List<string> workWith1FileOptions = new List<string>();

        //About, logout
        private Button menuButton;
        private ContextMenuStrip contextMenu;
    }
}

