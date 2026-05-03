using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MESC.Managers;
using MESC.Models;

namespace MESC.Forms
{
    public class RegisterForm : Form
    {
        public RegisterForm()
        {
            Text = "MESC - Register"; Size = new Size(440, 360); StartPosition = FormStartPosition.CenterParent;
            var um = new UserManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json"));
            var u = new TextBox{Left=40,Top=50,Width=330}; var e = new TextBox{Left=40,Top=105,Width=330}; var p = new TextBox{Left=40,Top=160,Width=330,PasswordChar='*'};
            var b = new Button{Text="Создать",Left=40,Top=210,Width=330,BackColor=Color.FromArgb(37,99,235),ForeColor=Color.White};
            Controls.AddRange(new Control[]{new Label{Text="Username",Left=40,Top=30},u,new Label{Text="Email",Left=40,Top=85},e,new Label{Text="Password",Left=40,Top=140},p,b});
            b.Click += (s,a)=> {
                if (string.IsNullOrWhiteSpace(u.Text)||string.IsNullOrWhiteSpace(e.Text)||string.IsNullOrWhiteSpace(p.Text)){MessageBox.Show("Заполните все поля");return;}
                if (p.Text.Length<6){MessageBox.Show("Пароль минимум 6 символов");return;}
                var ok=um.Register(new User{Username=u.Text.Trim(),Email=e.Text.Trim(),Password=p.Text});
                MessageBox.Show(ok?"Пользователь создан":"Пользователь уже существует"); if(ok) Close();
            };
        }
    }
}
