using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MESC.Controls;
using MESC.Managers;
using MESC.Models;

namespace MESC.Forms
{
    public class RegisterForm : Form
    {
        public RegisterForm()
        {
            Text = "MESC - Регистрация";
            Size = new Size(500, 430);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(248, 250, 252);

            var um = new UserManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json"));
            var card = new RoundedPanel { Left = 45, Top = 30, Width = 390, Height = 330, BackColor = Color.White, Radius = 18 };

            var u = new TextBox { Left = 30, Top = 70, Width = 330, Font = new Font("Segoe UI", 10) };
            var e = new TextBox { Left = 30, Top = 125, Width = 330, Font = new Font("Segoe UI", 10) };
            var p = new TextBox { Left = 30, Top = 180, Width = 330, PasswordChar = '*', Font = new Font("Segoe UI", 10) };
            var b = new RoundedButton { Text = "Создать аккаунт", Left = 30, Top = 240, Width = 330, Height = 42 };

            card.Controls.AddRange(new Control[]
            {
                new Label { Text = "Создание аккаунта", Left = 30, Top = 22, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true },
                new Label { Text = "Username", Left = 30, Top = 50, AutoSize = true }, u,
                new Label { Text = "Email", Left = 30, Top = 105, AutoSize = true }, e,
                new Label { Text = "Password", Left = 30, Top = 160, AutoSize = true }, p,
                b
            });

            b.Click += (s, a) =>
            {
                if (string.IsNullOrWhiteSpace(u.Text) || string.IsNullOrWhiteSpace(e.Text) || string.IsNullOrWhiteSpace(p.Text)) { MessageBox.Show("Заполните все поля"); return; }
                if (p.Text.Length < 6) { MessageBox.Show("Пароль минимум 6 символов"); return; }
                var ok = um.Register(new User { Username = u.Text.Trim(), Email = e.Text.Trim(), Password = p.Text });
                MessageBox.Show(ok ? "Пользователь создан" : "Пользователь уже существует");
                if (ok) Close();
            };

            Controls.Add(card);
        }
    }
}
