/* 
 ================================================================
 STATIONERY & PHOTOCOPY SYSTEM - COMPLETE DATABASE SETUP
 ================================================================
 Features: Tables, Stored Procedures, Triggers, and Seed Data.
 */

CREATE DATABASE PhotocopySystemDB;
GO

USE PhotocopySystemDB;
GO

-- ==============================================================
-- 1. TABLES
-- ==============================================================

CREATE TABLE [Users] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [FullName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [Role] NVARCHAR(50) NOT NULL, -- Admin, Teacher, Student
    [CreatedAt] DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE [Subjects] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [Name] NVARCHAR(100) NOT NULL
);

CREATE TABLE [Notes] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [Title] NVARCHAR(200) NOT NULL,
    [Version] NVARCHAR(50) NOT NULL,
    [TeacherId] INT NOT NULL FOREIGN KEY REFERENCES [Users]([Id]),
    [SubjectId] INT NOT NULL FOREIGN KEY REFERENCES [Subjects]([Id]),
    [UploadedAt] DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE [Categories] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [Name] NVARCHAR(100) NOT NULL
);

CREATE TABLE [Products] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [Name] NVARCHAR(100) NOT NULL,
    [Price] DECIMAL(18, 2) NOT NULL,
    [CategoryId] INT NOT NULL FOREIGN KEY REFERENCES [Categories]([Id])
);

CREATE TABLE [InventoryStocks] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [ProductId] INT NOT NULL FOREIGN KEY REFERENCES [Products]([Id]),
    [QuantityAvailable] INT NOT NULL DEFAULT 0,
    [RowVersion] ROWVERSION -- For Concurrency Control
);

CREATE TABLE [PrintJobs] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [StudentId] INT NOT NULL FOREIGN KEY REFERENCES [Users]([Id]),
    [NoteId] INT NULL FOREIGN KEY REFERENCES [Notes]([Id]),
    [DocumentName] NVARCHAR(255) NULL,
    [Copies] INT NOT NULL DEFAULT 1,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Queued', -- Queued, Printing, Completed, Cancelled
    [EstimatedPickupTime] DATETIME2 NOT NULL,
    [FineAmount] DECIMAL(18, 2) NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE [Orders] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [UserId] INT NOT NULL FOREIGN KEY REFERENCES [Users]([Id]),
    [OrderDate] DATETIME2 DEFAULT GETDATE(),
    [TotalAmount] DECIMAL(18, 2) NOT NULL
);

CREATE TABLE [OrderItems] (
    [Id] INT PRIMARY KEY IDENTITY(1, 1),
    [OrderId] INT NOT NULL FOREIGN KEY REFERENCES [Orders]([Id]),
    [ProductId] INT NOT NULL FOREIGN KEY REFERENCES [Products]([Id]),
    [Quantity] INT NOT NULL,
    [UnitPrice] DECIMAL(18, 2) NOT NULL
);
GO

-- ==============================================================
-- 2. STORED PROCEDURES
-- ==============================================================

CREATE PROCEDURE sp_RequestPrint 
    @StudentId INT,
    @NoteId INT = NULL,
    @DocumentName NVARCHAR(255) = NULL,
    @Copies INT 
AS 
BEGIN
    INSERT INTO PrintJobs (
        StudentId,
        NoteId,
        DocumentName,
        Copies,
        Status,
        EstimatedPickupTime
    )
    VALUES (
        @StudentId,
        @NoteId,
        @DocumentName,
        @Copies,
        'Queued',
        DATEADD(MINUTE, 30, GETDATE())
    );
END;
GO 

CREATE PROCEDURE sp_UpdatePrintStatus 
    @JobId INT,
    @NewStatus NVARCHAR(50) 
AS 
BEGIN
    UPDATE PrintJobs
    SET Status = @NewStatus
    WHERE Id = @JobId;
END;
GO

-- ==============================================================
-- 3. TRIGGERS
-- ==============================================================

-- Trigger: Automatically deduct stock when an order is placed
CREATE TRIGGER trg_AutoDeductStock ON OrderItems
AFTER INSERT 
AS 
BEGIN
    UPDATE InventoryStocks
    SET QuantityAvailable = QuantityAvailable - (
        SELECT Quantity
        FROM inserted
        WHERE ProductId = InventoryStocks.ProductId
    )
    FROM InventoryStocks
    JOIN inserted ON InventoryStocks.ProductId = inserted.ProductId;
END;
GO 

-- Trigger: Apply fine if a job is cancelled while printing
CREATE TRIGGER trg_ApplyCancellationFine ON PrintJobs
AFTER UPDATE 
AS 
BEGIN 
    IF UPDATE(Status) 
    BEGIN
        UPDATE PrintJobs
        SET FineAmount = 50.00
        FROM PrintJobs j
        JOIN inserted i ON j.Id = i.Id
        JOIN deleted d ON j.Id = d.Id
        WHERE i.Status = 'Cancelled'
        AND d.Status = 'Printing';
    END
END;
GO

-- ==============================================================
-- 4. INITIAL SEED DATA
-- ==============================================================

-- A. Seed Users
INSERT INTO [Users] ([FullName], [Email], [PasswordHash], [Role]) VALUES 
('System Admin', 'admin@system.com', 'Admin123', 'Admin'),
('Dr. Ahmed Ali', 'ahmed@univ.com', 'Teacher123', 'Teacher'),
('Student One', 'student1@univ.com', 'Student123', 'Student');
GO

-- B. Seed Categories
INSERT INTO [Categories] ([Name]) VALUES 
('Writing'),
('Paper'),
('Binding');
GO

-- C. Seed Products
INSERT INTO [Products] ([Name], [Price], [CategoryId]) VALUES 
('Ballpoint Pen (Blue)', 25.00, 1),
('Pencil HB', 15.00, 1),
('A4 Paper Ream (500 sheets)', 850.00, 2),
('Ruled Notebook (200 pages)', 350.00, 2),
('Spiral Binding', 120.00, 3);
GO

-- D. Seed Inventory Stocks
INSERT INTO [InventoryStocks] ([ProductId], [QuantityAvailable]) VALUES 
(1, 200),
(2, 300),
(3, 50),
(4, 75),
(5, 100);
GO
