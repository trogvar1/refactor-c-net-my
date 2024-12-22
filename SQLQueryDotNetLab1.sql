CREATE DATABASE ATMDatabase;

USE ATMDatabase;

CREATE TABLE Accounts (
    CardNumber NVARCHAR(16) PRIMARY KEY,
    FirstName NVARCHAR(50), 
    LastName NVARCHAR(50), 
    Balance DECIMAL(18, 2),
    PinCode NVARCHAR(4) 
);

CREATE TABLE ATMs (
    AtmId NVARCHAR(50) PRIMARY KEY,
    Location NVARCHAR(100),
    CashAvailable DECIMAL(18, 2)
);

CREATE TABLE Banks (
    BankName NVARCHAR(100) PRIMARY KEY 
);

CREATE TABLE BankATMs (
    BankName NVARCHAR(100),
    AtmId NVARCHAR(50),
    CONSTRAINT FK_Bank FOREIGN KEY (BankName) REFERENCES Banks(BankName),
    CONSTRAINT FK_Atm FOREIGN KEY (AtmId) REFERENCES ATMs(AtmId)
);

INSERT INTO Accounts (CardNumber, FirstName, LastName, Balance, PinCode) VALUES
('1111222233334445', 'Lesha', 'Semenchuk', 3000.00, '1112'),
('2222333344445556', 'Sasha', 'Sushko', 4200.00, '2223'),
('3333444455556667', 'Denys', 'Linevych', 1750.00, '3334'),
('4444555566667778', 'Vlad', 'Shevchuk', 5000.00, '4445');

INSERT INTO ATMs (AtmId, Location, CashAvailable) VALUES
('ATM001', '250 Lake st', 13000.00),
('ATM002', '600 Can St', 10000.00),
('ATM003', '900 Beaver Rd', 8000.00),
('ATM004', '550 Cut Blvd', 7000.00),
('ATM005', '200 Sea Dr', 13000.00);

INSERT INTO Banks (BankName) VALUES
('National Bank of USA'),
('City Bank of NYC'),
('First Trust Bank of Trump'),
('Global Bank from UK');

INSERT INTO BankATMs (BankName, AtmId) VALUES
('National Bank of USA', 'ATM001'),
('National Bank of USA', 'ATM002'),
('City Bank of NYC', 'ATM003'),
('First Trust Bank of Trump', 'ATM004'),
('Global Bank from UK', 'ATM005'),
('National Bank of USA', 'ATM003');