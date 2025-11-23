using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartInventorySystem.Domain.Services;
using SmartInventorySystem.Domain.Entities;

namespace SmartInventorySystem.UI
{
    public partial class LowStockForm : Form
    {
        private readonly StockAlertService _stockAlertService;

        private DataGridView gridLowStock;
        private Button btnRefresh;

        public LowStockForm(StockAlertService stockAlertService)
        {
            _stockAlertService = stockAlertService;

            InitializeComponent();
            BuildUI();
            Load += LowStockForm_Load;
        }

        private void BuildUI()
        {
            Text = "Low Stock Alerts";
            Size = new Size(700, 420);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            // TITLE
            Label lblTitle = new Label
            {
                Text = "Low Stock Products",
                Font = new Font("Segoe UI Semibold", 14),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // DATA GRID
            gridLowStock = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 640,
                Height = 270,
                ReadOnly = true,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            gridLowStock.CellFormatting += GridLowStock_CellFormatting;

            // REFRESH BUTTON
            btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 120,
                Height = 32,
                Location = new Point(20, 340),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += async (s, e) => await LoadLowStock();

            Controls.Add(lblTitle);
            Controls.Add(gridLowStock);
            Controls.Add(btnRefresh);
        }

        private async void LowStockForm_Load(object? sender, EventArgs e)
        {
            await LoadLowStock();

            var user = Program.LoggedInUser;

            // Cashier can only view, not modify anything
            if (user != null && user.Role == "Cashier")
            {
                btnRefresh.Enabled = true; 
            }
        }

        private async Task LoadLowStock()
        {
            var lowStockItems = await _stockAlertService.GetLowStockProductsAsync();

            var displayList = lowStockItems.Select(p => new
            {
                p.Id,
                p.Name,
                Category = p.Category != null ? p.Category.Name : "Unknown",
                p.Quantity,
                p.MinStock,
                Status = p.Quantity <= p.MinStock ? "LOW" : "OK"
            }).ToList();

            gridLowStock.DataSource = displayList;
        }

        private void GridLowStock_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridLowStock.Columns[e.ColumnIndex].Name == "Status")
            {
                string? status = e.Value?.ToString();

                if (status == "LOW")
                {
                    gridLowStock.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        Color.FromArgb(254, 226, 226);

                    gridLowStock.Rows[e.RowIndex].DefaultCellStyle.ForeColor =
                        Color.FromArgb(220, 38, 38);
                }
            }
        }
    }
}
