CREATE DATABASE AgenciaDeVuelo;
GO

select * from Paquetes;


USE AgenciaDeVuelo;
GO


CREATE TABLE Empleados
(
    Usuario VARCHAR(15) NOT NULL PRIMARY KEY,

    Contrasenia VARCHAR(10) NOT NULL
        CHECK (
            LEN(Contrasenia) BETWEEN 5 AND 10
            AND Contrasenia LIKE '%[A-Za-z]%'
            AND Contrasenia LIKE '%[0-9]%'
            AND Contrasenia LIKE '%[^A-Za-z0-9]%'
        ),

    NombreCompleto VARCHAR(100) NOT NULL,

    Activo BIT NOT NULL DEFAULT 1
);
GO


CREATE TABLE Estados
(
    Codigo VARCHAR(5) NOT NULL PRIMARY KEY
        CHECK (
            LEN(Codigo) BETWEEN 4 AND 5
            AND Codigo NOT LIKE '%[^A-Za-z]%'
        ),

    Nombre VARCHAR(50) NOT NULL,

    Pais VARCHAR(50) NOT NULL,

    Activo BIT NOT NULL DEFAULT 1
);
GO


CREATE TABLE Vuelos
(
    Codigo CHAR(10) NOT NULL PRIMARY KEY
        CHECK (LEN(Codigo) = 10),

    FechaHoraPartida DATETIME NOT NULL,

    CodigoEstadoPartida VARCHAR(5) NOT NULL,

    FechaHoraLlegada DATETIME NOT NULL,

    CodigoEstadoLlegada VARCHAR(5) NOT NULL,

    PrecioPasaje DECIMAL(10,2) NOT NULL
        CHECK (PrecioPasaje > 0),

    Activo BIT NOT NULL DEFAULT 1,

    FOREIGN KEY (CodigoEstadoPartida)
        REFERENCES Estados(Codigo),

    FOREIGN KEY (CodigoEstadoLlegada)
        REFERENCES Estados(Codigo),

    CHECK (FechaHoraLlegada > FechaHoraPartida),

    CHECK (CodigoEstadoPartida <> CodigoEstadoLlegada)
);
GO


CREATE TABLE Hospedajes
(
    CodigoInterno VARCHAR(10) NOT NULL PRIMARY KEY
        CHECK (
            LEN(CodigoInterno) <= 10
            AND CodigoInterno NOT LIKE '%[^A-Za-z]%'
        ),

    Nombre VARCHAR(80) NOT NULL,

    Direccion VARCHAR(150) NOT NULL,

    TipoHospedaje VARCHAR(20) NOT NULL
        CHECK (
            TipoHospedaje IN
            ('Hotel STD', 'Posada', 'All Inclusive')
        ),

    PrecioNochePersona DECIMAL(10,2) NOT NULL
        CHECK (PrecioNochePersona > 0),

    CodigoEstado VARCHAR(5) NOT NULL,

    Activo BIT NOT NULL DEFAULT 1,

    FOREIGN KEY (CodigoEstado)
        REFERENCES Estados(Codigo)
);
GO


CREATE TABLE Paquetes
(
    Codigo INT IDENTITY(1,1) NOT NULL PRIMARY KEY,

    Titulo VARCHAR(100) NOT NULL,

    Descripcion VARCHAR(500) NOT NULL,

    CodigoEstadoDestino VARCHAR(5) NOT NULL,

    CantDias INT NOT NULL
        CHECK (CantDias > 0),

    PrecioIndividual DECIMAL(10,2) NOT NULL
        CHECK (PrecioIndividual > 0),

    PrecioDoble DECIMAL(10,2) NOT NULL
        CHECK (PrecioDoble > 0),

    PrecioTriple DECIMAL(10,2) NOT NULL
        CHECK (PrecioTriple > 0),

    CodigoVueloIda CHAR(10) NOT NULL,

    CodigoVueloVuelta CHAR(10) NOT NULL,

    UsuarioEmpleado VARCHAR(15) NOT NULL,

    Activo BIT NOT NULL DEFAULT 1,

    FOREIGN KEY (CodigoEstadoDestino)
        REFERENCES Estados(Codigo),

    FOREIGN KEY (CodigoVueloIda)
        REFERENCES Vuelos(Codigo),

    FOREIGN KEY (CodigoVueloVuelta)
        REFERENCES Vuelos(Codigo),

    FOREIGN KEY (UsuarioEmpleado)
        REFERENCES Empleados(Usuario),

    CHECK (CodigoVueloIda <> CodigoVueloVuelta)
);
GO


CREATE TABLE PaqueteHospedaje
(
    CodigoPaquete INT NOT NULL,

    CodigoHospedaje VARCHAR(10) NOT NULL,

    CantNoches INT NOT NULL
        CHECK (CantNoches > 0),

    PRIMARY KEY (CodigoPaquete, CodigoHospedaje),

    FOREIGN KEY (CodigoPaquete)
        REFERENCES Paquetes(Codigo),

    FOREIGN KEY (CodigoHospedaje)
        REFERENCES Hospedajes(CodigoInterno)
);
GO

-- Empleado

