using System.Drawing;
using Microsoft.Extensions.DependencyInjection;

namespace SmartInventorySystem.UI
{
    public partial class DashboardForm : Form
    {
        private Button btnProducts;
        private Button btnCategories;
        private Button btnSales;
        private Button btnLowStock;
        private Button btnLogout;
        private Label lblUserInfo;

        public DashboardForm()
        {
            InitializeComponent();
            BuildUI();
            Load += DashboardForm_Load;
        }

        private void BuildUI()
        {
            Text = "Smart Inventory - Dashboard";
            Size = new Size(700, 450);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(15, 23, 42);

            // HEADER
            var lblTitle = new Label
            {
                Text = "SMART INVENTORY DASHBOARD",
                Font = new Font("Segoe UI Semibold", 16),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(140, 25)
            };

            lblUserInfo = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(20, 60)
            };

            // MAIN PANEL
            var panel = new Panel
            {
                Size = new Size(640, 260),
                Location = new Point(30, 90),
                BackColor = Color.FromArgb(30, 41, 59)
            };

            // BUTTONS
            btnProducts = CreateMenuButton("Products", 40, 40);
            btnProducts.Click += (s, e) =>
            {
                var form = Program.Services.GetRequiredService<ProductsForm>();
                form.Show();
            };

            btnCategories = CreateMenuButton("Categories", 340, 40);
            btnCategories.Click += (s, e) =>
            {
                var form = Program.Services.GetRequiredService<CategoriesForm>();
                form.Show();
            };

            btnSales = CreateMenuButton("Sales", 40, 130);
            btnSales.Click += (s, e) =>
            {
                var form = Program.Services.GetRequiredService<SalesForm>();
                form.Show();
            };

            btnLowStock = CreateMenuButton("Low Stock Alerts", 340, 130);
            btnLowStock.Click += (s, e) =>
            {
                var form = Program.Services.GetRequiredService<LowStockForm>();
                form.Show();
            };

            panel.Controls.AddRange(new Control[]
            {
                btnProducts, btnCategories, btnSales, btnLowStock
            });

            // LOGOUT
            btnLogout = new Button
            {
                Text = "Logout",
                Location = new Point(540, 360),
                Width = 120,
                Height = 32,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) =>
            {
                var login = Program.Services.GetRequiredService<LoginForm>();
                login.Show();
                this.Hide();
            };

            Controls.Add(lblTitle);
            Controls.Add(lblUserInfo);
            Controls.Add(panel);
            Controls.Add(btnLogout);
        }

        private Button CreateMenuButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Width = 240,
                Height = 60,
                Font = new Font("Segoe UI Semibold", 10),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void DashboardForm_Load(object? sender, EventArgs e)
        {
            var user = Program.LoggedInUser;

            if (user != null)
            {
                lblUserInfo.Text = $"Logged in as: {user.Username} ({user.Role})";
            }

            // ROLE-BASED ACCESS
            if (user?.Role == "Cashier")
            {
                btnProducts.Visible = false;
                btnCategories.Visible = false;
                btnLowStock.Visible = false;

                // Make Sales button bigger and centered
                btnSales.Width = 500;
                btnSales.Location = new Point(70, 90);
            }
        }
    }
}
