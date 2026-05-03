using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MESC.Managers;
using MESC.Models;

namespace MESC.Forms
{
    public class MainForm : Form
    {
        private readonly MaterialManager _materialManager;
        private readonly UploadManager _uploadManager = new UploadManager();
        private readonly User _currentUser;
        private readonly ComboBox _grade = new ComboBox();
        private readonly TextBox _search = new TextBox();
        private readonly ListBox _list = new ListBox();
        public MainForm(User user)
        {
            _currentUser = user; Text = "MESC Dashboard"; WindowState = FormWindowState.Maximized;
            _materialManager = new MaterialManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "materials.json"));
            BuildUi(); LoadMaterials();
        }
        private void BuildUi()
        {
            var sidebar = new Panel{Dock=DockStyle.Left,Width=220,BackColor=Color.FromArgb(30,41,59)};
            var menu = new[]{"Главная","Предметы","Материалы","Google Drive","Загрузка файлов","Помощь","О программе"};
            int y=80; foreach(var m in menu){ var b=new Button{Text=m,Left=20,Top=y,Width=180,Height=40,FlatStyle=FlatStyle.Flat,ForeColor=Color.White,BackColor=Color.FromArgb(30,41,59)}; b.FlatAppearance.BorderSize=0; sidebar.Controls.Add(b); y+=48; if(m=="Google Drive") b.Click+=(s,e)=>OpenUrl("https://drive.google.com/"); if(m=="Загрузка файлов") b.Click+=UploadClick; }
            var top = new Panel{Dock=DockStyle.Top,Height=70,BackColor=Color.White};
            _grade.Items.AddRange(new object[]{"10","11","12"}); _grade.SelectedIndex=0; _grade.SetBounds(20,20,120,30); _grade.SelectedIndexChanged+=(s,e)=>LoadMaterials();
            _search.SetBounds(160,20,260,30); _search.TextChanged += (s,e)=>LoadMaterials();
            var theme = new Button{Text="Light/Dark",Left=440,Top=20,Width=120}; theme.Click+=(s,e)=>ThemeManager.Toggle(this);
            top.Controls.AddRange(new Control[]{_grade,_search,theme,new Label{Text=$"Добро пожаловать, {_currentUser.Username}",Left=580,Top=25,AutoSize=true}});
            _list.Dock = DockStyle.Fill;
            _list.DoubleClick += (s,e)=>OpenSelectedMaterial();
            Controls.AddRange(new Control[]{_list,top,sidebar});
        }
        private void LoadMaterials()
        {
            var grade = int.Parse(_grade.SelectedItem.ToString());
            IEnumerable<MaterialItem> data = string.IsNullOrWhiteSpace(_search.Text) ? _materialManager.GetByGrade(grade) : _materialManager.Search(_search.Text, grade);
            _list.DataSource = data.Select(m=>$"{m.Subject}: {m.Title}").ToList();
        }
        private void OpenSelectedMaterial(){ if(_list.SelectedItem!=null) MessageBox.Show(_list.SelectedItem.ToString(),"Материал"); }
        private void UploadClick(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "Files|*.pdf;*.docx;*.txt;*.pptx" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show(_uploadManager.AddFile(ofd.FileName) ? "Файл добавлен" : "Ошибка добавления файла");
                }
            }
        }
        private void OpenUrl(string url)
        {
            try { Process.Start(url); }
            catch (Exception ex) { MessageBox.Show($"Ошибка открытия: {ex.Message}"); }
        }
    }
}