CREATE PROCEDURE EmpleadoAlta
    @Usuario VARCHAR(15),
    @Contrasenia VARCHAR(10),
    @NombreCompleto VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Empleados WHERE Usuario = @Usuario)
        RETURN -1;

    IF LEN(@Contrasenia) NOT BETWEEN 5 AND 10
        RETURN -2;

    IF @Contrasenia NOT LIKE '%[A-Za-z]%'
        RETURN -3;

    IF @Contrasenia NOT LIKE '%[0-9]%'
        RETURN -4;

    IF @Contrasenia NOT LIKE '%[^A-Za-z0-9]%'
        RETURN -5;

    INSERT INTO Empleados
    VALUES (@Usuario, @Contrasenia, @NombreCompleto, 1);

    DECLARE @Sql NVARCHAR(MAX);

    SET @Sql =
        'CREATE LOGIN [' + @Usuario + '] 
         WITH PASSWORD = ''' + @Contrasenia + ''',
         CHECK_POLICY = OFF';

    EXEC(@Sql);

    SET @Sql =
        'CREATE USER [' + @Usuario + '] 
         FOR LOGIN [' + @Usuario + ']';

    EXEC(@Sql);

    SET @Sql =
        'GRANT EXECUTE TO [' + @Usuario + ']';

    EXEC(@Sql);

    RETURN 1;
END;
GO

CREATE PROCEDURE EmpleadoModificar
    @Usuario VARCHAR(15),
    @Contrasenia VARCHAR(10),
    @NombreCompleto VARCHAR(100),
    @Activo BIT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Empleados WHERE Usuario = @Usuario)
        RETURN -1;

    IF LEN(@Contrasenia) NOT BETWEEN 5 AND 10
        RETURN -2;

    IF @Contrasenia NOT LIKE '%[A-Za-z]%'
        RETURN -3;

    IF @Contrasenia NOT LIKE '%[0-9]%'
        RETURN -4;

    IF @Contrasenia NOT LIKE '%[^A-Za-z0-9]%'
        RETURN -5;

    UPDATE Empleados
    SET Contrasenia = @Contrasenia,
        NombreCompleto = @NombreCompleto,
        Activo = @Activo
    WHERE Usuario = @Usuario;

    RETURN 1;
END;
GO

CREATE PROCEDURE EmpleadoBuscar
    @Usuario VARCHAR(15)
AS
BEGIN
    SELECT Usuario, Contrasenia, NombreCompleto, Activo
    FROM Empleados
    WHERE Usuario = @Usuario;
END;
GO

CREATE PROCEDURE EmpleadoLogueo
    @Usuario VARCHAR(15),
    @Contrasenia VARCHAR(10)
AS
BEGIN
    SELECT Usuario, NombreCompleto, Activo
    FROM Empleados
    WHERE Usuario = @Usuario
      AND Contrasenia = @Contrasenia
      AND Activo = 1;
END;
GO

CREATE PROCEDURE EmpleadoListado
AS
BEGIN
    SELECT Usuario,
           Contrasenia,
           NombreCompleto,
           Activo
    FROM Empleados
    ORDER BY NombreCompleto;
END;
GO

-- Estados

CREATE PROCEDURE EstadoAlta
    @Codigo VARCHAR(5),
    @Nombre VARCHAR(50),
    @Pais VARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT * FROM Estados WHERE Codigo = @Codigo)
        RETURN -1;

    INSERT INTO Estados
    VALUES (@Codigo, @Nombre, @Pais, 1);

    RETURN 1;
END;
GO

CREATE PROCEDURE EstadoModificar
    @Codigo VARCHAR(5),
    @Nombre VARCHAR(50),
    @Pais VARCHAR(50),
    @Activo BIT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @Codigo)
        RETURN -1;

    UPDATE Estados
    SET Nombre = @Nombre,
        Pais = @Pais,
        Activo = @Activo
    WHERE Codigo = @Codigo;

    RETURN 1;
END;
GO

CREATE PROCEDURE EstadoBaja
    @Codigo VARCHAR(5)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @Codigo)
        RETURN -1;

    IF EXISTS (SELECT * FROM Vuelos WHERE CodigoEstadoPartida = @Codigo OR CodigoEstadoLlegada = @Codigo)
        OR EXISTS (SELECT * FROM Hospedajes WHERE CodigoEstado = @Codigo)
        OR EXISTS (SELECT * FROM Paquetes WHERE CodigoEstadoDestino = @Codigo)
    BEGIN
        UPDATE Estados
        SET Activo = 0
        WHERE Codigo = @Codigo;

        RETURN 2;
    END

    DELETE FROM Estados
    WHERE Codigo = @Codigo;

    RETURN 1;
END;
GO

CREATE PROCEDURE EstadoBuscar
    @Codigo VARCHAR(5)
AS
BEGIN
    SELECT Codigo, Nombre, Pais, Activo
    FROM Estados
    WHERE Codigo = @Codigo;
END;
GO

CREATE PROCEDURE EstadoListado
    @FiltroNombre VARCHAR(50)
AS
BEGIN
    SELECT Codigo, Nombre, Pais, Activo
    FROM Estados
    WHERE Nombre LIKE '%' + @FiltroNombre + '%'
    ORDER BY Nombre;
END;
GO

-- vuelos

