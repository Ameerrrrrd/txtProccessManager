using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp222
{
    partial class FormTemplates
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
            this.Text = "Шаблоны";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            createTemplateButton = new Button
            {
                Text = "Создать шаблон",
                Location = new Point(30, 20),
                Size = new Size(200, 40),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Black,
                BackColor = Color.LightGray
            };
            createTemplateButton.FlatAppearance.BorderColor = Color.Green;
            createTemplateButton.FlatAppearance.BorderSize = 1;

            addTemplateButton = new Button
            {
                Text = "Создать шаблон", // Изменено на "Создать шаблон"
                Location = new Point(240, 20),
                Size = new Size(200, 40),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Black,
                BackColor = Color.LightGray
            };
            addTemplateButton.FlatAppearance.BorderColor = Color.Green;
            addTemplateButton.FlatAppearance.BorderSize = 1;
            addTemplateButton.Click += AddTestTemplateButton_Click; // Обработчик для открытия формы

            myTemplatesLabel = new Label
            {
                Text = "Мои шаблоны:",
                Location = new Point(30, 80),
                AutoSize = true,
                ForeColor = Color.Black
            };

            myTemplatesPanel = new FlowLayoutPanel
            {
                Location = new Point(30, 110),
                Size = new Size(200, 300),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.White
            };

            singleFileTemplatesLabel = new Label
            {
                Text = "Шаблоны одного файла:",
                Location = new Point(240, 80),
                AutoSize = true,
                ForeColor = Color.Black
            };

            for (int i = 0; i < 2; i++)
            {
                singleFileButtons[i] = new Button
                {
                    Text = "Шаблон " + (i + 1),
                    Location = new Point(240, 110 + i * 50),
                    Size = new Size(200, 40),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.LightGray,
                    ForeColor = Color.Black
                };
                singleFileButtons[i].FlatAppearance.BorderColor = Color.Green;
                singleFileButtons[i].FlatAppearance.BorderSize = 1;
            }

            multipleFilesTemplatesLabel = new Label
            {
                Text = "Шаблоны нескольких файлов:",
                Location = new Point(450, 80),
                AutoSize = true,
                ForeColor = Color.Black
            };

            for (int i = 0; i < 4; i++)
            {
                multipleFileButtons[i] = new Button
                {
                    Text = "Шаблон " + (i + 1),
                    Location = new Point(450, 110 + i * 50),
                    Size = new Size(200, 40),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.LightGray,
                    ForeColor = Color.Black
                };
                multipleFileButtons[i].FlatAppearance.BorderColor = Color.Green;
                multipleFileButtons[i].FlatAppearance.BorderSize = 1;
            }

            this.Controls.Add(createTemplateButton);
            this.Controls.Add(addTemplateButton);
            this.Controls.Add(myTemplatesLabel);
            this.Controls.Add(myTemplatesPanel);
            this.Controls.Add(singleFileTemplatesLabel);
            foreach (var btn in singleFileButtons)
            {
                this.Controls.Add(btn);
            }
            this.Controls.Add(multipleFilesTemplatesLabel);
            foreach (var btn in multipleFileButtons)
            {
                this.Controls.Add(btn);
            }
        }

        // Updated class member declarations
        private Button createTemplateButton;
        private Button addTemplateButton; // Кнопка "Создать шаблон" для открытия формы
        private Label myTemplatesLabel;
        private FlowLayoutPanel myTemplatesPanel;
        private Label singleFileTemplatesLabel;
        private Button[] singleFileButtons = new Button[2];
        private Label multipleFilesTemplatesLabel;
        private Button[] multipleFileButtons = new Button[4];
        private string connectionString = "Data Source=templates.db;Version=3;";

    }
    #endregion
}