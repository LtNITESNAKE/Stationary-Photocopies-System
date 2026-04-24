-- Database Script for Photocopy System
CREATE DATABASE PhotocopySystemDB 
 
USE PhotocopySystemDB 
 

-- 1. Tables
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(MAX),
    Role NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE()
) 

CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
) 

CREATE TABLE Subjects (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
) 

CREATE TABLE Notes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TeacherId INT FOREIGN KEY REFERENCES Users(Id),
    SubjectId INT FOREIGN KEY REFERENCES Subjects(Id),
    Title NVARCHAR(200),
    Version NVARCHAR(50),
    FilePath NVARCHAR(MAX),
    UploadedAt DATETIME DEFAULT GETDATE()
) 

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(18,2) NOT NULL,
    CategoryId INT FOREIGN KEY REFERENCES Categories(Id)
) 

CREATE TABLE InventoryStocks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT FOREIGN KEY REFERENCES Products(Id),
    QuantityAvailable INT DEFAULT 0,
    RowVersion ROWVERSION
) 

CREATE TABLE ServerNodes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ServerName NVARCHAR(100),
    IPAddress NVARCHAR(50),
    IsOnline BIT DEFAULT 1,
    CurrentActiveJobs INT DEFAULT 0
) 

CREATE TABLE PrintJobs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT FOREIGN KEY REFERENCES Users(Id),
    NoteId INT NULL FOREIGN KEY REFERENCES Notes(Id),
    DocumentName NVARCHAR(255),
    Copies INT,
    Status NVARCHAR(50) DEFAULT 'Queued',
    AssignedServerId INT NULL FOREIGN KEY REFERENCES ServerNodes(Id),
    EstimatedPickupTime DATETIME NULL,
    FineAmount DECIMAL(18,2) DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
) 

CREATE TABLE ProxyLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IncomingIp NVARCHAR(50),
    RequestType NVARCHAR(50),
    RoutedToServerId INT NULL FOREIGN KEY REFERENCES ServerNodes(Id),
    Timestamp DATETIME DEFAULT GETDATE()
) 

CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2),
    Status NVARCHAR(50) DEFAULT 'Cart'
) 

CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT FOREIGN KEY REFERENCES Orders(Id),
    ProductId INT FOREIGN KEY REFERENCES Products(Id),
    Quantity INT,
    UnitPrice DECIMAL(18,2)
) 

CREATE TABLE Suppliers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(100),
    ContactPerson NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(50)
) 

CREATE TABLE Reviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    ProductId INT FOREIGN KEY REFERENCES Products(Id),
    Rating INT CHECK (Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE()
) 
 

-- 2. Triggers (At least 3)

-- Trigger 1: Deduct stock when an OrderItem is inserted
CREATE TRIGGER trg_DeductStockOnOrder
ON OrderItems
AFTER INSERT
AS
BEGIN
    UPDATE InventoryStocks
    SET QuantityAvailable = QuantityAvailable - i.Quantity
    FROM InventoryStocks
    INNER JOIN inserted i ON InventoryStocks.ProductId = i.ProductId 
END 
 

-- Trigger 2: Increment Server Active Jobs when a Print Job is assigned
CREATE TRIGGER trg_IncrementServerJobs
ON PrintJobs
AFTER UPDATE
AS
BEGIN
    IF UPDATE(AssignedServerId)
    BEGIN
        UPDATE ServerNodes
        SET CurrentActiveJobs = CurrentActiveJobs + 1
        FROM ServerNodes
        INNER JOIN inserted i ON ServerNodes.Id = i.AssignedServerId
        WHERE i.AssignedServerId IS NOT NULL AND i.Status = 'Processing' 
    END
END 
 

-- Trigger 3: Decrement Server Active Jobs when a Print Job is completed or failed
CREATE TRIGGER trg_DecrementServerJobs
ON PrintJobs
AFTER UPDATE
AS
BEGIN
    IF UPDATE(Status)
    BEGIN
        UPDATE ServerNodes
        SET CurrentActiveJobs = CurrentActiveJobs - 1
        FROM ServerNodes
        INNER JOIN inserted i ON ServerNodes.Id = i.AssignedServerId
        WHERE i.Status IN ('Completed', 'Failed') AND i.AssignedServerId IS NOT NULL 
    END
END 
 

-- 3. Stored Procedures (5 to 10)

-- SP 1: Get User by Email
CREATE PROCEDURE sp_GetUserByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Users WHERE Email = @Email 
END 
 

-- SP 2: Get Available Stock for Product
CREATE PROCEDURE sp_GetAvailableStock
    @ProductId INT
AS
BEGIN
    SELECT QuantityAvailable FROM InventoryStocks WHERE ProductId = @ProductId 
END 
 

-- SP 3: Create Print Job
CREATE PROCEDURE sp_CreatePrintJob
    @StudentId INT,
    @NoteId INT = NULL,
    @DocumentName NVARCHAR(255),
    @Copies INT
AS
BEGIN
    INSERT INTO PrintJobs (StudentId, NoteId, DocumentName, Copies)
    VALUES (@StudentId, @NoteId, @DocumentName, @Copies) 
    SELECT SCOPE_IDENTITY() AS NewJobId 
END 
 

-- SP 4: Assign Print Job to Server
CREATE PROCEDURE sp_AssignPrintJob
    @JobId INT,
    @ServerId INT
AS
BEGIN
    UPDATE PrintJobs
    SET AssignedServerId = @ServerId, Status = 'Processing'
    WHERE Id = @JobId 
END 
 

-- SP 5: Get Least Loaded Server (For Load Balancer)
CREATE PROCEDURE sp_GetLeastLoadedServer
AS
BEGIN
    SELECT TOP 1 *
    FROM ServerNodes
    WHERE IsOnline = 1
    ORDER BY CurrentActiveJobs ASC 
END 
 

-- SP 6: Update Order Status
CREATE PROCEDURE sp_UpdateOrderStatus
    @OrderId INT,
    @NewStatus NVARCHAR(50)
AS
BEGIN
    UPDATE Orders
    SET Status = @NewStatus
    WHERE Id = @OrderId 
END 
 

-- SP 7: Insert Proxy Log
CREATE PROCEDURE sp_InsertProxyLog
    @IncomingIp NVARCHAR(50),
    @RequestType NVARCHAR(50),
    @RoutedToServerId INT
AS
BEGIN
    INSERT INTO ProxyLogs (IncomingIp, RequestType, RoutedToServerId)
    VALUES (@IncomingIp, @RequestType, @RoutedToServerId) 
END 
 
