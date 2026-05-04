using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MESC.Controls;
using MESC.Managers;

namespace MESC.Forms
{
    public class LoginForm : Form
    {
        private readonly UserManager _userManager;
        private readonly TextBox _tbUser = new TextBox();
        private readonly TextBox _tbPass = new TextBox();

        public LoginForm()
        {
            Text = "MESC - Вход";
            Size = new Size(480, 360);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(248, 250, 252);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json");
            _userManager = new UserManager(p);

            var card = new RoundedPanel { Left = 45, Top = 35, Width = 370, Height = 260, BackColor = Color.White, Radius = 18 };
            card.Controls.Add(new Label { Text = "MESC", Left = 24, Top = 20, Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), AutoSize = true });
            card.Controls.Add(new Label { Text = "Войдите в аккаунт", Left = 24, Top = 58, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, AutoSize = true });

            _tbUser.SetBounds(24, 90, 320, 34);
            _tbPass.SetBounds(24, 132, 320, 34);
            _tbPass.PasswordChar = '*';
            _tbUser.Font = _tbPass.Font = new Font("Segoe UI", 10);

            var loginBtn = new RoundedButton { Text = "Войти", Left = 24, Top = 182, Width = 150, Height = 40 };
            var registerBtn = new RoundedButton { Text = "Регистрация", Left = 194, Top = 182, Width = 150, Height = 40, BaseColor = Color.FromArgb(30, 41, 59), HoverColor = Color.FromArgb(51, 65, 85) };

            loginBtn.Click += LoginClick;
            registerBtn.Click += (s, e) => new RegisterForm().ShowDialog();

            card.Controls.AddRange(new Control[] { _tbUser, _tbPass, loginBtn, registerBtn });
            Controls.Add(card);
        }

        private void LoginClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_tbUser.Text) || string.IsNullOrWhiteSpace(_tbPass.Text))
            {
                MessageBox.Show("Заполните поля");
                return;
            }

            var user = _userManager.Login(_tbUser.Text.Trim(), _tbPass.Text);
            if (user == null) { MessageBox.Show("Неверные данные"); return; }
            Hide(); new MainForm(user).ShowDialog(); Show();
        }
    }
}
