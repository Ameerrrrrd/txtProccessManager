using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KP_KAZLOVSKIY
{
    partial class TxtDifference
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 540);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "TxtDifference";
            this.Icon = Properties.Resources.cat;

            listView1 = new ListView
            {
                Location = new Point(10, 50),
                Size = new Size(280, 450),
                View = View.Details,
                AllowDrop = true,
                FullRowSelect = true
            };
            listView1.Columns.Add("Имя", 280);
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragDrop += listView1_DragDrop;
            listView1.Click += listView1_Click;

            // Кнопка "Выбрать"
            btnSelectFiles = new Button
            {
                Location = new Point(10, 10),
                Size = new Size(280, 30),
                Text = "Выбрать",
            };
            btnSelectFiles.Click += btnSelectFiles_Click;

            LbChooseDirHint = new Label
            {
                Text = "Директория для разности",
                AutoSize = true,
                Location = new System.Drawing.Point(50, 510),
                Width = 200,
            };

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            this.Controls.Add(listView1);
            this.Controls.Add(btnSelectFiles);
            this.Controls.Add(LbChooseDirHint);

            selectedFiles = new List<string>();
        }

        private ListView listView1;         // Первая колонка для отображения дисков, папок или файлов
        private bool isDriveList;           // Флаг: отображается список дисков
        private bool isFileList;            // Флаг: отображается список выбранных файлов
        private string currentPath;         // Текущий путь
        private List<string> selectedFiles; // Список выбранных файлов
        private Button btnSelectFiles;
        private Label LbChooseDirHint;
        #endregion
    }
}