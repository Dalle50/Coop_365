CREATE TABLE dbo.Products(
ProductID INT NOT NULL PRIMARY KEY,
ProductName varchar(255) NOT NULL,
Stock INT,
Price float NOT NULL,
Description TEXT NOT NULL,
Url varchar(255),
)

CREATE TABLE dbo.Kunder(
KundeID int NOT NULL IDENTITY(100000,1) PRIMARY KEY,
Name varchar(255) NOT NULL,
Address varchar(255) NOT NULL,
Zipcode int NOT NULL,
City varchar(255) NOT NULL,
Email varchar(255) NOT NULL,
PhoneNumber int NOT NULL UNIQUE,
CoopPoints float NOT NULL,
)

CREATE TABLE dbo.Orders(
OrderID int NOT NULL  IDENTITY(40000,1) PRIMARY KEY,
TotalPrice float NOT NULL,
Date DateTime NOT NULL,
KundeID int NULL,
FOREIGN KEY (KundeID) REFERENCES dbo.Kunder(KundeID), 
)

CREATE TABLE dbo.OrderLines(
OrderLineID int NOT NULL IDENTITY PRIMARY KEY,
Amount int NOT NULL,
ProductID int FOREIGN KEY REFERENCES dbo.Products(ProductID),
OrderID int FOREIGN KEY REFERENCES dbo.Orders(OrderID),
Date date
)

--DROP TABLE dbo.OrderLines
--DROP TABLE dbo.Orders
--DROP TABLE dbo.Kunder
--DROP TABLE dbo.Products
