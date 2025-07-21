USE [Cuaderno_del_Profe]
GO

INSERT INTO [dbo].[Usuarios] ([NombreUsuario],[Email],[ContrasenaHash])
VALUES ('admin', 'admin@cuaderno_del_profe.com','$2a$11$WaB0nHfJVWgNOWOQStffR.ebC888fxNnkWSRrzlfifM/nL3s8BjUK') --Contraseña: 123456
GO

INSERT INTO Roles (Nombre) VALUES ('Admin'), ('Professor'), ('Student') 
GO

INSERT INTO UsuarioRoles (UsuarioId, RolId)
SELECT (SELECT TOP 1 id FROM Usuarios WHERE NombreUsuario = 'Admin') UsuarioId, (SELECT TOP 1 id FROM Roles WHERE Nombre = 'Admin') RolId
GO

INSERT INTO Materia ([Nombre] ,[Descripcion]) 
SELECT 'Lengua española' Nombres, NULL Descripcion UNION
SELECT 'Matemáticas' Nombres, NULL Descripcion UNION
SELECT 'Ciencias sociales' Nombres, NULL Descripcion UNION
SELECT 'Ciencias naturales' Nombres, NULL Descripcion 
GO

INSERT INTO Periodo (Nombre, FInicio, FFin)
SELECT '1er Semestre 2025-2026' Nombre, '2025-08-25' FInicio, '2025-12-19' FFin UNION 
SELECT '2do Semestre 2025-2026' Nombre, '2026-06-07' FInicio, '2026-07-17' FFin 

GO


