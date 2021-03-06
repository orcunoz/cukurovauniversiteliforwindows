USE [master]
GO
/****** Object:  Database [CukurovaUniversiteli]    Script Date: 7.11.2016 01:01:32 ******/
CREATE DATABASE [CukurovaUniversiteli]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CukurovaUniversiteli', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\CukurovaUniversiteli.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CukurovaUniversiteli_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\CukurovaUniversiteli_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CukurovaUniversiteli].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CukurovaUniversiteli] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET ARITHABORT OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CukurovaUniversiteli] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CukurovaUniversiteli] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CukurovaUniversiteli] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CukurovaUniversiteli] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET RECOVERY FULL 
GO
ALTER DATABASE [CukurovaUniversiteli] SET  MULTI_USER 
GO
ALTER DATABASE [CukurovaUniversiteli] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CukurovaUniversiteli] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CukurovaUniversiteli] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CukurovaUniversiteli] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [CukurovaUniversiteli] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CukurovaUniversiteli', N'ON'
GO
ALTER DATABASE [CukurovaUniversiteli] SET QUERY_STORE = OFF
GO
USE [CukurovaUniversiteli]
GO
/****** Object:  User [NT AUTHORITY\NETWORK SERVICE]    Script Date: 7.11.2016 01:01:32 ******/
CREATE USER [NT AUTHORITY\NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [DESKTOP-I2E1FTP\PdwControlNodeAccess]    Script Date: 7.11.2016 01:01:32 ******/
CREATE USER [DESKTOP-I2E1FTP\PdwControlNodeAccess] FOR LOGIN [DESKTOP-I2E1FTP\PdwControlNodeAccess]
GO
/****** Object:  User [DESKTOP-I2E1FTP\PdwComputeNodeAccess]    Script Date: 7.11.2016 01:01:32 ******/
CREATE USER [DESKTOP-I2E1FTP\PdwComputeNodeAccess] FOR LOGIN [DESKTOP-I2E1FTP\PdwComputeNodeAccess]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE]
GO
ALTER ROLE [db_datareader] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE]
GO
ALTER ROLE [db_datareader] ADD MEMBER [DESKTOP-I2E1FTP\PdwControlNodeAccess]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [DESKTOP-I2E1FTP\PdwControlNodeAccess]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [DESKTOP-I2E1FTP\PdwComputeNodeAccess]
GO
ALTER ROLE [db_datareader] ADD MEMBER [DESKTOP-I2E1FTP\PdwComputeNodeAccess]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [DESKTOP-I2E1FTP\PdwComputeNodeAccess]
GO
/****** Object:  Schema [pdw]    Script Date: 7.11.2016 01:01:32 ******/
CREATE SCHEMA [pdw]
GO
/****** Object:  Schema [QTables]    Script Date: 7.11.2016 01:01:32 ******/
CREATE SCHEMA [QTables]
GO
/****** Object:  Table [dbo].[Food]    Script Date: 7.11.2016 01:01:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food](
	[date] [date] NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[calorie] [int] NOT NULL,
	[contents] [text] NOT NULL,
	[image_src] [nvarchar](120) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[food_procedure]    Script Date: 7.11.2016 01:01:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[food_procedure]

@process_type int,
@date date = null,
@name nvarchar(50) = null,
@calorie int = null,
@contents text = null,
@image_src nvarchar(120) = null

AS BEGIN 

	if (@process_type = 0) 
	BEGIN
		SELECT * FROM Food WHERE [date] >= @date ORDER BY [date]
	END

	if (@process_type = 1) 
	BEGIN
		INSERT INTO Food VALUES (@date, @name, @calorie, @contents, @image_src)
	END

	if (@process_type = 2)
	BEGIN
		TRUNCATE TABLE Food
	END

	if (@process_type = 3)
	BEGIN
		SELECT COUNT(*) FROM Food
	END

END
GO
/****** Object:  StoredProcedure [pdw].[instpdw]    Script Date: 7.11.2016 01:01:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [pdw].[instpdw]
    @DatabaseName NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Sql nvarchar(max);
    SET @Sql = 'USE ' + QUOTENAME(@DatabaseName) + ';    
    PRINT ''Create schema pdw...''
    IF (SCHEMA_ID(''pdw'') IS NULL)
    BEGIN
      DECLARE @sql nvarchar(128)
      SET @sql = ''CREATE SCHEMA pdw''
      EXEC sp_executesql @sql
    END'
	EXEC sp_executesql @Sql;

    SET @Sql = 'USE ' + QUOTENAME(@DatabaseName) + ';
    PRINT ''Create schema QTables...''
    IF (SCHEMA_ID(''QTables'') IS NULL)
    BEGIN
      DECLARE @sql nvarchar(128)
      SET @sql = ''CREATE SCHEMA QTables''
      EXEC sp_executesql @sql
    END'
	EXEC sp_executesql @Sql

    EXECUTE sp_executesql @Sql
END

GO
USE [master]
GO
ALTER DATABASE [CukurovaUniversiteli] SET  READ_WRITE 
GO
