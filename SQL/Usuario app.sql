-- 1. Crear el LOGIN a nivel de servidor
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'app')
BEGIN
    CREATE LOGIN [app] WITH PASSWORD = '#prueba@2025';
END
GO

-- 2. Usar la base de datos Cuaderno_del_Profe
USE [Cuaderno_del_Profe];
GO

-- 3. Crear el USER dentro de la base de datos si no existe
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'app')
BEGIN
    CREATE USER [app] FOR LOGIN [app];
END
GO

-- 4. Crear rol db_reader si no existe y asignar permisos
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'db_reader')
BEGIN
    CREATE ROLE [db_reader];
    GRANT SELECT ON DATABASE::[Cuaderno_del_Profe] TO [db_reader];
END
GO

-- 5. Crear rol db_writer si no existe y asignar permisos
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'db_writer')
BEGIN
    CREATE ROLE [db_writer];
    GRANT INSERT, UPDATE, DELETE ON DATABASE::[Cuaderno_del_Profe] TO [db_writer];
END
GO

-- 6. Agregar el usuario a los roles
ALTER ROLE [db_reader] ADD MEMBER [app];
ALTER ROLE [db_writer] ADD MEMBER [app];
GO