CREATE PROCEDURE VueloAlta
    @Codigo CHAR(10),
    @FechaHoraPartida DATETIME,
    @CodigoEstadoPartida VARCHAR(5),
    @FechaHoraLlegada DATETIME,
    @CodigoEstadoLlegada VARCHAR(5),
    @PrecioPasaje DECIMAL(10,2)
AS
BEGIN
    IF EXISTS (SELECT * FROM Vuelos WHERE Codigo = @Codigo)
        RETURN -1;

    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @CodigoEstadoPartida AND Activo = 1)
        RETURN -2;

    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @CodigoEstadoLlegada AND Activo = 1)
        RETURN -3;

    IF @FechaHoraLlegada <= @FechaHoraPartida
        RETURN -4;

    IF @CodigoEstadoPartida = @CodigoEstadoLlegada
        RETURN -5;

    IF @PrecioPasaje <= 0
        RETURN -6;

    INSERT INTO Vuelos
    VALUES
    (
        @Codigo,
        @FechaHoraPartida,
        @CodigoEstadoPartida,
        @FechaHoraLlegada,
        @CodigoEstadoLlegada,
        @PrecioPasaje,
        1
    );

    RETURN 1;
END;
GO

CREATE PROCEDURE VueloModificar
    @Codigo CHAR(10),
    @FechaHoraPartida DATETIME,
    @CodigoEstadoPartida VARCHAR(5),
    @FechaHoraLlegada DATETIME,
    @CodigoEstadoLlegada VARCHAR(5),
    @PrecioPasaje DECIMAL(10,2),
    @Activo BIT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Vuelos WHERE Codigo = @Codigo)
        RETURN -1;

    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @CodigoEstadoPartida AND Activo = 1)
        RETURN -2;

    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @CodigoEstadoLlegada AND Activo = 1)
        RETURN -3;

    IF @FechaHoraLlegada <= @FechaHoraPartida
        RETURN -4;

    IF @CodigoEstadoPartida = @CodigoEstadoLlegada
        RETURN -5;

    IF @PrecioPasaje <= 0
        RETURN -6;

    UPDATE Vuelos
    SET FechaHoraPartida = @FechaHoraPartida,
        CodigoEstadoPartida = @CodigoEstadoPartida,
        FechaHoraLlegada = @FechaHoraLlegada,
        CodigoEstadoLlegada = @CodigoEstadoLlegada,
        PrecioPasaje = @PrecioPasaje,
        Activo = @Activo
    WHERE Codigo = @Codigo;

    RETURN 1;
END;
GO

CREATE PROCEDURE VueloBaja
    @Codigo CHAR(10)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Vuelos WHERE Codigo = @Codigo)
        RETURN -1;

    IF EXISTS
    (
        SELECT *
        FROM Paquetes
        WHERE CodigoVueloIda = @Codigo
           OR CodigoVueloVuelta = @Codigo
    )
    BEGIN
        UPDATE Vuelos
        SET Activo = 0
        WHERE Codigo = @Codigo;

        RETURN 2;
    END

    DELETE FROM Vuelos
    WHERE Codigo = @Codigo;

    RETURN 1;
END;
GO

CREATE PROCEDURE VueloBuscar
    @Codigo CHAR(10)
AS
BEGIN
    SELECT Codigo,
           FechaHoraPartida,
           CodigoEstadoPartida,
           FechaHoraLlegada,
           CodigoEstadoLlegada,
           PrecioPasaje,
           Activo
    FROM Vuelos
    WHERE Codigo = @Codigo;
END;
GO

CREATE PROCEDURE VueloListado
AS
BEGIN
    SELECT Codigo,
           FechaHoraPartida,
           CodigoEstadoPartida,
           FechaHoraLlegada,
           CodigoEstadoLlegada,
           PrecioPasaje,
           Activo
    FROM Vuelos
    ORDER BY FechaHoraPartida;
END;
GO

CREATE PROCEDURE VuelosIdaPorEstado
    @CodigoEstadoDestino VARCHAR(5)
AS
BEGIN
    SELECT Codigo,
           FechaHoraPartida,
           FechaHoraLlegada,
           PrecioPasaje
    FROM Vuelos
    WHERE CodigoEstadoLlegada = @CodigoEstadoDestino
      AND Activo = 1
    ORDER BY FechaHoraPartida;
END;
GO

CREATE PROCEDURE VuelosVueltaPorEstado
    @CodigoEstadoDestino VARCHAR(5)
AS
BEGIN
    SELECT Codigo,
           FechaHoraPartida,
           FechaHoraLlegada,
           PrecioPasaje
    FROM Vuelos
    WHERE CodigoEstadoPartida = @CodigoEstadoDestino
      AND Activo = 1
    ORDER BY FechaHoraPartida;
END;
GO

--Hospedaje

CREATE PROCEDURE HospedajeAlta
    @CodigoInterno VARCHAR(10),
    @Nombre VARCHAR(80),
    @Direccion VARCHAR(150),
    @TipoHospedaje VARCHAR(20),
    @PrecioNochePersona DECIMAL(10,2),
    @CodigoEstado VARCHAR(5)
