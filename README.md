🌟 Smart Inventory & Sales Management System

A modern, clean, and efficient C# Windows Forms + ADO.NET + SQL Server desktop application designed for managing inventory, sales, stock levels, and users.
Made to be simple, fast, and professional.

<div align="center">
🛒 Inventory Management • 💰 Sales Tracking • 📊 Dashboard Analytics
</div>
🎯 Overview

The Smart Inventory System is a complete desktop application that allows businesses to track:

Products

Categories

Sales

Users

Stock Levels

This project follows a clean and structured architecture, making it easy to upgrade, maintain, and extend.

✨ Key Features
🧩 Product & Category Management

Add / Edit / Delete products

Manage categories

Product price management

Real-time stock updates

📉 Stock Monitoring

Automatic low-stock alerts

Dashboard quick reminders

Instant value calculations

💵 Sales System

Create sales

Calculate totals automatically

Store sale data in SQL Server

🔐 Authentication System

Login form

Role-based navigation (Admin / Staff)

🖥️ Modern UI (Code-Designed)

Custom Windows Forms UI using C#

Dark theme (professional design)

Dashboard with buttons & statistics

🏗️ Project Architecture
SmartInventorySystem/
│
├── Domain/                 # Business logic + Models + Interfaces
│   ├── Entities
│   ├── Interfaces
│   └── Services
│
├── Infrastructure/         # ADO.NET Data Access Layer
│   ├── Database
│   └── Repositories
│
├── UI/                     # Windows Forms (Dashboard, Products, Login...)
│   ├── Forms
│   └── Components
│
├── Program.cs
└── app.config


Why this structure?
So the project behaves like a real industry project (clean architecture).

🗄️ Database Structure

This project uses SQL Server LocalDB.
Run this SQL script to create your database:

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

🔧 Configuration

Update the connection string in app.config:

<connectionStrings>
  <add name="db"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmartInventory;Integrated Security=True" />
</connectionStrings>

▶️ How to Run the Project

Clone the repository

Open the solution in Rider or Visual Studio

Create the SQL database (run the script)

Update the connection string

Run the project

You're ready to go 🚀

📸 Screenshots (Add Later)

You can add images here:

/screenshots/dashboard.png
/screenshots/products.png
/screenshots/login.png

🚀 Future Improvements

Export sales as PDF

Barcode generation

Enhanced reporting (charts)

User roles & permissions

Supplier management

👨‍💻 Author

Mouadh Radhoini
Smart Inventory System – 2025
