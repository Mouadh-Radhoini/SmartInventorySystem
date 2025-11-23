using System;
using System.Drawing;
using System.Threading.Tasks;
using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;

namespace SmartInventorySystem.UI
{
    public partial class CategoriesForm : Form
    {
        private readonly ICategoryRepository _categoryRepository;

        private DataGridView gridCategories;
        private TextBox txtName;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;

        private int selectedCategoryId = 0;

        public CategoriesForm(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            InitializeComponent();
            BuildUI();
            Load += CategoriesForm_Load;
        }

        private void BuildUI()
        {
            Text = "Category Management";
            Size = new Size(600, 400);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            // Title
            Label lblTitle = new Label
            {
                Text = "Manage Categories",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // Category Name Input
            Label lblName = new Label
            {
                Text = "Category Name",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 80)
            };

            txtName = new TextBox
            {
                Location = new Point(150, 75),
                Width = 200,
                Font = new Font("Segoe UI", 10)
            };

            // ADD button
            btnAdd = new Button
            {
                Text = "Add",
                Width = 100,
                Height = 35,
                Location = new Point(20, 130),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            // UPDATE button
            btnUpdate = new Button
            {
                Text = "Update",
                Width = 100,
                Height = 35,
                Location = new Point(130, 130),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += BtnUpdate_Click;

            // DELETE button
            btnDelete = new Button
            {
                Text = "Delete",
                Width = 100,
                Height = 35,
                Location = new Point(240, 130),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            // DataGrid
            gridCategories = new DataGridView
            {
                Location = new Point(380, 20),
                Width = 200,
                Height = 320,
                ReadOnly = true,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            gridCategories.CellClick += GridCategories_CellClick;

            Controls.Add(lblTitle);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(gridCategories);
        }

        private async void CategoriesForm_Load(object? sender, EventArgs e)
        {
            // ROLE-BASED PERMISSIONS
            var user = Program.LoggedInUser;

            if (user != null && user.Role == "Cashier")
            {
                // Cashier is READ-ONLY
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                txtName.ReadOnly = true;
            }

            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            gridCategories.DataSource = categories;
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Category name is required.");
                return;
            }

            var category = new Category { Name = txtName.Text };

            await _categoryRepository.AddAsync(category);
            await LoadCategories();

            txtName.Clear();
            MessageBox.Show("Category added.");
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (selectedCategoryId == 0)
            {
                MessageBox.Show("Select a category first.");
                return;
            }

            var category = await _categoryRepository.GetByIdAsync(selectedCategoryId);
            if (category == null) return;

            category.Name = txtName.Text;

            await _categoryRepository.UpdateAsync(category);
            await LoadCategories();

            txtName.Clear();
            MessageBox.Show("Category updated.");
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedCategoryId == 0)
            {
                MessageBox.Show("Select a category to delete.");
                return;
            }

            await _categoryRepository.DeleteAsync(selectedCategoryId);
            await LoadCategories();

            txtName.Clear();
            MessageBox.Show("Category deleted.");
        }

        private void GridCategories_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (gridCategories.SelectedRows.Count > 0)
            {
                var row = gridCategories.SelectedRows[0];

                selectedCategoryId = (int)row.Cells["Id"].Value;
                txtName.Text = row.Cells["Name"].Value?.ToString();
            }
        }
    }
}