AS
BEGIN
    IF EXISTS (SELECT * FROM Hospedajes WHERE CodigoInterno = @CodigoInterno)
        RETURN -1;

    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @CodigoEstado AND Activo = 1)
        RETURN -2;

    IF @TipoHospedaje NOT IN ('Hotel STD', 'Posada', 'All Inclusive')
        RETURN -3;

    IF @PrecioNochePersona <= 0
        RETURN -4;

    INSERT INTO Hospedajes
    VALUES
    (
        @CodigoInterno,
        @Nombre,
        @Direccion,
        @TipoHospedaje,
        @PrecioNochePersona,
        @CodigoEstado,
        1
    );

    RETURN 1;
END;
GO

CREATE PROCEDURE HospedajeModificar
    @CodigoInterno VARCHAR(10),
    @Nombre VARCHAR(80),
    @Direccion VARCHAR(150),
    @TipoHospedaje VARCHAR(20),
    @PrecioNochePersona DECIMAL(10,2),
    @CodigoEstado VARCHAR(5),
    @Activo BIT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Hospedajes WHERE CodigoInterno = @CodigoInterno)
        RETURN -1;

    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @CodigoEstado AND Activo = 1)
        RETURN -2;

    IF @TipoHospedaje NOT IN ('Hotel STD', 'Posada', 'All Inclusive')
        RETURN -3;

    IF @PrecioNochePersona <= 0
        RETURN -4;

    UPDATE Hospedajes
    SET Nombre = @Nombre,
        Direccion = @Direccion,
        TipoHospedaje = @TipoHospedaje,
        PrecioNochePersona = @PrecioNochePersona,
        CodigoEstado = @CodigoEstado,
        Activo = @Activo
    WHERE CodigoInterno = @CodigoInterno;

    RETURN 1;
END;
GO

CREATE PROCEDURE HospedajeBaja
    @CodigoInterno VARCHAR(10)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Hospedajes WHERE CodigoInterno = @CodigoInterno)
        RETURN -1;

    IF EXISTS
    (
        SELECT *
        FROM PaqueteHospedaje
        WHERE CodigoHospedaje = @CodigoInterno
    )
    BEGIN
        UPDATE Hospedajes
        SET Activo = 0
        WHERE CodigoInterno = @CodigoInterno;

        RETURN 2;
    END

    DELETE FROM Hospedajes
    WHERE CodigoInterno = @CodigoInterno;

    RETURN 1;
END;
GO

CREATE PROCEDURE HospedajeBuscar
    @CodigoInterno VARCHAR(10)
AS
BEGIN
    SELECT CodigoInterno,
           Nombre,
           Direccion,
           TipoHospedaje,
           PrecioNochePersona,
           CodigoEstado,
           Activo
    FROM Hospedajes
    WHERE CodigoInterno = @CodigoInterno;
END;
GO

CREATE PROCEDURE HospedajeListado
    @FiltroNombre VARCHAR(80)
AS
BEGIN
    SELECT CodigoInterno,
           Nombre,
           Direccion,
           TipoHospedaje,
           PrecioNochePersona,
           CodigoEstado,
           Activo
    FROM Hospedajes
    WHERE Nombre LIKE '%' + @FiltroNombre + '%'
    ORDER BY Nombre;
END;
GO

CREATE PROCEDURE HospedajesPorEstado
    @CodigoEstadoDestino VARCHAR(5)
AS
BEGIN
    SELECT CodigoInterno,
           Nombre,
           Direccion,
           TipoHospedaje,
           PrecioNochePersona
    FROM Hospedajes
    WHERE CodigoEstado = @CodigoEstadoDestino
      AND Activo = 1
    ORDER BY Nombre;
END;
GO

--Paquete

