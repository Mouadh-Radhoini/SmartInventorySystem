using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;
using SmartInventorySystem.Domain.Services;

namespace SmartInventorySystem.UI
{
    public partial class ProductsForm : Form
    {
        private readonly InventoryService _inventoryService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        private TextBox txtName;
        private TextBox txtPrice;
        private TextBox txtQuantity;
        private TextBox txtMinStock;
        private ComboBox cmbCategory;

        private TextBox txtSearch;
        private ComboBox cmbFilterCategory;
        private Button btnResetFilter;

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;

        private DataGridView gridProducts;

        private int selectedProductId = 0;
        private List<Product> _allProducts = new();

        public ProductsForm(
            InventoryService inventoryService,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _inventoryService = inventoryService;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;

            InitializeComponent();
            BuildUI();
            Load += ProductsForm_Load;
        }

        private void BuildUI()
        {
            Text = "Products Management";
            Size = new Size(880, 540);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            // --- LEFT SIDE FORM ---

            Label lblName = new Label() { Text = "Name", Location = new Point(20, 20) };
            txtName = new TextBox() { Location = new Point(120, 20), Width = 220 };

            Label lblPrice = new Label() { Text = "Price", Location = new Point(20, 60) };
            txtPrice = new TextBox() { Location = new Point(120, 60), Width = 220 };

            Label lblQuantity = new Label() { Text = "Quantity", Location = new Point(20, 100) };
            txtQuantity = new TextBox() { Location = new Point(120, 100), Width = 220 };

            Label lblMinStock = new Label() { Text = "Min Stock", Location = new Point(20, 140) };
            txtMinStock = new TextBox() { Location = new Point(120, 140), Width = 220 };

            Label lblCategory = new Label() { Text = "Category", Location = new Point(20, 180) };
            cmbCategory = new ComboBox()
            {
                Location = new Point(120, 180),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnAdd = new Button() { Text = "Add", Location = new Point(20, 230), Width = 90 };
            btnUpdate = new Button() { Text = "Update", Location = new Point(120, 230), Width = 90 };
            btnDelete = new Button() { Text = "Delete", Location = new Point(220, 230), Width = 90 };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            // --- SEARCH + FILTER ---

            Label lblSearch = new Label { Text = "Search", Location = new Point(20, 290) };
            txtSearch = new TextBox { Location = new Point(120, 285), Width = 220 };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            Label lblFilterCategory = new Label { Text = "Filter by Category", Location = new Point(20, 330) };
            cmbFilterCategory = new ComboBox
            {
                Location = new Point(150, 325),
                Width = 190,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilterCategory.SelectedIndexChanged += CmbFilterCategory_SelectedIndexChanged;

            btnResetFilter = new Button
            {
                Text = "Reset",
                Location = new Point(120, 365),
                Width = 220
            };
            btnResetFilter.Click += BtnResetFilter_Click;

            // --- GRID ---

            gridProducts = new DataGridView
            {
                Location = new Point(370, 20),
                Width = 480,
                Height = 430,
                ReadOnly = true,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            gridProducts.CellClick += gridProducts_CellClick;

            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblPrice);
            Controls.Add(txtPrice);
            Controls.Add(lblQuantity);
            Controls.Add(txtQuantity);
            Controls.Add(lblMinStock);
            Controls.Add(txtMinStock);
            Controls.Add(lblCategory);
            Controls.Add(cmbCategory);

            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);

            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(lblFilterCategory);
            Controls.Add(cmbFilterCategory);
            Controls.Add(btnResetFilter);

            Controls.Add(gridProducts);
        }

        private async void ProductsForm_Load(object? sender, EventArgs e)
        {
            await LoadCategories();
            await LoadProducts();

            // Role-based: Cashier can only view
            var user = Program.LoggedInUser;
            if (user != null && user.Role == "Cashier")
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private async Task LoadCategories()
        {
            var categories = (await _categoryRepository.GetAllAsync()).ToList();

            // For product form
            cmbCategory.DataSource = categories.ToList();
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";

            // For filter dropdown
            var filterList = new List<Category>
            {
                new Category { Id = 0, Name = "All" }
            };
            filterList.AddRange(categories);

            cmbFilterCategory.DataSource = filterList;
            cmbFilterCategory.DisplayMember = "Name";
            cmbFilterCategory.ValueMember = "Id";
        }

        private async Task LoadProducts()
        {
            _allProducts = await _productRepository.GetAllAsync();
            gridProducts.DataSource = _allProducts
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Quantity,
                    p.MinStock,
                    Category = p.Category != null ? p.Category.Name : "Unknown",
                    p.CategoryId
                })
                .ToList();
        }

