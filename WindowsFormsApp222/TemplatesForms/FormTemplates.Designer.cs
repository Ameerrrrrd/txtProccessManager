using System.Drawing;
using System.Windows.Forms;

namespace KP_KAZLOVSKIY
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
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Icon = Properties.Resources.cat;
            this.BackColor = Color.White;

            addTemplateButton = new Button
            {
                Text = "Создать шаблон", // Изменено на "Создать шаблон"
                Location = new Point(20, 20),
                Size = new Size(200, 40),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Black,
                BackColor = Color.LightGray
            };
            addTemplateButton.FlatAppearance.BorderColor = Color.Green;
            addTemplateButton.FlatAppearance.BorderSize = 1;
            addTemplateButton.Click += AddTestTemplateButton_Click; // Обработчик для открытия формы

            hintLabel = new Label
            {
                Text = "Только уникальные имена\nПКМ для удаления",
                Location = new Point(250, 20),
                Size = new Size(400,40),
                ForeColor = Color.Gray

            };
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

            this.Controls.Add(hintLabel);
            this.Controls.Add(createTemplateButton);
            this.Controls.Add(addTemplateButton);
            this.Controls.Add(myTemplatesLabel);
            this.Controls.Add(myTemplatesPanel);
            this.Controls.Add(singleFileTemplatesLabel);
            foreach (var btn in singleFileButtons)
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
        private Label hintLabel;

    }
    #endregion
}