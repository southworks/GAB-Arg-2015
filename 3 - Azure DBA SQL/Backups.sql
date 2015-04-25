/* backup azure*/

IF NOT EXISTS
(SELECT * FROM sys.credentials 
WHERE credential_identity = 'azuretrdb')
CREATE CREDENTIAL mycredential WITH IDENTITY = 'mystorageaccount'
,SECRET = '<storage access key>' ;


BACKUP DATABASE Azuredb
TO URL = 'https://trdbsql.blob.core.windows.net/backupsql/azuredb.bak' 
      WITH CREDENTIAL = 'mycredential' 
     ,COMPRESSION
     ,STATS = 5;
GO 

BACKUP DATABASE Azuredb
TO URL = 'https://trdbsql.blob.core.windows.net/backupsql/azuredb.bak' 
      WITH CREDENTIAL = 'mycredential' 
     ,COMPRESSION
     ,STATS = 5;
GO 


BACKUP DATABASE [AZUREDB] 
TO  URL = N'https://trdbsql.blob.core.windows.net/backupsql/AZUREDB2.bak' 
WITH  CREDENTIAL = N'mycredential' , 
NOFORMAT, 
NOINIT,  
NOSKIP, 
NOREWIND, 
NOUNLOAD, 
ENCRYPTION(ALGORITHM = TRIPLE_DES_3KEY, SERVER CERTIFICATE = [CertificadoBK]),  STATS = 10
GO

RESTORE DATABASE azuredb2 
FROM URL = 'https://trdbsql.blob.core.windows.net/backupsql/AZUREDB2.bak'
WITH CREDENTIAL = 'mycredential'
,MOVE 'azuredb' to 'C:\temp\azuredb2.mdf'
,MOVE 'azuredb_log' to 'C:\temp\azuredb2.ldf'
,STATS = 5 




----- creamos datafiles en azure

-- Create a credential
CREATE CREDENTIAL [https://testdb.blob.core.windows.net/data]
WITH IDENTITY='SHARED ACCESS SIGNATURE',
SECRET = 'your SAS key'

CREATE DATABASE testdbazure 
ON
( NAME = testdb_dat,
    FILENAME = 'https://trdbsql.blob.core.windows.net/backupsql/TestData.mdf' )
 LOG ON
( NAME = testdb_log,
    FILENAME =  'https://trdbsql.blob.core.windows.net/backupsql/TestLog.ldf') 