        private async Task ApplyFilters()
        {
            if (_allProducts == null || _allProducts.Count == 0)
                _allProducts = await _productRepository.GetAllAsync();

            IEnumerable<Product> filtered = _allProducts;

            // Search by name
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var search = txtSearch.Text.ToLower();
                filtered = filtered.Where(p => p.Name != null &&
                                               p.Name.ToLower().Contains(search));
            }

            // Filter by category
            if (cmbFilterCategory.SelectedItem is Category selectedCategory && selectedCategory.Id != 0)
            {
                filtered = filtered.Where(p => p.CategoryId == selectedCategory.Id);
            }

            gridProducts.DataSource = filtered
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Quantity,
                    p.MinStock,
                    Category = p.Category != null ? p.Category.Name : "Unknown",
                    p.CategoryId
                })
                .ToList();
        }

        // ADD
        private async void btnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtMinStock.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out var price) ||
                !int.TryParse(txtQuantity.Text, out var quantity) ||
                !int.TryParse(txtMinStock.Text, out var minStock))
            {
                MessageBox.Show("Invalid number values.");
                return;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Select a category.");
                return;
            }

            var product = new Product
            {
                Name = txtName.Text.Trim(),
                Price = price,
                Quantity = quantity,
                MinStock = minStock,
                CategoryId = (int)cmbCategory.SelectedValue
            };

            await _inventoryService.AddProductAsync(product);
            await LoadProducts();
            await ApplyFilters();

            ClearInputs();
            MessageBox.Show("Product added.");
        }

        // UPDATE
        private async void btnUpdate_Click(object? sender, EventArgs e)
        {
            if (selectedProductId == 0)
            {
                MessageBox.Show("Select a product first.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out var price) ||
                !int.TryParse(txtQuantity.Text, out var quantity) ||
                !int.TryParse(txtMinStock.Text, out var minStock))
            {
                MessageBox.Show("Invalid values.");
                return;
            }

            var product = await _productRepository.GetByIdAsync(selectedProductId);
            if (product == null)
            {
                MessageBox.Show("Product not found.");
                return;
            }

            product.Name = txtName.Text.Trim();
            product.Price = price;
            product.Quantity = quantity;
            product.MinStock = minStock;
            product.CategoryId = (int)cmbCategory.SelectedValue;

            await _inventoryService.UpdateProductAsync(product);
            await LoadProducts();
            await ApplyFilters();

            ClearInputs();
            MessageBox.Show("Product updated.");
        }

        // DELETE
        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedProductId == 0)
            {
                MessageBox.Show("Select a product.");
                return;
            }

            await _inventoryService.DeleteProductAsync(selectedProductId);
            selectedProductId = 0;

            await LoadProducts();
            await ApplyFilters();

            ClearInputs();
            MessageBox.Show("Product deleted.");
        }

        private void gridProducts_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (gridProducts.SelectedRows.Count > 0)
            {
                var row = gridProducts.SelectedRows[0];

                selectedProductId = (int)row.Cells["Id"].Value;
                txtName.Text = row.Cells["Name"].Value?.ToString();
                txtPrice.Text = row.Cells["Price"].Value?.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value?.ToString();
                txtMinStock.Text = row.Cells["MinStock"].Value?.ToString();

                if (row.Cells["CategoryId"].Value != null)
                {
                    cmbCategory.SelectedValue = (int)row.Cells["CategoryId"].Value;
                }
            }
        }

        private async void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            await ApplyFilters();
        }

        private async void CmbFilterCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            await ApplyFilters();
        }

        private async void BtnResetFilter_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cmbFilterCategory.Items.Count > 0)
            {
                cmbFilterCategory.SelectedIndex = 0; // All
            }

            await LoadProducts();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtMinStock.Clear();
            selectedProductId = 0;
        }
    }
}
