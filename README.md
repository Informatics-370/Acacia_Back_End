# Acacia_Back_End
--------------------------------------------------------------------------
--Supplier Returns
--------------------------------------------------------------------------
CREATE VIEW SupplierReturnsView AS
SELECT
    so.ManagerEmail AS Email,
    so.Date AS TransactionDate,
    so.Total AS Amount,
    SUM(oi.Quantity) AS Quantity,
    'Supplier Return' AS Type 
FROM SupplierReturns so
INNER JOIN SupplierReturnItems oi ON so.Id = oi.SupplierReturnId
GROUP BY so.Id, so.ManagerEmail, so.Date, so.Total;

CREATE PROCEDURE GetSupplierReturnsBySearch
    @Search NVARCHAR(MAX)
AS
BEGIN
    SELECT *
    FROM SupplierReturnsView
    WHERE Email LIKE '%' + @Search + '%';
END;


Select * From SupplierReturnsView
DROP PROCEDURE GetSupplierReturnsBySearch;
DROP VIEW SupplierReturnsView;
EXEC GetSupplierReturnsBySearch @Search = 'mzamotembe7@gmail.com';

--------------------------------------------------------------------------
--Write Offs
--------------------------------------------------------------------------

CREATE VIEW WriteOffsView AS
SELECT
    so.ManagerEmail AS Email,
    so.Date AS TransactionDate,
    so.ProductPrice AS Amount,
    so.Quantity AS Quantity,
    'Write Off' AS Type 
FROM WriteOffs so
GROUP BY so.Id, so.ManagerEmail, so.Date, so.Quantity, so.ProductPrice;

CREATE PROCEDURE GetWriteOffsBySearch
    @Search NVARCHAR(MAX)
AS
BEGIN
    SELECT *
    FROM WriteOffsView
    WHERE Email LIKE '%' + @Search + '%';
END;


Select * From WriteOffsView
DROP PROCEDURE GetWriteOffsBySearch;
DROP VIEW WriteOffsView;
EXEC GetWriteOffsBySearch @Search = 'mzamotembe7@gmail.com';

--------------------------------------------------------------------------
--Sale Orders
--------------------------------------------------------------------------

--Customer Order
CREATE VIEW SalesOrderView AS
SELECT
    so.CustomerEmail AS Email,
    so.OrderDate AS TransactionDate,
    (so.SubTotal - so.Savings + dm.Price * (1 - so.GroupElephantDiscount/100)) AS Amount,
    SUM(oi.Quantity) AS Quantity,
    'Sale Order' AS Type 
FROM Orders so
INNER JOIN OrderItems oi ON so.Id = oi.OrderId
INNER JOIN DeliveryMethods dm ON dm.Id = so.DeliveryMethodId
GROUP BY so.Id, so.CustomerEmail, so.OrderDate, so.SubTotal, so.Savings, dm.Price, so.GroupElephantDiscount;

CREATE PROCEDURE GetOrdersBySearch
    @Search NVARCHAR(MAX)
AS
BEGIN
    SELECT *
    FROM SalesOrderView
    WHERE Email LIKE '%' + @Search + '%';
END;


Select * From SalesOrderView
DROP PROCEDURE GetOrdersBySearch;
DROP VIEW SalesOrderView;
EXEC GetOrdersBySearch @Search = 'mzamotembe7@gmail.com';

--------------------------------------------------------------------------
--Supplier Orders
--------------------------------------------------------------------------

CREATE VIEW SupplierOrderCombinedView AS
SELECT
    so.ManagerEmail AS Email,
    so.OrderDate AS TransactionDate,
    so.Total AS Amount,
    SUM(oi.Quantity) AS Quantity,
    'Supplier Order' AS Type 
FROM SupplierOrders so
INNER JOIN SupplierOrderItems oi ON so.Id = oi.SupplierOrderId
GROUP BY so.Id, so.ManagerEmail, so.OrderDate, so.Total;

CREATE PROCEDURE GetSupplierOrdersBySearch
    @Search NVARCHAR(MAX)
AS
BEGIN
    SELECT *
    FROM SupplierOrderCombinedView
    WHERE Email LIKE '%' + @Search + '%';
END;


Select * From SupplierOrderCombinedView
DROP PROCEDURE GetSupplierOrdersBySearch;
DROP VIEW SupplierOrderCombinedView;
EXEC GetSupplierOrdersBySearch @Search = 'mzamotembe7@gmail.com';

--------------------------------------------------------------------------
--Sale Returns
--------------------------------------------------------------------------

CREATE VIEW SalesReturnsView AS
SELECT
    so.CustomerEmail AS Email,
    so.Date AS TransactionDate,
    so.Total AS Amount,
    SUM(oi.Quantity) AS Quantity,
    'Sale Return' AS Type 
FROM CustomerReturns so
INNER JOIN ReturnItems oi ON so.Id = oi.CustomerReturnId
GROUP BY so.Id, so.CustomerEmail, so.Date, so.Total;

CREATE PROCEDURE GetCustomerReturnsBySearch
    @Search NVARCHAR(MAX)
AS
BEGIN
    SELECT *
    FROM SalesReturnsView
    WHERE Email LIKE '%' + @Search + '%';
END;


Select * From SalesReturnsView
DROP PROCEDURE GetCustomerReturnsBySearch;
DROP VIEW SalesReturnsView;
EXEC GetCustomerReturnsBySearch @Search = 'mzamotembe7@gmail.com';

----------------------------------------------------------------------------
Remove contraints all tables
----------------------------------------------------------------------------
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql = @sql + 'ALTER TABLE ' + t.name + ' DROP CONSTRAINT ' + c.name + ';
'
FROM sys.foreign_keys AS c
JOIN sys.tables AS t ON c.parent_object_id = t.object_id
WHERE c.type = 'F';

EXEC sp_executesql @sql;


----------------------------------------------------------------------------
Delete all tables
----------------------------------------------------------------------------
DECLARE @Sql NVARCHAR(MAX) = ''

SELECT @Sql = @Sql + 'DROP TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + '; '
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'

EXEC sp_executesql @Sql
