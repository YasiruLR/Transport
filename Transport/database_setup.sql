-- Transport Management System Database Setup
-- Execute this script in MySQL to create the database and tables

-- Create Database
CREATE DATABASE IF NOT EXISTS TransportDB;
USE TransportDB;

-- Create Admin table
CREATE TABLE IF NOT EXISTS Admin (
    AdminID INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    Role VARCHAR(20) NOT NULL DEFAULT 'Admin'
);

-- Create Customer table
CREATE TABLE IF NOT EXISTS Customer (
    CustomerID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Phone VARCHAR(20) NOT NULL,
    Address TEXT NOT NULL,
    RegisteredDate DATE NOT NULL,
    Password VARCHAR(100) NOT NULL
);

-- Create Job table
CREATE TABLE IF NOT EXISTS Job (
    JobID INT AUTO_INCREMENT PRIMARY KEY,
    CustomerID INT NOT NULL,
    StartLocation VARCHAR(200) NOT NULL,
    Destination VARCHAR(200) NOT NULL,
    Status VARCHAR(50) DEFAULT 'Pending',
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) ON DELETE CASCADE
);

-- Create Product table
CREATE TABLE IF NOT EXISTS Product (
    ProductID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    Price DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0
);

-- Create LoadDetails table
CREATE TABLE IF NOT EXISTS LoadDetails (
    LoadID INT AUTO_INCREMENT PRIMARY KEY,
    JobID INT NOT NULL,
    Lorry VARCHAR(50),
    Driver VARCHAR(100),
    Assistant VARCHAR(100),
    Container VARCHAR(50),
    FOREIGN KEY (JobID) REFERENCES Job(JobID) ON DELETE CASCADE
);

-- Insert sample admin user
INSERT INTO Admin (Username, Password, Role) VALUES 
('admin', 'admin123', 'Admin'),
('manager', 'manager123', 'Manager');

-- Insert sample customer (use your email)
INSERT INTO Customer (Name, Email, Phone, Address, RegisteredDate, Password) VALUES 
('John Doe', 'john@example.com', '1234567890', '123 Main Street, City', '2024-01-01', 'password123'),
('Jane Smith', 'jane@example.com', '0987654321', '456 Oak Avenue, Town', '2024-01-02', 'password456');

-- Insert sample products
INSERT INTO Product (Name, Description, Price, Stock) VALUES 
('Box - Small', 'Small cardboard box for light items', 5.00, 100),
('Box - Medium', 'Medium cardboard box for general use', 8.00, 75),
('Box - Large', 'Large cardboard box for heavy items', 12.00, 50),
('Bubble Wrap', 'Protective bubble wrap material', 15.00, 25);

-- Insert sample jobs
INSERT INTO Job (CustomerID, StartLocation, Destination, Status) VALUES 
(1, 'New York', 'Los Angeles', 'Pending'),
(1, 'Chicago', 'Miami', 'In Progress'),
(2, 'Boston', 'Seattle', 'Completed');

-- Insert sample load details
INSERT INTO LoadDetails (JobID, Lorry, Driver, Assistant, Container) VALUES 
(1, 'TRK-001', 'Mike Johnson', 'Tom Wilson', 'CNT-001'),
(2, 'TRK-002', 'Sarah Davis', 'Bob Miller', 'CNT-002'),
(3, 'TRK-003', 'David Brown', 'Lisa Garcia', 'CNT-003');

-- Display all tables
SELECT 'Admin Table:' AS Info;
SELECT * FROM Admin;

SELECT 'Customer Table:' AS Info;
SELECT * FROM Customer;

SELECT 'Job Table:' AS Info;
SELECT * FROM Job;

SELECT 'Product Table:' AS Info;
SELECT * FROM Product;

SELECT 'LoadDetails Table:' AS Info;
SELECT * FROM LoadDetails;