CREATE PROCEDURE PaqueteAlta
    @Titulo VARCHAR(100),
    @Descripcion VARCHAR(500),
    @CodigoEstadoDestino VARCHAR(5),
    @CodigoVueloIda CHAR(10),
    @CodigoVueloVuelta CHAR(10),
    @UsuarioEmpleado VARCHAR(15),
    @CodigoHospedaje VARCHAR(10),
    @CantNoches INT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Estados WHERE Codigo = @CodigoEstadoDestino AND Activo = 1)
        RETURN -1;

    IF NOT EXISTS (SELECT * FROM Vuelos WHERE Codigo = @CodigoVueloIda AND Activo = 1)
        RETURN -2;

    IF NOT EXISTS (SELECT * FROM Vuelos WHERE Codigo = @CodigoVueloVuelta AND Activo = 1)
        RETURN -3;

    IF NOT EXISTS (SELECT * FROM Empleados WHERE Usuario = @UsuarioEmpleado AND Activo = 1)
        RETURN -4;

    IF NOT EXISTS (SELECT * FROM Hospedajes WHERE CodigoInterno = @CodigoHospedaje AND Activo = 1)
        RETURN -5;

    IF @CantNoches <= 0
        RETURN -6;

    IF (SELECT CodigoEstadoLlegada FROM Vuelos WHERE Codigo = @CodigoVueloIda) <> @CodigoEstadoDestino
        RETURN -7;

    IF (SELECT CodigoEstadoPartida FROM Vuelos WHERE Codigo = @CodigoVueloVuelta) <> @CodigoEstadoDestino
        RETURN -8;

    IF (SELECT CodigoEstado FROM Hospedajes WHERE CodigoInterno = @CodigoHospedaje) <> @CodigoEstadoDestino
        RETURN -9;

    DECLARE @FechaIda DATETIME;
    DECLARE @FechaVuelta DATETIME;
    DECLARE @CantDias INT;
    DECLARE @PrecioBase DECIMAL(10,2);
    DECLARE @PrecioIndividual DECIMAL(10,2);
    DECLARE @PrecioDoble DECIMAL(10,2);
    DECLARE @PrecioTriple DECIMAL(10,2);
    DECLARE @CodigoPaquete INT;

    SELECT @FechaIda = FechaHoraPartida
    FROM Vuelos
    WHERE Codigo = @CodigoVueloIda;

    SELECT @FechaVuelta = FechaHoraPartida
    FROM Vuelos
    WHERE Codigo = @CodigoVueloVuelta;

    IF @FechaVuelta <= @FechaIda
        RETURN -10;

    SET @CantDias = DATEDIFF(DAY, @FechaIda, @FechaVuelta);

    SET @PrecioBase =
        (SELECT PrecioPasaje FROM Vuelos WHERE Codigo = @CodigoVueloIda)
        +
        (SELECT PrecioPasaje FROM Vuelos WHERE Codigo = @CodigoVueloVuelta)
        +
        (@CantNoches * (SELECT PrecioNochePersona FROM Hospedajes WHERE CodigoInterno = @CodigoHospedaje));

    SET @PrecioIndividual = @PrecioBase * 1.35;
    SET @PrecioDoble = (@PrecioBase * 2) * 1.10;
    SET @PrecioTriple = (@PrecioBase * 3) * 1.10;

    INSERT INTO Paquetes
    VALUES
    (
        @Titulo,
        @Descripcion,
        @CodigoEstadoDestino,
        @CantDias,
        @PrecioIndividual,
        @PrecioDoble,
        @PrecioTriple,
        @CodigoVueloIda,
        @CodigoVueloVuelta,
        @UsuarioEmpleado,
        1
    );

    SET @CodigoPaquete = SCOPE_IDENTITY();

    INSERT INTO PaqueteHospedaje
    VALUES (@CodigoPaquete, @CodigoHospedaje, @CantNoches);

    RETURN @CodigoPaquete;
END;
GO

CREATE PROCEDURE PaqueteHospedajeAgregar
    @CodigoPaquete INT,
    @CodigoHospedaje VARCHAR(10),
    @CantNoches INT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM Paquetes WHERE Codigo = @CodigoPaquete AND Activo = 1)
        RETURN -1;

    IF NOT EXISTS (SELECT * FROM Hospedajes WHERE CodigoInterno = @CodigoHospedaje AND Activo = 1)
        RETURN -2;

    IF EXISTS (SELECT * FROM PaqueteHospedaje WHERE CodigoPaquete = @CodigoPaquete AND CodigoHospedaje = @CodigoHospedaje)
        RETURN -3;

    IF @CantNoches <= 0
        RETURN -4;

    IF (SELECT CodigoEstado FROM Hospedajes WHERE CodigoInterno = @CodigoHospedaje)
       <>
       (SELECT CodigoEstadoDestino FROM Paquetes WHERE Codigo = @CodigoPaquete)
        RETURN -5;

    INSERT INTO PaqueteHospedaje
    VALUES (@CodigoPaquete, @CodigoHospedaje, @CantNoches);

    DECLARE @PrecioBase DECIMAL(10,2);

    SET @PrecioBase =
        (SELECT V1.PrecioPasaje + V2.PrecioPasaje
         FROM Paquetes P, Vuelos V1, Vuelos V2
         WHERE P.CodigoVueloIda = V1.Codigo
           AND P.CodigoVueloVuelta = V2.Codigo
           AND P.Codigo = @CodigoPaquete)
        +
        (SELECT SUM(PH.CantNoches * H.PrecioNochePersona)
         FROM PaqueteHospedaje PH, Hospedajes H
         WHERE PH.CodigoHospedaje = H.CodigoInterno
           AND PH.CodigoPaquete = @CodigoPaquete);

    UPDATE Paquetes
    SET PrecioIndividual = @PrecioBase * 1.35,
        PrecioDoble = (@PrecioBase * 2) * 1.10,
        PrecioTriple = (@PrecioBase * 3) * 1.10
    WHERE Codigo = @CodigoPaquete;

    RETURN 1;
END;
GO

CREATE PROCEDURE PaqueteBuscar
    @Codigo INT
AS
BEGIN
    SELECT *
    FROM Paquetes
    WHERE Codigo = @Codigo;
END;
GO

CREATE PROCEDURE PaqueteListadoGeneral
    @CodigoEstado VARCHAR(5)
AS
BEGIN
    SELECT Codigo,
           Titulo,
           CodigoEstadoDestino,
           CantDias,
           PrecioIndividual,
           PrecioDoble,
           PrecioTriple,
           Activo
    FROM Paquetes
    WHERE CodigoEstadoDestino LIKE '%' + @CodigoEstado + '%'
    ORDER BY Codigo;
END;
GO

CREATE PROCEDURE PaqueteListadoPorHospedaje
    @CodigoHospedaje VARCHAR(10)
