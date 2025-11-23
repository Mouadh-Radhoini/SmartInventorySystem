📄 README.md at the root of your project.

Here is a clean, professional README for a C# Smart Inventory System:

📦 Smart Inventory & Sales Management System

A modern and professional C# Windows Forms + ADO.NET + SQL Server desktop application for managing products, categories, stock, sales, and users.
Built for performance, simplicity, and scalability.

🚀 Features
🛒 Inventory Management

Add, update, delete products

Manage categories

Track quantities & prices

Automatic low-stock detection

💰 Sales Module

Create sales

Record transactions

View daily/monthly revenue reports

👥 User Management

Authentication (login/logout)

Role-based navigation (admin vs staff)

📊 Dashboard

Total products, low stock alerts

Total sales & statistics

Quick navigation buttons

🏗️ Architecture (Clean Architecture Style)
SmartInventorySystem/
│
├── Domain/               → Core business logic (Entities, Interfaces)
├── Infrastructure/       → Database access using ADO.NET + SQL Server
├── UI/                   → Windows Forms (WinForms)
│
├── Program.cs
├── app.config
└── SmartInventorySystem.sln

🗄️ Database

This project uses SQL Server LocalDB.

Run this SQL script to create the DB:
CREATE DATABASE SmartInventory;
GO

USE SmartInventory;

CREATE TABLE Categories (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(150),
    CategoryId INT,
    Quantity INT,
    Price DECIMAL(10,2),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE Sales (
    Id INT IDENTITY PRIMARY KEY,
    ProductId INT,
    Quantity INT,
    Total DECIMAL(10,2),
    DateSale DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE Users (
    Id INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(100),
    Password NVARCHAR(100),
    Role NVARCHAR(50)
);

▶️ Running the Project

Open the solution in Rider or Visual Studio

Ensure LocalDB is installed

Run the SQL script

Update the connection string in app.config:

<connectionStrings>
  <add name="db"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmartInventory;Integrated Security=True"/>
</connectionStrings>


Start the app — you're ready to use it 🎉

📝 Author

Mouadh Radhoini
Smart Inventory System • 2025
