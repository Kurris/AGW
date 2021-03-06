USE [AGW]
GO
/****** Object:  Table [dbo].[T_Button]    Script Date: 2020/9/15 20:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Button](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fBtnName] [varchar](50) NOT NULL,
	[fBtnText] [varchar](50) NOT NULL,
	[fAccess] [nchar](10) NOT NULL,
	[fBtnImage] [nvarchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_ColumnShowType]    Script Date: 2020/9/15 20:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ColumnShowType](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fShowTypeName] [nvarchar](50) NOT NULL,
	[fShowTypeValue] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_FormRelationships]    Script Date: 2020/9/15 20:47:38 ******/
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
/****** Object:  Table [dbo].[T_InterFace]    Script Date: 2020/9/15 20:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_InterFace](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fInterfaceName] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_InterfaceColums]    Script Date: 2020/9/15 20:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_InterfaceColums](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fColName] [varchar](30) NOT NULL,
	[fInterFaceColIsTree] [bit] NULL,
	[fInterfacename] [varchar](30) NOT NULL,
	[fEmpty] [bit] NULL,
	[fDefaultValue] [nvarchar](50) NULL,
	[fKey] [bit] NULL,
	[fColType] [nvarchar](50) NULL,
	[fDataSource] [varchar](50) NULL,
	[fDataSourceCols] [varchar](50) NULL,
	[fDataMapCols] [varchar](50) NULL,
	[fNum] [int] NULL,
	[fVisiable] [bit] NULL,
	[fReadOnly] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_language]    Script Date: 2020/9/15 20:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_language](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fKey] [varchar](50) NOT NULL,
	[fCns] [nvarchar](100) NOT NULL,
	[fEng] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[fId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Navigation]    Script Date: 2020/9/15 20:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Navigation](
	[fid] [int] IDENTITY(1,1) NOT NULL,
	[fno] [nvarchar](50) NOT NULL,
	[fCid] [int] NULL,
	[fName] [nvarchar](100) NOT NULL,
	[fPid] [int] NULL,
	[fNavType] [int] NULL,
	[fAssembly] [nvarchar](50) NULL,
	[fGroup] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[fid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Program]    Script Date: 2020/9/15 20:47:38 ******/
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
/****** Object:  Table [dbo].[T_ToolStrip]    Script Date: 2020/9/15 20:47:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ToolStrip](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fToolName] [varchar](30) NOT NULL,
	[fInterFaceName] [varchar](30) NOT NULL,
	[fCustomName] [nvarchar](15) NULL,
	[fAssamblyName] [varchar](200) NULL,
	[fRemoteEXEName] [varchar](30) NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[T_Button] ON 

INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (1, N'add', N'新增', N'add       ', N'f:\630.png')
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (2, N'edit', N'编辑', N'edit      ', NULL)
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (3, N'delete', N'删除', N'delete    ', NULL)
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (4, N'search', N'查找', N'search    ', NULL)
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (5, N'print', N'打印', N'print     ', NULL)
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (6, N'export', N'导出', N'export    ', NULL)
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (7, N'import', N'导入', N'import    ', NULL)
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (8, N'custom', N'自定义', N'custom    ', NULL)
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (9, N'refresh', N'刷新', N'refresh   ', NULL)
INSERT [dbo].[T_Button] ([fId], [fBtnName], [fBtnText], [fAccess], [fBtnImage]) VALUES (10, N'XXX', N'XXX', N'XXX       ', NULL)
SET IDENTITY_INSERT [dbo].[T_Button] OFF
GO
SET IDENTITY_INSERT [dbo].[T_ColumnShowType] ON 

INSERT [dbo].[T_ColumnShowType] ([fId], [fShowTypeName], [fShowTypeValue]) VALUES (1, N'下拉框', N'combobox')
SET IDENTITY_INSERT [dbo].[T_ColumnShowType] OFF
GO
SET IDENTITY_INSERT [dbo].[T_FormRelationships] ON 

INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (2, N'av_toolstrip', N'av_interface', N'finterfacename', N'finterfacename', N'av_interface')
INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (3, N'av_formrelationships', N'av_interface', N'fmainname', N'finterfacename', N'av_interface')
INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (4, N'av_InterfaceColums', N'av_interface', N'finterfacename', N'finterfacename', N'av_interface')
INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (19, N'av_menunodes', N'av_menu', N'fgroup', N'fgroup', N'av_menu')
INSERT [dbo].[T_FormRelationships] ([fId], [fChildName], [fFatherName], [fChildKeys], [fFatherKeys], [fMainName]) VALUES (20, N'av_menuprogram', N'av_menunodes', N'fpid,fgroup', N'fid,fgroup', N'av_menu')
SET IDENTITY_INSERT [dbo].[T_FormRelationships] OFF
GO
SET IDENTITY_INSERT [dbo].[T_InterFace] ON 

INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (1, N'av_datasource')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (2, N'av_interface')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (3, N'av_language')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (4, N'av_formrelationships')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (5, N'av_Navigation')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (7, N'av_toolstrip')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (8, N'av_menu')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (9, N'av_menunodes')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (10, N'av_menuprogram')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (11, N'av_button')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (12, N'av_InterfaceColums')
INSERT [dbo].[T_InterFace] ([fId], [fInterfaceName]) VALUES (14, N'av_columnshowtype')
SET IDENTITY_INSERT [dbo].[T_InterFace] OFF
GO
SET IDENTITY_INSERT [dbo].[T_InterfaceColums] ON 

INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (75, N'fId', 0, N'av_formrelationships', 0, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (280, N'fInterfaceName', 0, N'av_interface', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (213, N'fSql', 0, N'av_datasource', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (214, N'fTable', 1, N'av_datasource', 0, N'', 0, N'', N'', N'', N'', 2, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (215, N'fProgram', 0, N'av_datasource', 0, N'', 0, N'', N'', N'', N'', 1, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (216, N'fCNName', 0, N'av_datasource', 0, N'', 0, N'', N'', N'', N'', 4, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (217, N'fno', 0, N'av_menuprogram', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (218, N'fCid', 0, N'av_menuprogram', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (301, N'fId', 0, N'av_InterfaceColums', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (219, N'fName', 0, N'av_menuprogram', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (220, N'fPid', 0, N'av_menuprogram', 1, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (221, N'fNavType', 0, N'av_menuprogram', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (222, N'fAssembly', 0, N'av_menuprogram', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (302, N'fInterFaceColIsTree', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (224, N'fGroup', 0, N'av_menuprogram', 1, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (225, N'fno', 0, N'av_menunodes', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (226, N'fCid', 0, N'av_menunodes', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (123, N'fId', 0, N'av_menunodes', 1, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (227, N'fName', 1, N'av_menunodes', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (228, N'fPid', 0, N'av_menunodes', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (229, N'fNavType', 0, N'av_menunodes', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (230, N'fAssembly', 0, N'av_menunodes', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (313, N'fInterfacename', 0, N'av_InterfaceColums', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (232, N'fGroup', 0, N'av_menunodes', 1, N'', 0, N'', N'', N'', N'', 0, 1, 1)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (314, N'fEmpty', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (315, N'fDefaultValue', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (132, N'fId', 0, N'av_menuprogram', 1, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (316, N'fKey', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (317, N'fColType', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (318, N'fDataSource', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (319, N'fDataSourceCols', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (320, N'fDataMapCols', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (321, N'fNum', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (241, N'fno', 0, N'av_navigation', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (242, N'fCid', 1, N'av_navigation', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (243, N'fName', 0, N'av_navigation', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (244, N'fPid', 0, N'av_navigation', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (245, N'fNavType', 0, N'av_navigation', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (246, N'fAssembly', 0, N'av_navigation', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (322, N'fVisiable', 0, N'av_InterfaceColums', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (248, N'fGroup', 0, N'av_navigation', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (249, N'fKey', 0, N'av_language', 0, N'', 0, N'', N'', N'', N'', 1, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (250, N'fCns', 0, N'av_language', 0, N'', 0, N'', N'', N'', N'', 2, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (251, N'fEng', 0, N'av_language', 1, N'', 0, N'', N'', N'', N'', 3, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (252, N'fColName', 0, N'av_InterfaceColums', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (264, N'fReadOnly', 0, N'av_InterfaceColums', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (265, N'fBtnName', 0, N'av_button', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (266, N'fBtnText', 0, N'av_button', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (267, N'fAccess', 0, N'av_button', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (268, N'fBtnImage', 0, N'av_button', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (269, N'fChildName', 0, N'av_formrelationships', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (270, N'fFatherName', 0, N'av_formrelationships', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (271, N'fChildKeys', 0, N'av_formrelationships', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (272, N'fFatherKeys', 0, N'av_formrelationships', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (273, N'fMainName', 0, N'av_formrelationships', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (274, N'fToolName', 0, N'av_toolstrip', 0, N'', 0, N'', N'', N'', N'', 1, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (275, N'fInterFaceName', 0, N'av_toolstrip', 0, N'', 0, N'', N'', N'', N'', 5, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (276, N'fCustomName', 0, N'av_toolstrip', 1, N'', 0, N'', N'', N'', N'', 2, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (277, N'fAssamblyName', 0, N'av_toolstrip', 1, N'', 0, N'', N'', N'', N'', 3, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (278, N'fRemoteEXEName', 0, N'av_toolstrip', 1, N'', 0, N'', N'', N'', N'', 4, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (281, N'fName', 0, N'av_datasource', 0, N'', 0, N'', N'', N'', N'', 3, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (81, N'fId', 0, N'av_button', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (105, N'fId', 0, N'av_navigation', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (58, N'fId', 0, N'av_datasource', 0, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (64, N'fId', 0, N'av_columnshowtype', 0, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (69, N'fId', 0, N'av_toolstrip', 0, N'', 0, N'', N'', N'', N'', -1, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (99, N'fId', 0, N'av_language', 0, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (210, N'fShowTypeName', 0, N'av_columnshowtype', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (211, N'fShowTypeValue', 0, N'av_columnshowtype', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (67, N'fId', 0, N'av_interface', 0, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (293, N'fid', 0, N'av_Menu', 0, N'', 0, N'', N'', N'', N'', 0, 0, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (294, N'fno', 0, N'av_Menu', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (295, N'fCid', 0, N'av_Menu', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (296, N'fName', 0, N'av_Menu', 0, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (297, N'fPid', 0, N'av_Menu', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (298, N'fNavType', 0, N'av_Menu', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (299, N'fAssembly', 0, N'av_Menu', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
INSERT [dbo].[T_InterfaceColums] ([fId], [fColName], [fInterFaceColIsTree], [fInterfacename], [fEmpty], [fDefaultValue], [fKey], [fColType], [fDataSource], [fDataSourceCols], [fDataMapCols], [fNum], [fVisiable], [fReadOnly]) VALUES (300, N'fGroup', 0, N'av_Menu', 1, N'', 0, N'', N'', N'', N'', 0, 1, 0)
SET IDENTITY_INSERT [dbo].[T_InterfaceColums] OFF
GO
SET IDENTITY_INSERT [dbo].[T_language] ON 

INSERT [dbo].[T_language] ([fId], [fKey], [fCns], [fEng]) VALUES (2, N'fColName', N'列名', N'ColumnName')
INSERT [dbo].[T_language] ([fId], [fKey], [fCns], [fEng]) VALUES (3, N'fId', N'自增ID', N'ID')
INSERT [dbo].[T_language] ([fId], [fKey], [fCns], [fEng]) VALUES (4, N'fEmpty', N'是否为空', N'IsEmpty')
INSERT [dbo].[T_language] ([fId], [fKey], [fCns], [fEng]) VALUES (5, N'fInterFaceColIsTree', N'是否为树', N'')
INSERT [dbo].[T_language] ([fId], [fKey], [fCns], [fEng]) VALUES (6, N'fno', N'代号', N'NO')
INSERT [dbo].[T_language] ([fId], [fKey], [fCns], [fEng]) VALUES (7, N'finterfacename', N'界面名称', N'InterFaceName')
INSERT [dbo].[T_language] ([fId], [fKey], [fCns], [fEng]) VALUES (8, N'ftable', N'表名称', N'TableName')
SET IDENTITY_INSERT [dbo].[T_language] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Navigation] ON 

INSERT [dbo].[T_Navigation] ([fid], [fno], [fCid], [fName], [fPid], [fNavType], [fAssembly], [fGroup]) VALUES (1, N'SystemSetting', 1, N'系统设置', -1, 1, NULL, N'SystemSetting')
INSERT [dbo].[T_Navigation] ([fid], [fno], [fCid], [fName], [fPid], [fNavType], [fAssembly], [fGroup]) VALUES (2, N'ProgramSetting', 2, N'程序设置', 0, NULL, NULL, N'SystemSetting')
INSERT [dbo].[T_Navigation] ([fid], [fno], [fCid], [fName], [fPid], [fNavType], [fAssembly], [fGroup]) VALUES (3, N'DataSource', 3, N'数据源', 2, NULL, N'av_datasource', N'SystemSetting')
INSERT [dbo].[T_Navigation] ([fid], [fno], [fCid], [fName], [fPid], [fNavType], [fAssembly], [fGroup]) VALUES (5, N'Interface', 5, N'程序界面', 2, 0, N'av_interface', N'SystemSetting')
INSERT [dbo].[T_Navigation] ([fid], [fno], [fCid], [fName], [fPid], [fNavType], [fAssembly], [fGroup]) VALUES (6, N'Button', 6, N'按钮', 2, NULL, N'av_button', N'SystemSetting')
INSERT [dbo].[T_Navigation] ([fid], [fno], [fCid], [fName], [fPid], [fNavType], [fAssembly], [fGroup]) VALUES (7, N'Language', 7, N'多语言', 2, NULL, N'av_language', N'SystemSetting')
INSERT [dbo].[T_Navigation] ([fid], [fno], [fCid], [fName], [fPid], [fNavType], [fAssembly], [fGroup]) VALUES (8, N'Menu', 8, N'程序菜单', 2, NULL, N'av_Menu', N'SystemSetting')
INSERT [dbo].[T_Navigation] ([fid], [fno], [fCid], [fName], [fPid], [fNavType], [fAssembly], [fGroup]) VALUES (10, N'ColumnShowType', 9, N'列类型', 2, 0, N'av_columnshowtype', N'SystemSetting')
SET IDENTITY_INSERT [dbo].[T_Navigation] OFF
GO
SET IDENTITY_INSERT [dbo].[T_Program] ON 

INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (1, N'av_datasource', N'select * from [T_Program] with(nolock)', N'T_Program', N'frmBase', N'数据源')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (2, N'av_interface', N'select * from [t_interface] with(nolock)', N't_interface', N'frmBase', N'程序界面')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (3, N'av_toolstrip', N'select * from t_toolstrip', N't_toolstrip', N'frmbase', N'工具栏')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (4, N'av_formrelationships', N'select * from t_formrelationships', N't_formrelationships', N'frmbase', N'程序从属Grid')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (6, N'av_button', N'select * from t_button', N't_button', N'frmbase', N'按钮')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (7, N'av_InterfaceColums', N'select * from T_InterfaceColums', N'T_InterfaceColums', N'frmbase', N'界面字段')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (16, N'av_language', N'select * from t_language', N'T_language', N'frmBase', N'多语言')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (15, N'av_navigation', N'select * from T_Navigation', N'T_Navigation', N'frmBase', N'程序菜单')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (17, N'av_Menu', N'select * from T_Navigation  where fnavtype =1', N'T_Navigation', N'frmBase', N'程序主菜单')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (18, N'av_menunodes', N'select * from T_Navigation  where fPid=0 and fNavType is null', N'T_Navigation', N'frmBase', N'菜单节点')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (19, N'av_menuprogram', N'select * from T_Navigation where fPid>0 ', N'T_Navigation', N'frmBase', N'程序')
INSERT [dbo].[T_Program] ([fId], [fName], [fSql], [fTable], [fProgram], [fCNName]) VALUES (20, N'av_columnshowtype', N'select * from T_ColumnShowType with(nolock)', N'T_ColumnShowType', N'frmBase', N'列类型')
SET IDENTITY_INSERT [dbo].[T_Program] OFF
GO
SET IDENTITY_INSERT [dbo].[T_ToolStrip] ON 

INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (1, N'add', N'av_datasource', NULL, NULL, NULL)
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (3, N'refresh', N'av_datasource', NULL, NULL, NULL)
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (4, N'refresh', N'av_button', NULL, NULL, NULL)
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (5, N'refresh', N'av_language', NULL, NULL, NULL)
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (6, N'refresh', N'av_interface', NULL, NULL, NULL)
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (8, N'delete', N'av_datasource', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (46, N'delete', N'av_columnshowtype', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (10, N'delete', N'av_navigation', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (11, N'add', N'av_navigation', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (12, N'add', N'av_formrelationships', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (13, N'delete', N'av_formrelationships', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (33, N'add', N'av_language', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (17, N'edit', N'av_datasource', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (21, N'custom', N'av_datasource', N'更新数据', N'Button.UpdateColumnInfo,UpdateColumnInfo', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (28, N'delete', N'av_menu', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (29, N'edit', N'av_menu', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (30, N'edit', N'av_button', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (35, N'delete', N'av_interface', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (36, N'delete', N'av_InterfaceColums', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (37, N'edit', N'av_language', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (38, N'edit', N'av_InterfaceColums', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (44, N'refresh', N'av_columnshowtype', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (45, N'edit', N'av_menuprogram', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (7, N'add', N'av_toolstrip', NULL, NULL, NULL)
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (40, N'edit', N'av_toolstrip', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (47, N'delete', N'av_toolstrip', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (22, N'add', N'av_menu', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (26, N'add', N'av_menunodes', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (27, N'add', N'av_menuprogram', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (31, N'add', N'av_button', N'', N'', N'')
INSERT [dbo].[T_ToolStrip] ([fId], [fToolName], [fInterFaceName], [fCustomName], [fAssamblyName], [fRemoteEXEName]) VALUES (41, N'add', N'av_columnshowtype', N'', N'', N'')
SET IDENTITY_INSERT [dbo].[T_ToolStrip] OFF
GO
ALTER TABLE [dbo].[T_InterfaceColums] ADD  DEFAULT ((0)) FOR [fReadOnly]
GO
ALTER TABLE [dbo].[T_Program] ADD  DEFAULT ('') FOR [fCNName]
GO