AS
BEGIN
    SELECT P.Codigo,
           P.Titulo,
           V.FechaHoraPartida AS FechaSalida,
           P.CantDias,
           P.PrecioIndividual,
           P.PrecioDoble,
           P.PrecioTriple
    FROM Paquetes P, PaqueteHospedaje PH, Vuelos V
    WHERE P.Codigo = PH.CodigoPaquete
      AND P.CodigoVueloIda = V.Codigo
      AND PH.CodigoHospedaje = @CodigoHospedaje
    ORDER BY V.FechaHoraPartida;
END;
GO

CREATE PROCEDURE PaqueteDetalleHospedajes
    @CodigoPaquete INT
AS
BEGIN
    SELECT H.CodigoInterno,
           H.Nombre,
           H.Direccion,
           H.TipoHospedaje,
           H.PrecioNochePersona,
           PH.CantNoches
    FROM PaqueteHospedaje PH, Hospedajes H
    WHERE PH.CodigoHospedaje = H.CodigoInterno
      AND PH.CodigoPaquete = @CodigoPaquete;
END;
GO
-- DATOS DE PRUEBA

--Empleados
EXEC EmpleadoAlta 'admin01', 'Abc1!', 'Administrador General';
EXEC EmpleadoAlta 'empleado01', 'Via1!', 'Lucía Fernández';
EXEC EmpleadoAlta 'empleado02', 'Sol2@', 'Sofía Martínez';
EXEC EmpleadoAlta 'empleado03', 'Cam3#', 'Camila Pereira';
EXEC EmpleadoAlta 'empleado04', 'Tur4$', 'Rafael Gómez';
EXEC EmpleadoAlta 'empleado05', 'Ana5%', 'Ana Rodríguez';
EXEC EmpleadoAlta 'empleado06', 'Mar6&', 'Marcos Silva';
EXEC EmpleadoAlta 'empleado07', 'Lau7*', 'Laura Méndez';
EXEC EmpleadoAlta 'empleado08', 'Paz8!', 'Paz Suárez';
EXEC EmpleadoAlta 'empleado09', 'Leo9@', 'Leonardo Castro';
GO

-- Estado 
EXEC EstadoAlta 'MVDUY', 'Montevideo', 'Uruguay';
EXEC EstadoAlta 'ALABR', 'Alagoas', 'Brasil';
EXEC EstadoAlta 'QROOM', 'Quintana Roo', 'México';
EXEC EstadoAlta 'RJBR', 'Río de Janeiro', 'Brasil';
EXEC EstadoAlta 'FLUSA', 'Florida', 'Estados Unidos';
EXEC EstadoAlta 'BCARG', 'Bariloche', 'Argentina';
EXEC EstadoAlta 'BAPRO', 'Buenos Aires', 'Argentina';
EXEC EstadoAlta 'SPBR', 'San Pablo', 'Brasil';
EXEC EstadoAlta 'CUSPE', 'Cusco', 'Perú';
EXEC EstadoAlta 'PUNUY', 'Punta del Este', 'Uruguay';
GO

-- Vuelos
EXEC VueloAlta 'ALA260310A', '20260310 07:30:00', 'MVDUY', '20260310 15:45:00', 'ALABR', 1200;
EXEC VueloAlta 'ALA260320B', '20260320 18:00:00', 'ALABR', '20260321 02:15:00', 'MVDUY', 1150;
EXEC VueloAlta 'ALA260410A', '20260410 07:00:00', 'MVDUY', '20260410 15:00:00', 'ALABR', 1250;
EXEC VueloAlta 'ALA260420B', '20260420 19:00:00', 'ALABR', '20260421 03:00:00', 'MVDUY', 1180;

EXEC VueloAlta 'QRO260405A', '20260405 06:00:00', 'MVDUY', '20260405 17:30:00', 'QROOM', 1500;
EXEC VueloAlta 'QRO260415B', '20260415 19:00:00', 'QROOM', '20260416 06:30:00', 'MVDUY', 1450;
EXEC VueloAlta 'QRO260505A', '20260505 06:30:00', 'MVDUY', '20260505 18:00:00', 'QROOM', 1520;
EXEC VueloAlta 'QRO260515B', '20260515 20:00:00', 'QROOM', '20260516 07:30:00', 'MVDUY', 1480;

EXEC VueloAlta 'RIO260502A', '20260502 08:00:00', 'MVDUY', '20260502 12:00:00', 'RJBR', 900;
EXEC VueloAlta 'RIO260509B', '20260509 16:00:00', 'RJBR', '20260509 20:00:00', 'MVDUY', 850;
EXEC VueloAlta 'RIO260602A', '20260602 08:30:00', 'MVDUY', '20260602 12:30:00', 'RJBR', 930;
EXEC VueloAlta 'RIO260609B', '20260609 17:00:00', 'RJBR', '20260609 21:00:00', 'MVDUY', 870;

