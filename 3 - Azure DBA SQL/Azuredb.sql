/* demodb azure */

CREATE DATABASE AZUREDB
GO

USE AZUREDB
GO

SELECT * INTO CLIENTES
 FROM AdventureWorks2012.Sales.Customer  

create clustered index c1 on clientes (customerid)