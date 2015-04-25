/*
  rls azure v2
*/

--- creamos las cuentas

CREATE USER Manager WITHOUT LOGIN;
CREATE USER Sales1 WITHOUT LOGIN;
CREATE USER Sales2 WITHOUT LOGIN;

--- creamos las tablas

CREATE TABLE Sales
    (
    OrderID int,
    SalesRep sysname,
    Product varchar(10),
    Qty int
    );

--- insertamos informacion en las tablas

INSERT Sales VALUES 
(1, 'Sales1', 'Valve', 5), 
(2, 'Sales1', 'Wheel', 2), 
(3, 'Sales1', 'Valve', 4),
(4, 'Sales2', 'Bracket', 2), 
(5, 'Sales2', 'Wheel', 5), 
(6, 'Sales2', 'Seat', 5);
-- View the 6 rows in the table
SELECT * FROM Sales;


---- damos acceso a los usuarios

GRANT SELECT ON Sales TO Manager;
GRANT SELECT ON Sales TO Sales1;
GRANT SELECT ON Sales TO Sales2;


--- creamos el schema de funcion

CREATE SCHEMA Security;
GO

CREATE FUNCTION Security.fn_securitypredicate(@SalesRep AS sysname)
    RETURNS TABLE
WITH SCHEMABINDING
AS
    RETURN SELECT 1 AS fn_securitypredicate_result 
    WHERE @SalesRep = USER_NAME() OR USER_NAME() = 'Manager';

--- creamos una directiva de seguridad

CREATE SECURITY POLICY SalesFilter
ADD FILTER PREDICATE Security.fn_securitypredicate(SalesRep) 
ON dbo.Sales
WITH (STATE = ON);

--- probamos

EXECUTE AS USER = 'Sales1';
SELECT * FROM Sales; 
REVERT;

EXECUTE AS USER = 'Sales2';
SELECT * FROM Sales; 
REVERT;

EXECUTE AS USER = 'Manager';
SELECT * FROM Sales; 
REVERT;

--- limpiamos

drop USER Manager;
drop USER Sales1;
drop USER Sales2;

drop SECURITY POLICY SalesFilter
drop function Security.fn_securitypredicate
drop schema security
drop table sales