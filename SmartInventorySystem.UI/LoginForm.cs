using System.Drawing;
using SmartInventorySystem.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using SmartInventorySystem.Domain.Entities;

namespace SmartInventorySystem.UI
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

        public LoginForm(AuthService authService)
        {
            _authService = authService;

            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Smart Inventory - Login";
            Size = new Size(480, 360);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(15, 23, 42); // dark background

            // MAIN TITLE ON TOP
            var lblAppTitle = new Label
            {
                Text = "SMART INVENTORY SYSTEM",
                Font = new Font("Segoe UI Semibold", 14),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(60, 25)
            };

            // CARD PANEL
            var panel = new Panel
            {
                Size = new Size(360, 220),
                Location = new Point(60, 80),
                BackColor = Color.FromArgb(30, 41, 59) // slate
            };

            var lblLoginTitle = new Label
            {
                Text = "Login",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(25, 20)
            };

            var lblUsername = new Label
            {
                Text = "Username",
                ForeColor = Color.Gainsboro,
                Location = new Point(25, 65)
            };

            txtUsername = new TextBox
            {
                Location = new Point(25, 85),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };

            var lblPassword = new Label
            {
                Text = "Password",
                ForeColor = Color.Gainsboro,
                Location = new Point(25, 120)
            };

            txtPassword = new TextBox
            {
                Location = new Point(25, 140),
                Width = 300,
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };

            btnLogin = new Button
            {
                Text = "Sign in",
                Location = new Point(25, 175),
                Width = 300,
                Height = 32,
                Font = new Font("Segoe UI Semibold", 10),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            panel.Controls.Add(lblLoginTitle);
            panel.Controls.Add(lblUsername);
            panel.Controls.Add(txtUsername);
            panel.Controls.Add(lblPassword);
            panel.Controls.Add(txtPassword);
            panel.Controls.Add(btnLogin);

            Controls.Add(lblAppTitle);
            Controls.Add(panel);
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            var user = await _authService.LoginAsync(username, password);

            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                return;
            }

            Program.LoggedInUser = user;

            MessageBox.Show($"Welcome {user.Username} ({user.Role})");

            var dashboard = Program.Services.GetRequiredService<DashboardForm>();
            dashboard.Show();

            Hide();
        }
    }
}
