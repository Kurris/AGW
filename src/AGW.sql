USE [master]
GO
/****** Object:  Database [AGW]    Script Date: 2020/6/3 13:25:59 ******/
CREATE DATABASE [AGW]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AGW', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\AGW.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AGW_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\AGW_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AGW] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AGW].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AGW] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AGW] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AGW] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AGW] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AGW] SET ARITHABORT OFF 
GO
ALTER DATABASE [AGW] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AGW] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AGW] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AGW] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AGW] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AGW] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AGW] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AGW] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AGW] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AGW] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AGW] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AGW] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AGW] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AGW] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AGW] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AGW] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AGW] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AGW] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AGW] SET  MULTI_USER 
GO
ALTER DATABASE [AGW] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AGW] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AGW] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AGW] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AGW] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AGW] SET QUERY_STORE = OFF
GO
USE [AGW]
GO
/****** Object:  Table [dbo].[T_Button]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Button](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fBtnName] [varchar](50) NOT NULL,
	[fBtnText] [varchar](50) NOT NULL,
	[fBtnImage] [binary](50) NULL,
	[fAccess] [nchar](10) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_FormRelationships]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_FormRelationships](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fChildName] [varchar](50) NOT NULL,
	[fFatherName] [varchar](50) NOT NULL,
	[fChildKeys] [varchar](50) NOT NULL,
	[fFatherKeys] [varchar](50) NOT NULL,
	[fMainName] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_InterFace]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_InterFace](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fInterfaceName] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_InterfaceColums]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_InterfaceColums](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fInterFaceColName] [varchar](30) NOT NULL,
	[fInterFaceColIsTree] [bit] NULL,
	[finterfacename] [varchar](30) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_language]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_language](
	[fkey] [varchar](50) NOT NULL,
	[fCns] [nvarchar](max) NOT NULL,
	[fEng] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Navigation]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Navigation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fno] [nvarchar](50) NOT NULL,
	[fId] [int] NULL,
	[fName] [nvarchar](100) NOT NULL,
	[fPid] [int] NULL,
	[fNavType] [int] NULL,
	[fAssembly] [nvarchar](50) NULL,
	[num] [int] NULL,
	[fGroup] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Program]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Program](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fName] [varchar](50) NOT NULL,
	[fSql] [varchar](max) NOT NULL,
	[fTable] [varchar](50) NOT NULL,
	[fProgram] [varchar](50) NOT NULL,
	[fCNName] [nvarchar](30) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_ProgramInfo]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ProgramInfo](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fProgramName] [varchar](50) NOT NULL,
	[fField] [varchar](20) NOT NULL,
	[fDefaultValue] [varchar](50) NULL,
	[fVisiable] [bit] NOT NULL,
	[fEnable] [bit] NOT NULL,
	[fLength] [varchar](50) NULL,
	[fEmpty] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_ToolStrip]    Script Date: 2020/6/3 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ToolStrip](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fToolName] [varchar](30) NOT NULL,
	[fInterFaceName] [varchar](30) NOT NULL,
	[fCustomName] [nvarchar](15) NULL,
	[fAssamblyName] [varchar](30) NULL,
	[fRemoteEXEName] [varchar](30) NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[T_Button] ON 

INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fBtnImage], [fAccess]) VALUES (1, N'add', N'新增', NULL, N'add       ')
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fBtnImage], [fAccess]) VALUES (2, N'edit', N'编辑', NULL, N'edit      ')
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fBtnImage], [fAccess]) VALUES (3, N'delete', N'删除', NULL, N'delete    ')
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fBtnImage], [fAccess]) VALUES (4, N'search', N'查找', NULL, N'search    ')
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fBtnImage], [fAccess]) VALUES (5, N'print', N'打印', NULL, N'print     ')
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fBtnImage], [fAccess]) VALUES (6, N'export', N'导出', NULL, N'export    ')
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fBtnImage], [fAccess]) VALUES (7, N'import', N'导入', NULL, N'import    ')
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fBtnImage], [fAccess]) VALUES (8, N'custom', N'自定义', NULL, N'custom    ')
SET IDENTITY_INSERT [dbo].[T_Button] OFF
GO
SET IDENTITY_INSERT [dbo].[T_FormRelationships] ON 

INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (1, N'av_programinfo', N'av_datasource', N'fprogramname', N'fname', N'av_datasource')
INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (2, N'av_toolstrip', N'av_interface', N'finterfacename', N'finterfacename', N'av_interface')
INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (3, N'av_formrelationships', N'av_interface', N'fmainname', N'finterfacename', N'av_interface')
INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (4, N'av_InterfaceColums', N'av_interface', N'finterfacename', N'finterfacename', N'av_interface')
SET IDENTITY_INSERT [dbo].[T_FormRelationships] OFF
GO
SET IDENTITY_INSERT [dbo].[T_InterFace] ON 

INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (1, N'av_datasource')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (2, N'av_interface')
SET IDENTITY_INSERT [dbo].[T_InterFace] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Navigation] ON 

INSERT [dbo].[T_Navigation] ([id], [fno], [fId], [fName], [fPid], [fNavType], [fAssembly], [num], [fGroup]) VALUES (1, N'SystemSetting', 1, N'系统设置', -1, 1, NULL, 1, N'SystemSetting')
INSERT [dbo].[T_Navigation] ([id], [fno], [fId], [fName], [fPid], [fNavType], [fAssembly], [num], [fGroup]) VALUES (2, N'ProgramSetting', 2, N'程序设置', 0, NULL, NULL, 1, N'SystemSetting')
INSERT [dbo].[T_Navigation] ([id], [fno], [fId], [fName], [fPid], [fNavType], [fAssembly], [num], [fGroup]) VALUES (3, N'DataSource', 3, N'数据源', 2, NULL, N'av_datasource', 1, N'SystemSetting')
INSERT [dbo].[T_Navigation] ([id], [fno], [fId], [fName], [fPid], [fNavType], [fAssembly], [num], [fGroup]) VALUES (4, N'XXXX', 4, N'xxxxx', -1, 1, NULL, NULL, N'XXXX')
INSERT [dbo].[T_Navigation] ([id], [fno], [fId], [fName], [fPid], [fNavType], [fAssembly], [num], [fGroup]) VALUES (5, N'Interface', 5, N'程序界面', 2, NULL, N'av_interface', 2, N'SystemSetting')
INSERT [dbo].[T_Navigation] ([id], [fno], [fId], [fName], [fPid], [fNavType], [fAssembly], [num], [fGroup]) VALUES (6, N'Button', 6, N'按钮', 2, NULL, N'av_button', 3, N'SystemSetting')
INSERT [dbo].[T_Navigation] ([id], [fno], [fId], [fName], [fPid], [fNavType], [fAssembly], [num], [fGroup]) VALUES (7, N'Language', 7, N'多语言', 2, NULL, N'av_language', 4, NULL)
SET IDENTITY_INSERT [dbo].[T_Navigation] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Program] ON 

INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (1, N'av_datasource', N'select * from [T_Program] with(nolock)', N'T_Program', N'frmBase', N'数据源')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (2, N'av_interface', N'select * from [t_interface] with(nolock)', N't_interface', N'frmBase', N'程序界面')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (3, N'av_toolstrip', N'select * from t_toolstrip', N't_toolstrip', N'frmbase', N'工具栏')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (4, N'av_formrelationships', N'select * from t_formrelationships', N't_formrelationships', N'frmbase', N'程序从属Grid')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (5, N'av_programinfo', N'select * from t_programinfo', N't_programinfo', N'frmbase', N'程序信息')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (6, N'av_button', N'select * from t_button', N't_button', N'frmbase', N'按钮')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (7, N'av_InterfaceColums', N'select * from T_InterfaceColums', N'T_InterfaceColums', N'frmbase', N'界面字段')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (8, N'av_language', N'select * from t_language', N't_language', N'frmbase', N'多语言')
SET IDENTITY_INSERT [dbo].[T_Program] OFF
GO
SET IDENTITY_INSERT [dbo].[T_ToolStrip] ON 

INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (1, N'add', N'av_datasource', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[T_ToolStrip] OFF
GO
ALTER TABLE [dbo].[T_Program] ADD  DEFAULT ('') FOR [fCNName]
GO
USE [master]
GO
ALTER DATABASE [AGW] SET  READ_WRITE 
GO
