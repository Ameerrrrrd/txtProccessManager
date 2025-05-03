using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp222
{
    public partial class Form1 : Form
    {
        private bool isSingleFileModeAllowed = false;
        public static string selectedFilePath;
        public static string filePathForDiv;

        public static long currentUserId = -1;
        public static bool isMethodCompleted;

        public Form1()
        {
            InitializeComponent();
            CreateSortCheckboxes();
            CreateProcessCheckboxes();
            CreateWorkWithFilesCheckboxes();
            CreateWorkWith1FileCheckboxes();
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
            textBox1.Text = "Компьютер";
            isDriveList = true;
            isFileList = false;
            listView2.Enabled = false;
            btnTemplates.Enabled = false;
            btnExecute.Enabled = false;
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

                textBox1.Text = currentPath;
                listView2.Enabled = false;
                btnTemplates.Enabled = false;
                btnExecute.Enabled = false;
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
                    item.Tag = file;
                    item.Selected = true;
                }
            }
            textBox1.Text = "Выбранные файлы";
            isFileList = true;
            isDriveList = false;
            listView2.Enabled = false;
            btnTemplates.Enabled = false;
            btnExecute.Enabled = false;
        }


        // Обработка клика по первой колонке
        private void ListView1_Click(object sender, EventArgs e)
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
        private void ListView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        // Обработка перетаскивания файлов
        private void ListView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ShowSelectedFiles(files);
        }

        
        
        
        
        // Обработка выбора пункта меню во второй колонке
        private void ListView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                string selectedMenu = listView2.SelectedItems[0].Text;
                if (selectedMenu == "Сортировки")
                {
                    sortPanel.Visible = true;
                    processPanel.Visible = false;
                    workWithFilesPanel.Visible = false;
                    workWith1FilePanel.Visible = false;
                    currentMenu = "Сортировки";
                }
                else if (selectedMenu == "Обработки строк")
                {
                    sortPanel.Visible = false;
                    workWithFilesPanel.Visible = false;
                    processPanel.Visible = true;
                    workWith1FilePanel.Visible = false;
                    currentMenu = "Обработки строк";
                }
                else if (selectedMenu == "Работа с другими файлами")
                {
                    sortPanel.Visible = false;
                    processPanel.Visible = false;
                    workWithFilesPanel.Visible = true;
                    workWith1FilePanel.Visible = false;
                    currentMenu = "Работа с другими файлами";
                }
                else if (selectedMenu == "Работа с одним файлом")
                {
                    if (!isSingleFileModeAllowed)
                    {
                        MessageBox.Show("Для работы с меню необходимо выбрать ровно один файл.");
                        workWith1FilePanel.Visible = false;
                        sortPanel.Visible = false;
                        processPanel.Visible = false;
                        workWithFilesPanel.Visible = false;
                        currentMenu = "Работа с одним файлом";
                    }
                    else
                    {
                        sortPanel.Visible = false;
                        processPanel.Visible = false;
                        workWithFilesPanel.Visible = false;
                        workWith1FilePanel.Visible = true;
                        currentMenu = "Работа с одним файлом";
                    }
                }

            }
        }

        // Обработка Ctrl+A и Enter на выбор тхтшников
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                 Thread.Sleep(10); // Работа с потоком
;                Application.Exit();
            }

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
                            selectedFilePath = fullPath;
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

                isSingleFileModeAllowed = selectedFiles.Count == 1;

                string fileList = "Выбранные файлы:\n" + (selectedFiles.Count > 0 ? string.Join("\n", selectedFiles) : "Ничего не выбрано.");
                MessageBox.Show(fileList);

                // Активируем вторую колонку, если есть выбранные файлы
                listView2.Enabled = selectedFiles.Count > 0;
                btnTemplates.Enabled = selectedFiles.Count > 0;
                btnExecute.Enabled = selectedFiles.Count > 0;

                // Сбрасываем галочки в чекбоксах
                foreach (CheckBox cb in sortPanel.Controls)
                {
                    cb.Checked = false;
                }
                foreach (CheckBox cb in processPanel.Controls)
                {
                    cb.Checked = false;
                }
                foreach (CheckBox cb in workWithFilesPanel.Controls)
                {
                    cb.Checked = false;
                }
                sortOptions.Clear();
                processOptions.Clear();
                workWithFilesOptions.Clear();
                workWith1FileOptions.Clear();
            }
        }

        // Обработка кнопки "Открыть проводник"
        private void BtnOpenExplorer_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ShowSelectedFiles(openFileDialog.FileNames);
                }
            }
        }

        // Обработка кнопки "Выбрать" для файлов
        private void BtnSelectFiles_Click(object sender, EventArgs e)
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
                        selectedFilePath = fullPath;
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

            isSingleFileModeAllowed = selectedFiles.Count == 1;

            string fileList = "Выбранные файлы:\n" + (selectedFiles.Count > 0 ? string.Join("\n", selectedFiles) : "Ничего не выбрано.");
            MessageBox.Show(fileList);

            listView2.Enabled = selectedFiles.Count > 0;
            btnTemplates.Enabled = selectedFiles.Count > 0;
            btnExecute.Enabled = selectedFiles.Count > 0;

            foreach (CheckBox cb in sortPanel.Controls)
            {
                cb.Checked = false;
            }
            foreach (CheckBox cb in processPanel.Controls)
            {
                cb.Checked = false;
            }
            foreach (CheckBox cb in workWithFilesPanel.Controls)
            {
                cb.Checked = false;
            }
            foreach (CheckBox cb in workWith1FilePanel.Controls)
            {
                cb.Checked = false;
            }
            sortOptions.Clear();
            processOptions.Clear();
            workWithFilesOptions.Clear();
            workWith1FileOptions.Clear();
        }




        // Обработка кнопки "Сохранить"
        private void BtnExecute_Click(object sender, EventArgs e)
        {
            if (sortOptions.Count == 0 && processOptions.Count == 0 && workWithFilesOptions.Count == 0 && workWith1FileOptions.Count == 0)
                MessageBox.Show("Никаких опций не выбрано");
            else
            {
                using (var fs = new FormSavingName())
                {
                    var res = fs.ShowDialog();

                    if (FormSavingName.res)
                    {
                        using (var folderFialog = new FolderBrowserDialog())
                        {
                            folderFialog.Description = "Выберите директорию для сохранения файла:";

                            if (folderFialog.ShowDialog() == DialogResult.OK)
                            {
                                string saveDirectory = folderFialog.SelectedPath;

                                try
                                {
                                    if (selectedFiles.Count > 1)
                                    {
                                        if (workWithFilesOptions.Contains("Разность текстовиков") || processOptions.Contains("Разделение текстовика на части") || workWithFilesOptions.Contains("Объединение по имени файла"))
                                        {
                                            IEnumerable<string> mergedLines = Enumerable.Empty<string>();
                                            foreach (var file in selectedFiles)
                                            {
                                                var lines = File.ReadLines(file);
                                                mergedLines = mergedLines.Concat(lines);
                                            }

                                            mergedLines = ReturnWorkWithFilesResult(mergedLines);
                                            mergedLines = ReturnProcessingResult(mergedLines);
                                            mergedLines = ReturnSortResult(mergedLines);

                                            string sortedfilename = Path.Combine(saveDirectory, FormSavingName.savingName + ".txt");
                                            File.WriteAllLines(sortedfilename, mergedLines);
                                        }

                                        else
                                        {
                                            var result = MessageBox.Show("Объединить файлы?", "Подтверждение", MessageBoxButtons.YesNo);
                                            if (result == DialogResult.Yes)
                                            {
                                                IEnumerable<string> mergedLines = Enumerable.Empty<string>();
                                                foreach (var file in selectedFiles)
                                                {
                                                    var lines = File.ReadLines(file);
                                                    mergedLines = mergedLines.Concat(lines);
                                                }

                                                mergedLines = ReturnWorkWithFilesResult(mergedLines);
                                                mergedLines = ReturnProcessingResult(mergedLines);
                                                mergedLines = ReturnSortResult(mergedLines);

                                                string sortedfilename = Path.Combine(saveDirectory, FormSavingName.savingName + ".txt");
                                                File.WriteAllLines(sortedfilename, mergedLines);
                                            }
                                            else if (result == DialogResult.No)
                                            {
                                                int index = 0;

                                                foreach (var file in selectedFiles)
                                                {
                                                    var lines = File.ReadLines(file);

                                                    var processed = ReturnWorkWithFilesResult(lines);
                                                    processed = ReturnProcessingResult(processed);
                                                    processed = ReturnSortResult(processed);

                                                    string filenameSuffix = index == 0 ? "" : $"_{index}";
                                                    string filename = $"{FormSavingName.savingName}{filenameSuffix}.txt";
                                                    string fullPath = Path.Combine(saveDirectory, filename);

                                                    File.WriteAllLines(fullPath, processed);
                                                    index++;
                                                }
                                            }
                                        }
                                    }
                                    else if (selectedFiles.Count == 1)
                                    {
                                        IEnumerable<string> lines = File.ReadLines(selectedFiles[0]);

                                        if (workWith1FileOptions.Contains("Разделение текстовика на части"))
                                            WorkWith1File.SplitAndSaveFiles(lines, saveDirectory);
                                        else if (workWith1FileOptions.Contains("Изменение регистра почты"))
                                        {
                                            lines = ReturnWorkWith1FileResult(lines);

                                            string sortedfilename = Path.Combine(saveDirectory, FormSavingName.savingName + ".txt");
                                            File.WriteAllLines(sortedfilename, lines);
                                        }
                                        else
                                        {
                                            lines = ReturnWorkWithFilesResult(lines);
                                            lines = ReturnProcessingResult(lines);
                                            lines = ReturnSortResult(lines);

                                            string sortedfilename = Path.Combine(saveDirectory, FormSavingName.savingName + ".txt");
                                            File.WriteAllLines(sortedfilename, lines);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Нет выбранных файлов.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
        }

        // Прописанная логика опций сортировок
        private IEnumerable<string> ReturnSortResult(IEnumerable<string> lines)
        {

            if (sortOptions.Contains("По домену A-Z") && sortOptions.Contains("По значению Total (100-0)"))
            {
                lines = Sorting.CombineSortDomainTotalAndNumber(lines);
            }
            else
            {
                if (sortOptions.Contains("По домену A-Z")) lines = Sorting.SortByDomain(lines);
                if (sortOptions.Contains("По значению Total (100-0)")) lines = Sorting.SortByNumber(lines);
            }
            if (sortOptions.Contains("Выполнить обратную сортировку")) lines = Sorting.ReverseSorting(lines);

            return lines;
        }

        // Прописанная логика опций обработки строк
        private IEnumerable<string> ReturnProcessingResult(IEnumerable<string> lines)
        {
            if (processOptions.Contains("Удалить повторы")) lines = Processing.RemoveDuplicates(lines);
            if (processOptions.Contains("Удаление строк по КС")) lines = Processing.RemoveByKeyword(lines);
            if (processOptions.Contains("Удаление строк без КС")) lines = Processing.KeepOnlyByKeyword(lines);
            if (processOptions.Contains("Удаление содержимого до КС")) lines = Processing.RemoveBeforeKeyword(lines);
            if (processOptions.Contains("Удаление содержимого после КС")) lines = Processing.RemoveAfterKeyword(lines);

            return lines;
        }

        // Прописанная логика опций работы с файлами
        private IEnumerable<string> ReturnWorkWithFilesResult(IEnumerable<string> lines)
        {
            if (workWithFilesOptions.Contains("Разность текстовиков")) lines = WorkWithFiles.DifferenceTxt(lines);
            if (workWith1FileOptions.Contains("Объединение по имени файла")) lines = WorkWithFiles.FilterFileLinesByKeyword(selectedFiles);
            
            return lines;
        }

        // Прописанная логика опций работы с файлом
        private IEnumerable<string> ReturnWorkWith1FileResult(IEnumerable<string> lines)
        {
            return WorkWith1File.GenerateCaseMutations(lines);
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
                foreach (CheckBox cba in workWith1FilePanel.Controls)
                {
                        cba.Checked = false;
                }
                workWith1FileOptions.Clear();

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
            string[] processOptions = { "Удалить повторы", "Удаление строк по КС", "Удаление строк без КС", "Удаление содержимого до КС", "Удаление содержимого после КС"};
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
                foreach (CheckBox cba in workWith1FilePanel.Controls)
                {
                    cba.Checked = false;
                }
                workWith1FileOptions.Clear();

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
                foreach (CheckBox cba in workWith1FilePanel.Controls)
                {
                    cba.Checked = false;
                }
                workWith1FileOptions.Clear();

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
                else if (cb.Text == "Объединение по имени файла")
                {
                    if (selectedFiles.Count < 2)
                    {
                        MessageBox.Show("Вы выбрали недостаточное количество файлов для объединения");
                        cb.Checked = false;
                        workWithFilesOptions.Remove(cb.Text);
                    }
                    else
                    {
                        FileNameCombine form = new FileNameCombine();
                        form.ShowDialog();
                        if (string.IsNullOrWhiteSpace(FileNameCombine.combineByFileName))
                        {
                            cb.Checked = false;
                            workWithFilesOptions.Remove(cb.Text);
                        }
                    }
                }
            }
            else
            {
                workWithFilesOptions.Remove(cb.Text);
            }
        }

        // Создание чекбоксов работы с одним файлом
        private void CreateWorkWith1FileCheckboxes()
        {
            string[] workWith1FileOptions = { "Разделение текстовика на части"};
            for (int i = 0; i < workWith1FileOptions.Length; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Text = workWith1FileOptions[i],
                    Location = new Point(10, 20 * i),
                    AutoSize = true
                };
                cb.CheckedChanged += WorkWith1FileCheckBox_CheckedChanged;
                workWith1FilePanel.Controls.Add(cb);
            }
        }

        // Обработчик чекбоксов работы с одним файлом
        private void WorkWith1FileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Checked)
            {
                foreach (CheckBox cba in sortPanel.Controls)
                {
                    cba.Checked = false;
                }
                foreach (CheckBox cba in processPanel.Controls)
                {
                    cba.Checked = false;
                }
                foreach (CheckBox cba in workWithFilesPanel.Controls)
                {
                    cba.Checked = false;
                }
                foreach (CheckBox cba in workWith1FilePanel.Controls)
                {
                    if (cba != cb) cba.Checked = false;
                }

                sortOptions.Clear();
                processOptions.Clear();
                workWithFilesOptions.Clear();

                workWith1FileOptions.Clear();
                workWith1FileOptions.Add(cb.Text);

                if (!workWith1FileOptions.Contains(cb.Text))
                    workWith1FileOptions.Add(cb.Text);

                if (cb.Text == "Разделение текстовика на части")
                {
                    filePathForDiv = selectedFiles[0];
                    DivIntoPartsForm form = new DivIntoPartsForm();
                    form.ShowDialog();
                }
            }
            else
            {
                workWith1FileOptions.Remove(cb.Text);
            }
        }




        //// Выбор директории для сохранения файлов
        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedPath = folderDialog.SelectedPath;
                }
            }
        }

        // Обработка кнопки "Шаблоны"
        private void BtnTemplates_Click(object sender, EventArgs e)
        {
            FormTemplates form = new FormTemplates();
            DialogResult res = form.ShowDialog();

            if (res == DialogResult.OK)
            {
                var templateName = form.SelectedTemplateName;
                var template = InitDatabase_templates.GetTemplateByName(templateName);

                if (template != null)
                {
                    sortOptions = template["SortOptions"]
                        .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    processOptions = template["ProcessOptions"]
                        .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    workWithFilesOptions = template["WorkWithFilesOptions"]
                        .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    // другие формы:
                    FormSortPriority.SortKeyword = template["KeywordSorting"];
                    FormKeyWordDelLines.DelLinesKeyword = template["KeywordDelLines"].Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    FormWithoutKeyWordDelLines.DelLinesWithoutKeyword = template["KeywordWithoutDelLines"].Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    FormKeyWordDelLineContentAfter.DelLineContentAfterKeyword = template["KeywordAfterDelContent"];
                    FormKeyWordDelLineContentBefore.DelLineContentBeforeKeyword = template["KeywordBeforeDelContent"];
                    TxtDifference.CombineTxtDifference = template["txtDifference"];

                    ApplyOptionsToPanel(sortPanel, sortOptions);
                    ApplyOptionsToPanel(processPanel, processOptions);
                    ApplyOptionsToPanel(workWithFilesPanel, workWithFilesOptions);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    e.Cancel = true;
                else
                    Application.Exit();
            }
        }

        //метод для проставления чекбоксов после парса БД
        private void ApplyOptionsToPanel(Panel panel, List<string> options)
        {
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is CheckBox checkBox)
                {
                    checkBox.Checked = options.Contains(checkBox.Text);
                }
            }
        }

        //меню логаут, хелп
        private void MenuButton_Click(object sender, EventArgs e)
        {
            contextMenu.Show(menuButton, 0, menuButton.Height);
        }
    }
}