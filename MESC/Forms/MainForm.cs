using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MESC.Controls;
using MESC.Managers;
using MESC.Models;

namespace MESC.Forms
{
    public class MainForm : Form
    {
        private readonly MaterialManager _materialManager;
        private readonly UploadManager _uploadManager = new UploadManager();
        private readonly UploadStorageManager _uploadStorageManager;
        private readonly DriveLinkManager _driveLinkManager;
        private readonly User _currentUser;
        private readonly ComboBox _grade = new ComboBox();
        private readonly TextBox _search = new TextBox();
        private readonly Panel _content = new Panel();

        public MainForm(User user)
        {
            _currentUser = user;
            Text = "MESC Dashboard";
            WindowState = FormWindowState.Maximized;
            _materialManager = new MaterialManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "materials.json"));
            _driveLinkManager = new DriveLinkManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "drive_links.json"));
            _uploadStorageManager = new UploadStorageManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "uploads.json"));
            BuildUi();
            ShowHome();
        }

        private void BuildUi()
        {
            var sidebar = new Panel { Dock = DockStyle.Left, Width = 220, BackColor = Color.FromArgb(30, 41, 59) };
            var menu = new[] { "Главная", "Предметы", "Материалы", "Google Drive", "Загрузка файлов", "Помощь", "О программе" };
            int y = 80;
            foreach (var m in menu)
            {
                var b = new RoundedButton { Text = m, Left = 20, Top = y, Width = 180, Height = 40, BaseColor = Color.FromArgb(30, 41, 59), HoverColor = Color.FromArgb(51, 65, 85) };
                b.Click += (s, e) => Navigate(m);
                sidebar.Controls.Add(b);
                y += 48;
            }

            var top = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.White };
            _grade.Items.AddRange(new object[] { "10", "11", "12" });
            _grade.SelectedIndex = 0;
            _grade.SetBounds(20, 20, 120, 30);
            _search.SetBounds(160, 20, 260, 30);
            var theme = new RoundedButton { Text = "Light/Dark", Left = 440, Top = 16, Width = 130, Height = 38, BaseColor = Color.FromArgb(30, 41, 59), HoverColor = Color.FromArgb(51, 65, 85) };
            theme.Click += (s, e) => ThemeManager.Toggle(this);
            top.Controls.AddRange(new Control[] { _grade, _search, theme, new Label { Text = $"Добро пожаловать, {_currentUser.Username}", Left = 580, Top = 25, AutoSize = true } });

            _content.Dock = DockStyle.Fill;
            _content.AutoScroll = true;

            Controls.AddRange(new Control[] { _content, top, sidebar });
        }

        private void Navigate(string section)
        {
            switch (section)
            {
                case "Главная": ShowHome(); break;
                case "Предметы": ShowSubjects(); break;
                case "Материалы": ShowMaterials(); break;
                case "Google Drive": ShowDriveLinks(); break;
                case "Загрузка файлов": ShowUploadPage(); break;
                case "Помощь": ShowHelp(); break;
                case "О программе": ShowAbout(); break;
            }
        }

        private void ShowHome()
        {
            _content.Controls.Clear();
            var popular = string.Join(", ", _materialManager.Recommend(int.Parse(_grade.SelectedItem.ToString())).Select(x => x.Subject));
            var hero = new RoundedPanel { Left = 20, Top = 20, Width = 900, Height = 140, BackColor = Color.FromArgb(37, 99, 235), Radius = 18 };
            hero.Controls.Add(new Label { Text = $"Добро пожаловать, {_currentUser.Username}!", Left = 24, Top = 24, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = true });
            hero.Controls.Add(new Label { Text = $"Популярные предметы: {popular}", Left = 24, Top = 66, Font = new Font("Segoe UI", 11), ForeColor = Color.WhiteSmoke, AutoSize = true });
            var quickMaterials = new RoundedButton { Text = "Открыть материалы", Left = 24, Top = 94, Width = 170, Height = 34, BaseColor = Color.White, HoverColor = Color.FromArgb(226, 232, 240), ForeColor = Color.FromArgb(30, 41, 59) };
            quickMaterials.Click += (s, e) => ShowMaterials();
            hero.Controls.Add(quickMaterials);
            _content.Controls.Add(hero);
        }

        private void ShowSubjects()
        {
            _content.Controls.Clear();
            var subjects = new[] { "Математика", "Физика", "Химия", "Биология", "История", "Информатика", "Английский язык", "География" };
            int y = 20;
            foreach (var s in subjects)
            {
                var btn = new Button { Text = $"Открыть: {s}", Left = 20, Top = y, Width = 300, Height = 36 };
                btn.Click += (a, b) => ShowMaterials(s);
                _content.Controls.Add(btn);
                y += 44;
            }
        }

        private void ShowMaterials(string subject = null)
        {
            _content.Controls.Clear();
            var grade = int.Parse(_grade.SelectedItem.ToString());
            var query = _materialManager.GetByGrade(grade);
            if (!string.IsNullOrWhiteSpace(subject)) query = query.Where(x => x.Subject == subject);
            if (!string.IsNullOrWhiteSpace(_search.Text)) query = query.Where(x => x.Title.ToLower().Contains(_search.Text.ToLower()) || x.Subject.ToLower().Contains(_search.Text.ToLower()));
            var list = new ListBox { Left = 20, Top = 20, Width = 900, Height = 450 };
            var data = query.ToList();
            list.DataSource = data;
            list.DisplayMember = "Title";
            list.DoubleClick += (s, e) =>
            {
                if (list.SelectedItem is MaterialItem m)
                {
                    OpenMaterial(m);
                }
            };
            _content.Controls.Add(list);
        }

        private void ShowDriveLinks()
        {
            _content.Controls.Clear();
            var links = _driveLinkManager.GetAll();
            int y = 20;
            foreach (var link in links)
            {
                var title = new Label { Text = link.Title, Left = 20, Top = y, Width = 600, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
                var desc = new Label { Text = link.Description, Left = 20, Top = y + 22, Width = 700 };
                var open = new Button { Text = "Открыть", Left = 740, Top = y + 10, Width = 120 };
                open.Click += (s, e) => OpenUrl(link.Url);
                _content.Controls.AddRange(new Control[] { title, desc, open });
                y += 64;
            }
        }

        private void ShowUploadPage()
        {
            _content.Controls.Clear();
            var uploadBtn = new Button { Text = "Загрузить файл", Left = 20, Top = 20, Width = 180, Height = 40 };
            var list = new ListView { Left = 20, Top = 80, Width = 900, Height = 430, View = View.Details, FullRowSelect = true };
            list.Columns.Add("Файл", 250);
            list.Columns.Add("Путь", 500);
            list.Columns.Add("Дата", 140);

            Action refresh = () =>
            {
                list.Items.Clear();
                foreach (var item in _uploadStorageManager.GetAll())
                {
                    list.Items.Add(new ListViewItem(new[] { item.FileName, item.FullPath, item.UploadedAt }));
                }
            };

            uploadBtn.Click += (s, e) =>
            {
                using (var ofd = new OpenFileDialog { Filter = "Files|*.pdf;*.docx;*.txt;*.pptx" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (_uploadManager.AddFile(ofd.FileName))
                        {
                            var selectedSubject = AskSubject();
                            if (string.IsNullOrWhiteSpace(selectedSubject)) return;
                            var title = AskText("Название материала", Path.GetFileNameWithoutExtension(ofd.FileName));
                            if (string.IsNullOrWhiteSpace(title)) return;
                            var grade = int.Parse(_grade.SelectedItem.ToString());

                            _uploadStorageManager.Add(new UploadItem { FileName = Path.GetFileName(ofd.FileName), FullPath = ofd.FileName, UploadedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm") });
                            _materialManager.AddMaterial(new MaterialItem
                            {
                                Title = title,
                                Subject = selectedSubject,
                                Grade = grade,
                                Theory = "Пользовательский загруженный материал",
                                Notes = "Файл: " + ofd.FileName,
                                Practice = "Откройте файл для практики",
                                Recommendations = "Добавлено через загрузку",
                                UsefulLinks = ofd.FileName
                            });
                            refresh();
                            MessageBox.Show($"Материал добавлен в предмет '{selectedSubject}' для {grade} класса.");
                        }
                        else MessageBox.Show("Ошибка добавления файла");
                    }
                }
            };

            _content.Controls.Add(uploadBtn);
            _content.Controls.Add(list);
            refresh();
        }

        private void ShowHelp()
        {
            _content.Controls.Clear();
            _content.Controls.Add(new Label { Left = 20, Top = 20, Width = 900, Height = 300, Text = "Помощь:\n1) Выберите класс сверху.\n2) Откройте Предметы или Материалы.\n3) Для Google Drive добавьте ссылки в Data/drive_links.json.\n4) Загруженные файлы отображаются в разделе 'Загрузка файлов'." });
        }

        private void ShowAbout()
        {
            _content.Controls.Clear();
            _content.Controls.Add(new Label { Left = 20, Top = 20, Width = 900, Height = 200, Text = "МЭСК — платформа для подготовки к экзаменам 10-12 классов.\nВерсия: 1.1" });
        }

        private void OpenUrl(string url)
        {
            try { Process.Start(url); }
            catch (Exception ex) { MessageBox.Show($"Ошибка открытия: {ex.Message}"); }
        }

        private void OpenMaterial(MaterialItem material)
        {
            var link = material.UsefulLinks ?? string.Empty;
            if (File.Exists(link))
            {
                Process.Start(link);
                return;
            }

            if (link.StartsWith("http://") || link.StartsWith("https://"))
            {
                OpenUrl(link);
                return;
            }

            MessageBox.Show($"{material.Subject}\nТеория: {material.Theory}\nКонспект: {material.Notes}\nПрактика: {material.Practice}\nРекомендации: {material.Recommendations}\nСсылка/Путь: {material.UsefulLinks}", material.Title);
        }

        private string AskSubject()
        {
            var subjects = new[] { "Математика", "Физика", "Химия", "Биология", "История", "Информатика", "Английский язык", "География" };
            using (var dialog = new Form { Text = "Выбор предмета", Width = 360, Height = 180, StartPosition = FormStartPosition.CenterParent })
            {
                var cb = new ComboBox { Left = 20, Top = 20, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
                cb.Items.AddRange(subjects);
                cb.SelectedIndex = 0;
                var ok = new Button { Text = "OK", Left = 20, Top = 70, Width = 120, DialogResult = DialogResult.OK };
                dialog.Controls.AddRange(new Control[] { cb, ok });
                dialog.AcceptButton = ok;
                return dialog.ShowDialog() == DialogResult.OK ? cb.SelectedItem.ToString() : null;
            }
        }

        private string AskText(string title, string defaultValue)
        {
            using (var dialog = new Form { Text = title, Width = 420, Height = 180, StartPosition = FormStartPosition.CenterParent })
            {
                var tb = new TextBox { Left = 20, Top = 20, Width = 360, Text = defaultValue };
                var ok = new Button { Text = "OK", Left = 20, Top = 70, Width = 120, DialogResult = DialogResult.OK };
                dialog.Controls.AddRange(new Control[] { tb, ok });
                dialog.AcceptButton = ok;
                return dialog.ShowDialog() == DialogResult.OK ? tb.Text.Trim() : null;
            }
        }
    }
}