EXEC VueloAlta 'FLO260610A', '20260610 09:30:00', 'MVDUY', '20260610 22:00:00', 'FLUSA', 1700;
EXEC VueloAlta 'FLO260620B', '20260620 14:00:00', 'FLUSA', '20260621 02:30:00', 'MVDUY', 1650;
EXEC VueloAlta 'FLO260710A', '20260710 09:00:00', 'MVDUY', '20260710 21:30:00', 'FLUSA', 1720;
EXEC VueloAlta 'FLO260720B', '20260720 15:00:00', 'FLUSA', '20260721 03:30:00', 'MVDUY', 1680;

EXEC VueloAlta 'BAR260701A', '20260701 10:00:00', 'MVDUY', '20260701 16:00:00', 'BCARG', 750;
EXEC VueloAlta 'BAR260708B', '20260708 18:00:00', 'BCARG', '20260709 00:00:00', 'MVDUY', 720;
EXEC VueloAlta 'BAR260801A', '20260801 09:30:00', 'MVDUY', '20260801 15:30:00', 'BCARG', 780;
EXEC VueloAlta 'BAR260808B', '20260808 19:00:00', 'BCARG', '20260809 01:00:00', 'MVDUY', 740;

EXEC VueloAlta 'BUE260812A', '20260812 08:00:00', 'MVDUY', '20260812 09:00:00', 'BAPRO', 250;
EXEC VueloAlta 'BUE260816B', '20260816 20:00:00', 'BAPRO', '20260816 21:00:00', 'MVDUY', 240;
EXEC VueloAlta 'SAO260901A', '20260901 07:00:00', 'MVDUY', '20260901 10:30:00', 'SPBR', 600;
EXEC VueloAlta 'SAO260906B', '20260906 18:00:00', 'SPBR', '20260906 21:30:00', 'MVDUY', 580;
EXEC VueloAlta 'CUS260915A', '20260915 05:30:00', 'MVDUY', '20260915 14:30:00', 'CUSPE', 1300;
EXEC VueloAlta 'CUS260925B', '20260925 16:00:00', 'CUSPE', '20260926 01:00:00', 'MVDUY', 1250;
EXEC VueloAlta 'PUN261001A', '20261001 09:00:00', 'MVDUY', '20261001 09:45:00', 'PUNUY', 180;
EXEC VueloAlta 'PUN261005B', '20261005 19:00:00', 'PUNUY', '20261005 19:45:00', 'MVDUY', 170;
GO


--Hospedaje 
EXEC HospedajeAlta 'GOCA', 'Gran Oca', 'Rua Principal 123 - Maragogi', 'All Inclusive', 150, 'ALABR';
EXEC HospedajeAlta 'MARA', 'Maragogi Praia', 'Av. Costera 456 - Maragogi', 'Hotel STD', 95, 'ALABR';
EXEC HospedajeAlta 'LAGOA', 'Lagoa Azul', 'Rua das Flores 88 - Maceió', 'Posada', 110, 'ALABR';

EXEC HospedajeAlta 'CANCUN', 'Sol Cancún', 'Av. Kukulcán 100 - Cancún', 'All Inclusive', 210, 'QROOM';
EXEC HospedajeAlta 'PLAYA', 'Posada Playa Azul', 'Calle Caribe 55 - Playa del Carmen', 'Posada', 130, 'QROOM';
EXEC HospedajeAlta 'TULUM', 'Tulum Garden', 'Calle Luna 25 - Tulum', 'Hotel STD', 145, 'QROOM';

EXEC HospedajeAlta 'RIOHOTEL', 'Rio Vista Hotel', 'Av. Atlántica 300 - Río de Janeiro', 'Hotel STD', 120, 'RJBR';
EXEC HospedajeAlta 'COPA', 'Copacabana Palace', 'Rua Copacabana 10 - Río de Janeiro', 'All Inclusive', 260, 'RJBR';
EXEC HospedajeAlta 'IPANEMA', 'Ipanema Sol', 'Rua Visconde 75 - Río de Janeiro', 'Posada', 140, 'RJBR';

EXEC HospedajeAlta 'ORLANDO', 'Orlando Family Hotel', 'International Drive 200 - Orlando', 'Hotel STD', 180, 'FLUSA';
EXEC HospedajeAlta 'MIAMI', 'Miami Beach Posada', 'Ocean Drive 80 - Miami', 'Posada', 160, 'FLUSA';
EXEC HospedajeAlta 'DISNEY', 'Magic Resort', 'Lake Buena Vista 50 - Orlando', 'All Inclusive', 290, 'FLUSA';

EXEC HospedajeAlta 'ANDES', 'Andes Lodge', 'Av. Bustillo 700 - Bariloche', 'Posada', 90, 'BCARG';
EXEC HospedajeAlta 'LAGO', 'Lago Sur Hotel', 'Calle Mitre 250 - Bariloche', 'Hotel STD', 110, 'BCARG';
EXEC HospedajeAlta 'NIEVE', 'Refugio Nieve', 'Cerro Catedral 45 - Bariloche', 'All Inclusive', 180, 'BCARG';

EXEC HospedajeAlta 'OBELISCO', 'Hotel Obelisco', 'Av. Corrientes 900 - Buenos Aires', 'Hotel STD', 100, 'BAPRO';
EXEC HospedajeAlta 'PALERMO', 'Palermo Suites', 'Calle Thames 123 - Buenos Aires', 'Posada', 115, 'BAPRO';

