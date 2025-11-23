using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;
using SmartInventorySystem.Domain.Services;

namespace SmartInventorySystem.UI
{
    public partial class SalesForm : Form
    {
        private readonly IProductRepository _productRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly SalesService _salesService;

        private ComboBox cmbProducts;
        private Label lblPrice;
        private Label lblStock;
        private TextBox txtQuantity;
        private Label lblTotal;
        private Button btnSell;

        private TextBox txtSearch;
        private ComboBox cmbDateFilter;
        private Button btnReset;

        private DataGridView gridSales;

        public SalesForm(
            IProductRepository productRepository,
            ISaleRepository saleRepository,
            SalesService salesService)
        {
            _productRepository = productRepository;
            _saleRepository = saleRepository;
            _salesService = salesService;

            InitializeComponent();
            BuildUI();
            Load += SalesForm_Load;
        }

        private void BuildUI()
        {
            Text = "Sales Management";
            Size = new Size(900, 550);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            Label lblTitle = new Label
            {
                Text = "REGISTER SALE",
                Font = new Font("Segoe UI Semibold", 16),
                AutoSize = true,
                Location = new Point(320, 15)
            };

            // PRODUCT
            Label lblProduct = new Label
            {
                Text = "Product",
                Location = new Point(30, 70)
            };

            cmbProducts = new ComboBox
            {
                Location = new Point(150, 65),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbProducts.SelectedIndexChanged += (s, e) => UpdateProductInfo();

            // PRICE
            Label lblPriceTitle = new Label
            {
                Text = "Price",
                Location = new Point(30, 110)
            };

            lblPrice = new Label
            {
                Text = "-",
                Location = new Point(150, 110),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // STOCK
            Label lblStockTitle = new Label
            {
                Text = "In Stock",
                Location = new Point(30, 140)
            };

            lblStock = new Label
            {
                Text = "-",
                Location = new Point(150, 140),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // QUANTITY
            Label lblQty = new Label
            {
                Text = "Quantity",
                Location = new Point(30, 180)
            };

            txtQuantity = new TextBox
            {
                Location = new Point(150, 175),
                Width = 80
            };
            txtQuantity.TextChanged += (s, e) => UpdateTotal();

            // TOTAL
            lblTotal = new Label
            {
                Text = "Total: 0.00",
                Location = new Point(150, 210),
                Font = new Font("Segoe UI Semibold", 12),
                ForeColor = Color.DarkGreen
            };

            // SELL BUTTON
            btnSell = new Button
            {
                Text = "Confirm Sale",
                Location = new Point(150, 250),
                Width = 200,
                Height = 40,
                BackColor = Color.FromArgb(46, 134, 222),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSell.FlatAppearance.BorderSize = 0;
            btnSell.Click += BtnSell_Click;

            // SALES GRID
            gridSales = new DataGridView
            {
                Location = new Point(400, 70),
                Width = 470,
                Height = 400,
                ReadOnly = true,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // SEARCH BAR
            Label lblSearch = new Label
            {
                Text = "Search",
                Location = new Point(400, 30)
            };

            txtSearch = new TextBox
            {
                Location = new Point(460, 25),
                Width = 200
            };
            txtSearch.TextChanged += (s, e) => ApplyFilters();

            // DATE FILTER
            cmbDateFilter = new ComboBox
            {
                Location = new Point(680, 25),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDateFilter.Items.AddRange(new string[] { "All", "Today", "This Week", "This Month" });
            cmbDateFilter.SelectedIndex = 0;
            cmbDateFilter.SelectedIndexChanged += (s, e) => ApplyFilters();

            // RESET
            btnReset = new Button
            {
                Text = "Reset",
                Location = new Point(840, 25),
                Width = 60
            };
            btnReset.Click += async (s, e) =>
            {
                txtSearch.Clear();
                cmbDateFilter.SelectedIndex = 0;
                await LoadSalesHistory();
            };

            Controls.Add(lblTitle);
            Controls.Add(lblProduct);
            Controls.Add(cmbProducts);
            Controls.Add(lblPriceTitle);
            Controls.Add(lblPrice);
            Controls.Add(lblStockTitle);
            Controls.Add(lblStock);
            Controls.Add(lblQty);
            Controls.Add(txtQuantity);
            Controls.Add(lblTotal);
            Controls.Add(btnSell);
            Controls.Add(gridSales);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(cmbDateFilter);
            Controls.Add(btnReset);
        }

        private async void SalesForm_Load(object? sender, EventArgs e)
        {
            await LoadProducts();
            await LoadSalesHistory();
        }

        // LOAD PRODUCTS
        private async Task LoadProducts()
        {
            var products = await _productRepository.GetAllAsync();
            cmbProducts.DataSource = products;
            cmbProducts.DisplayMember = "Name";
            cmbProducts.ValueMember = "Id";

            UpdateProductInfo();
        }

        // LOAD SALES
        private async Task LoadSalesHistory()
        {
            var sales = await _saleRepository.GetAllAsync();
            gridSales.DataSource = sales;
        }

        // UPDATE PRICE / STOCK
        private void UpdateProductInfo()
        {
            if (cmbProducts.SelectedItem is not Product product)
                return;

            lblPrice.Text = product.Price.ToString("0.00");
            lblStock.Text = product.Quantity.ToString();
            UpdateTotal();
        }

        // TOTAL PRICE CALC
        private void UpdateTotal()
        {
            if (cmbProducts.SelectedItem is not Product product)
            {
                lblTotal.Text = "Total: 0.00";
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                lblTotal.Text = "Total: 0.00";
                return;
            }

            decimal total = qty * product.Price;
            lblTotal.Text = $"Total: {total:0.00}";
        }

        // CONFIRM SALE
        private async void BtnSell_Click(object? sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem is not Product product)
            {
                MessageBox.Show("Select a product.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Invalid quantity.");
                return;
            }

            if (qty > product.Quantity)
            {
                MessageBox.Show("Not enough stock.");
                return;
            }

            try
            {
                await _salesService.MakeSaleAsync(product.Id, qty);

                MessageBox.Show("Sale recorded successfully.");

                await LoadProducts();
                await LoadSalesHistory();

                txtQuantity.Clear();
                lblTotal.Text = "Total: 0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error making sale: " + ex.Message);
            }
        }

        // APPLY SEARCH + DATE FILTER
        private async void ApplyFilters()
        {
            var sales = await _saleRepository.GetAllAsync();

            // SEARCH
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string s = txtSearch.Text.ToLower();
                sales = sales
                    .Where(x =>
                        x.Product.Name.ToLower().Contains(s) ||
                        x.Quantity.ToString().Contains(s) ||
                        x.TotalPrice.ToString().Contains(s))
                    .ToList();
            }

            // DATE FILTER
            string filter = cmbDateFilter.SelectedItem?.ToString() ?? "All";

            if (filter == "Today")
            {
                sales = sales.Where(s => s.Date.Date == DateTime.Today).ToList();
            }
            else if (filter == "This Week")
            {
                DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                sales = sales.Where(s => s.Date >= startOfWeek).ToList();
            }
            else if (filter == "This Month")
            {
                sales = sales.Where(s => s.Date.Month == DateTime.Today.Month).ToList();
            }

            gridSales.DataSource = sales;
        }
    }
}
