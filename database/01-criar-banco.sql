-- Execute no SSMS ou sqlcmd. A aplicação não cria o banco automaticamente.
USE master;
GO
IF DB_ID(N'dbShopee') IS NULL CREATE DATABASE dbShopee;
GO
USE dbShopee;
GO
IF OBJECT_ID(N'dbo.Produto', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Produto (
        Codigo INT IDENTITY(1,1) PRIMARY KEY,
        Nome VARCHAR(100) NOT NULL,
        Preco DECIMAL(10,2) NOT NULL,
        Estoque INT NOT NULL
    );
END;
GO
-- Dados iniciais inseridos somente quando a tabela está vazia.
IF NOT EXISTS (SELECT 1 FROM dbo.Produto)
BEGIN
    INSERT INTO dbo.Produto (Nome, Preco, Estoque) VALUES
    ('Mouse Gamer', 80.00, 20),
    ('Teclado Mecânico', 150.00, 15),
    ('Headset Gamer', 120.00, 10),
    ('Mouse Pad', 35.00, 30),
    ('Webcam Full HD', 200.00, 8);
END;
GO
SELECT * FROM dbo.Produto;