EXEC HospedajeAlta 'SAMPA', 'Sampa Hotel', 'Av. Paulista 1500 - San Pablo', 'Hotel STD', 130, 'SPBR';
EXEC HospedajeAlta 'JARDINS', 'Jardins Palace', 'Rua Augusta 400 - San Pablo', 'All Inclusive', 210, 'SPBR';

EXEC HospedajeAlta 'INKA', 'Inka Palace', 'Av. El Sol 200 - Cusco', 'Hotel STD', 150, 'CUSPE';
EXEC HospedajeAlta 'CUSCOL', 'Cusco Lodge', 'Calle Plateros 80 - Cusco', 'Posada', 100, 'CUSPE';

EXEC HospedajeAlta 'BRAVA', 'Hotel Brava', 'Rambla Brava 300 - Punta del Este', 'Hotel STD', 190, 'PUNUY';
EXEC HospedajeAlta 'MANSA', 'Posada Mansa', 'Rambla Mansa 120 - Punta del Este', 'Posada', 150, 'PUNUY';
GO

--Paquetes
EXEC PaqueteAlta 'Alagoas en familia', 'Paquete turístico a playas de Alagoas.', 'ALABR', 'ALA260310A', 'ALA260320B', 'admin01', 'GOCA', 7;
EXEC PaqueteAlta 'Alagoas relax', 'Promoción con hospedaje frente al mar.', 'ALABR', 'ALA260410A', 'ALA260420B', 'empleado05', 'MARA', 8;

EXEC PaqueteAlta 'Quintana Roo completo', 'Viaje promocional a Quintana Roo.', 'QROOM', 'QRO260405A', 'QRO260415B', 'empleado01', 'CANCUN', 8;
EXEC PaqueteAlta 'Tulum y playa', 'Viaje con días libres para disfrutar el Caribe.', 'QROOM', 'QRO260505A', 'QRO260515B', 'empleado06', 'TULUM', 7;

EXEC PaqueteAlta 'Río de Janeiro clásico', 'Promoción para conocer Río de Janeiro.', 'RJBR', 'RIO260502A', 'RIO260509B', 'empleado02', 'RIOHOTEL', 6;
EXEC PaqueteAlta 'Río premium', 'Paquete con hospedaje all inclusive en Copacabana.', 'RJBR', 'RIO260602A', 'RIO260609B', 'empleado07', 'COPA', 6;

EXEC PaqueteAlta 'Florida parques y playa', 'Paquete turístico para Florida.', 'FLUSA', 'FLO260610A', 'FLO260620B', 'empleado03', 'ORLANDO', 8;
EXEC PaqueteAlta 'Florida mágico', 'Viaje familiar con parques y descanso.', 'FLUSA', 'FLO260710A', 'FLO260720B', 'empleado08', 'DISNEY', 8;

EXEC PaqueteAlta 'Bariloche invierno', 'Viaje promocional a Bariloche.', 'BCARG', 'BAR260701A', 'BAR260708B', 'empleado04', 'ANDES', 6;
EXEC PaqueteAlta 'Bariloche nieve', 'Promoción de invierno en Cerro Catedral.', 'BCARG', 'BAR260801A', 'BAR260808B', 'empleado09', 'NIEVE', 6;

EXEC PaqueteAlta 'Buenos Aires cultural', 'Escapada cultural a Buenos Aires.', 'BAPRO', 'BUE260812A', 'BUE260816B', 'admin01', 'OBELISCO', 3;
EXEC PaqueteAlta 'San Pablo urbano', 'Viaje corto a San Pablo.', 'SPBR', 'SAO260901A', 'SAO260906B', 'empleado01', 'SAMPA', 4;
EXEC PaqueteAlta 'Cusco histórico', 'Paquete turístico a Cusco.', 'CUSPE', 'CUS260915A', 'CUS260925B', 'empleado02', 'INKA', 8;
EXEC PaqueteAlta 'Punta del Este descanso', 'Escapada a Punta del Este.', 'PUNUY', 'PUN261001A', 'PUN261005B', 'empleado03', 'BRAVA', 3;
GO

--Agregar Hospedaje a Paquetes

EXEC PaqueteHospedajeAgregar 1, 'MARA', 2;
EXEC PaqueteHospedajeAgregar 2, 'LAGOA', 1;
EXEC PaqueteHospedajeAgregar 3, 'PLAYA', 2;
EXEC PaqueteHospedajeAgregar 4, 'CANCUN', 1;
EXEC PaqueteHospedajeAgregar 5, 'IPANEMA', 1;
EXEC PaqueteHospedajeAgregar 6, 'IPANEMA', 1;
EXEC PaqueteHospedajeAgregar 7, 'MIAMI', 2;
EXEC PaqueteHospedajeAgregar 8, 'ORLANDO', 1;
EXEC PaqueteHospedajeAgregar 9, 'LAGO', 1;
EXEC PaqueteHospedajeAgregar 10, 'ANDES', 1;
EXEC PaqueteHospedajeAgregar 11, 'PALERMO', 1;
EXEC PaqueteHospedajeAgregar 12, 'JARDINS', 1;
EXEC PaqueteHospedajeAgregar 13, 'CUSCOL', 2;
EXEC PaqueteHospedajeAgregar 14, 'MANSA', 1;
GO