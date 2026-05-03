using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MESC.Managers;
using MESC.Models;

namespace MESC.Forms
{
    public class LoginForm : Form
    {
        private readonly UserManager _userManager;
        private readonly TextBox _tbUser = new TextBox();
        private readonly TextBox _tbPass = new TextBox();
        public LoginForm()
        {
            Text = "MESC - Login"; Size = new Size(440, 320); StartPosition = FormStartPosition.CenterScreen;
            var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json");
            _userManager = new UserManager(p);
            Controls.AddRange(new Control[] {
                new Label{Text="Логин",Location=new Point(40,40)}, _tbUser,
                new Label{Text="Пароль",Location=new Point(40,95)}, _tbPass,
                new Button{Text="Войти",Location=new Point(40,150),Width=160,BackColor=Color.FromArgb(37,99,235),ForeColor=Color.White},
                new Button{Text="Регистрация",Location=new Point(210,150),Width=160}
            });
            _tbUser.SetBounds(40,60,330,25); _tbPass.SetBounds(40,115,330,25); _tbPass.PasswordChar='*';
            ((Button)Controls[4]).Click += LoginClick; ((Button)Controls[5]).Click += (s,e)=> new RegisterForm().ShowDialog();
        }
        private void LoginClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_tbUser.Text) || string.IsNullOrWhiteSpace(_tbPass.Text)) { MessageBox.Show("Заполните поля"); return; }
            var user = _userManager.Login(_tbUser.Text.Trim(), _tbPass.Text);
            if (user == null) { MessageBox.Show("Неверные данные"); return; }
            Hide(); new MainForm(user).ShowDialog(); Show();
        }
    }
}
