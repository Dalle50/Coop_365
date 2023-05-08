CREATE TABLE dbo.Products(
ProductID INT NOT NULL PRIMARY KEY,
ProductName varchar(255) NOT NULL,
Stock INT,
Price float NOT NULL,
Description TEXT NOT NULL,
Url varchar(255),
)

CREATE TABLE dbo.Orders(
OrderID int NOT NULL PRIMARY KEY,
totalPrice float NOT NULL,

)
CREATE TABLE dbo.OrderLines(
OrderLineID int NOT NULL PRIMARY KEY,
Amount int NOT NULL,
Date DateTime NOT NULL,
ProductID int FOREIGN KEY REFERENCES dbo.Products(ProductID),
OrderID int FOREIGN KEY REFERENCES dbo.Orders(OrderID),
)

CREATE TABLE dbo.OrderTicket(
Id int NOT NULL PRIMARY KEY,
OrderId int FOREIGN KEY REFERENCES dbo.Orders(OrderId),
Date DateTime NOT NULL,
QRCode VARBINARY(MAX)NOT NULL,
)

