using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KP_KAZLOVSKIY
{
    public partial class TxtDifference : Form
    {
        public static string CombineTxtDifference{ get; set; } = string.Empty;
        public TxtDifference()
        {
            InitializeComponent();
            LoadDrives();
        }



        // Подгрузка дисков
        private void LoadDrives()
        {
            listView1.Items.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    listView1.Items.Add(drive.Name);
                }
            }
            isDriveList = true;
            isFileList = false;
        }

        // Отображение содержимого каталога
        private void ShowDirectoryContents(string path)
        {
            try
            {
                listView1.Items.Clear();
                currentPath = path;
                isDriveList = false;
                isFileList = false;

                listView1.Items.Add("..");

                string[] directories = Directory.GetDirectories(path);
                foreach (string dir in directories)
                {
                    listView1.Items.Add(new DirectoryInfo(dir).Name);
                }

                string[] files = Directory.GetFiles(path, "*.txt");
                foreach (string file in files)
                {
                    listView1.Items.Add(new FileInfo(file).Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Отображение выбранных файлов
        private void ShowSelectedFiles(string[] files)
        {
            listView1.Items.Clear();
            listView1.Items.Add("...");
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower() == ".txt")
                {
                    ListViewItem item = listView1.Items.Add(Path.GetFileName(file));
                    item.Tag = file; // Сохраняем полный путь
                    item.Selected = true; // Автоматически выделяем
                }
            }
            isFileList = true;
            isDriveList = false;
        }


        // Обработка клика по первой колонке
        private void listView1_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                string selectedItem = listView1.SelectedItems[0].Text;

                if (selectedItem == "..")
                {
                    if (!isDriveList && !isFileList)
                    {
                        string parentPath = Directory.GetParent(currentPath)?.FullName;
                        if (parentPath != null)
                        {
                            ShowDirectoryContents(parentPath);
                        }
                        else
                        {
                            LoadDrives();
                        }
                    }
                }
                else if (selectedItem == "...")
                {
                    if (isFileList)
                    {
                        LoadDrives();
                    }
                }
                else if (isDriveList)
                {
                    ShowDirectoryContents(selectedItem);
                }
                else if (!isFileList)
                {
                    string fullPath = Path.Combine(currentPath, selectedItem);
                    if (Directory.Exists(fullPath))
                    {
                        ShowDirectoryContents(fullPath);
                    }
                }
            }
        }

        // Поддержка DragEnter для перетаскивания
        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        // Обработка перетаскивания файлов
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ShowSelectedFiles(files);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Обработка Ctrl+A
            if (e.Control && e.KeyCode == Keys.A && !isDriveList && !isFileList)
            {
                try
                {
                    listView1.SelectedItems.Clear();
                    string[] files = Directory.GetFiles(currentPath, "*.txt");

                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        ListViewItem item = listView1.FindItemWithText(fileName);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }

                    listView1.Focus();
                    e.Handled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            // Обработка Enter
            else if (e.KeyCode == Keys.Enter && listView1.Focused && listView1.SelectedItems.Count > 0)
            {
                try
                {
                    selectedFiles.Clear();

                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        string fileName = item.Text;
                        if (fileName != ".." && fileName != "...")
                        {
                            string fullPath;
                            if (isFileList)
                            {
                                fullPath = item.Tag.ToString();
                            }
                            else
                            {
                                fullPath = Path.Combine(currentPath, fileName);
                            }
                            if (File.Exists(fullPath) && Path.GetExtension(fullPath).ToLower() == ".txt")
                            {
                                selectedFiles.Add(fullPath);
                            }
                        }
                    }

                    string fileList = "Выбранные файлы:\n" + (selectedFiles.Count > 0 ? string.Join("\n", selectedFiles) : "Ничего не выбрано.");
                    MessageBox.Show(fileList);
                    e.Handled = true;
                    SaveAndClose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btnSelectFiles_Click(object sender, EventArgs e)
        {
            selectedFiles.Clear();

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                string fileName = item.Text;
                if (fileName != ".." && fileName != "...")
                {
                    string fullPath;
                    if (isFileList)
                    {
                        fullPath = item.Tag.ToString();
                    }
                    else
                    {
                        fullPath = Path.Combine(currentPath, fileName);
                    }
                    if (File.Exists(fullPath) && Path.GetExtension(fullPath).ToLower() == ".txt")
                    {
                        selectedFiles.Add(fullPath);
                    }
                }
            }

            string fileList = "Выбранные файлы:\n" + (selectedFiles.Count > 0 ? string.Join("\n", selectedFiles) : "Ничего не выбрано.");
            MessageBox.Show(fileList);
            SaveAndClose();
        }

        private void SaveAndClose()
        {
            if (selectedFiles.Count > 0)
            {
                StringBuilder combinedContent = new StringBuilder();
                foreach (string file in selectedFiles)
                {
                    string[] lines = File.ReadAllLines(file); // Читаем все строки из файла
                    foreach (string line in lines)
                    {
                        combinedContent.AppendLine(line); // Добавляем каждую строку с переносом
                    }
                }
                CombineTxtDifference = combinedContent.ToString(); // Сохраняем результат
            }
            else
            {
                CombineTxtDifference = string.Empty; // Если файлов нет, сбрасываем переменную
            }


            this.Close();
        }
    }
}
