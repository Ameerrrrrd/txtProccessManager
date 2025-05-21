using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace KP_KAZLOVSKIY
{

    public partial class DivIntoPartsForm : Form
    {
        private TextBox txtLinesCount;
        private TextBox txtFilesCount;
        private Button btnOK;
        private int txtLinesCountInitially;

        public static string txtLinesCountFinal { get; private set; } = string.Empty;        
        public static string txtFilesCountFinal { get; private set; } = string.Empty;

        private int totalLines = 0;
        private bool isUpdating = false;

        public static string path = Form1.filePathForDiv; // путь к файлу, который будет установлен извне
        public DivIntoPartsForm()
        {
            InitializeComponent();
            CalculateFileString();
            this.Load += DivIntoPartsForm_Load;




        }
        private void DivIntoPartsForm_Load(object sender, EventArgs e)
        {
            InitializeFileStats();
        }
        private void InitializeFileStats()
        {
            if (File.Exists(path))
            {
                totalLines = File.ReadAllLines(path).Length;

                isUpdating = true;
                txtLinesCount.Text = totalLines.ToString();
                txtFilesCount.Text = "1";
                isUpdating = false;
            }
            else
            {
                MessageBox.Show("Файл не найден: " + path);
            }
        }

        private void CalculateFileString()
        {

            if (File.Exists(path))
            {
                int lineCount = File.ReadAllLines(path).Length;
                txtLinesCount.Text = lineCount.ToString();
                txtLinesCountInitially = int.Parse(txtLinesCount.Text);
                txtFilesCount.Text = "1";
            }
            else
            {
                MessageBox.Show("Файл не найден: " + path);
            }
}

        private void txtLinesCount_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;

            string input = txtLinesCount.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            try
            {
                int linesPerFile = int.Parse(input);
                if (linesPerFile <= 0) return;

                int fileCount = (int)Math.Ceiling((double)totalLines / linesPerFile);

                isUpdating = true;
                txtFilesCount.Text = fileCount.ToString();
                isUpdating = false;
            }
            catch
            {
                MessageBox.Show("Введите корректное количество строк на файл.");
            }
        }

        private void txtFilesCount_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;

            string input = txtFilesCount.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            try
            {
                int fileCount = int.Parse(input);
                if (fileCount <= 0) return;

                int linesPerFile = (int)Math.Ceiling((double)totalLines / fileCount);

                isUpdating = true;
                txtLinesCount.Text = linesPerFile.ToString();
                isUpdating = false;
            }
            catch
            {
                MessageBox.Show("Введите корректное количество файлов.");
            }
        }




        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "Разделение файла на части";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(310, 250);
            this.Icon = Properties.Resources.cat;

            // Метка "Разделить файл на части" (по центру)
            Label lblTitle = new Label
            {
                Text = "Разделить файл на части",
                Font = new Font("Arial", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 20) // Центрируем вручную для формы 400x250
            };

            // Левая часть: "количество строк в текстовике"
            Label lblLines = new Label
            {
                Text = "Количество строк",
                Location = new Point(10, 80),
                AutoSize = true
            };
            txtLinesCount = new TextBox
            {
                Location = new Point(20, 110),
                Width = 100
            };

            // Правая часть: "количество файлов"
            Label lblFiles = new Label
            {
                Text = "Количество файлов",
                Location = new Point(150, 80), // На той же высоте, что и левая метка
                AutoSize = true
            };
            txtFilesCount = new TextBox
            {
                Location = new Point(170, 110),
                Width = 100
            };

            // Кнопка "ОК"
            btnOK = new Button
            {
                Text = "ОК",
                Location = new Point(90, 160),
                Width = 100
            };
            btnOK.Click += btnOK_Click;

            // Добавляем элементы на форму
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblLines);
            this.Controls.Add(txtLinesCount);
            this.Controls.Add(lblFiles);
            this.Controls.Add(txtFilesCount);
            this.Controls.Add(btnOK);

            txtLinesCount.TextChanged += txtLinesCount_TextChanged;
            txtFilesCount.TextChanged += txtFilesCount_TextChanged;
            this.Load += new System.EventHandler(this.DivIntoPartsForm_Load);
        }

        // Обработчик нажатия кнопки "ОК"
        private void btnOK_Click(object sender, EventArgs e)
        {
            string linesText = txtLinesCount.Text.Trim();
            string filesText = txtFilesCount.Text.Trim();

            if (string.IsNullOrEmpty(linesText) || string.IsNullOrEmpty(filesText))
            {
                MessageBox.Show("Оба поля должны быть заполнены корректными значениями.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            txtFilesCountFinal = txtFilesCount.Text;
            txtLinesCountFinal = txtLinesCount.Text;
            this.Close();
        }

        // Обработка клавиш "Enter" и "Escape"
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnOK.PerformClick(); // Симулируем нажатие кнопки "ОК"
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                this.Close(); // Закрываем форму
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}