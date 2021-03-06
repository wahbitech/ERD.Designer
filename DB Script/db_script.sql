USE [master]
GO
/****** Object:  Database [DrawerDb]    Script Date: 29/10/2016 5:26:32 PM ******/
CREATE DATABASE [DrawerDb]
GO
USE [DrawerDb]
GO
/****** Object:  Table [dbo].[SavedDiagram]    Script Date: 29/10/2016 5:26:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SavedDiagram](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_SavedDiagram] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAttribute]    Script Date: 29/10/2016 5:26:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAttribute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[GivenId] [nvarchar](200) NULL,
	[ParentEntityId] [int] NOT NULL,
	[DataType] [nvarchar](50) NULL,
	[Length] [int] NULL,
	[IsKey] [bit] NOT NULL,
	[AttributeType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblAttribute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCompositeChildAttribute]    Script Date: 29/10/2016 5:26:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCompositeChildAttribute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[GivenId] [nvarchar](200) NULL,
	[ParentAttributeId] [int] NOT NULL,
	[DataType] [nvarchar](50) NOT NULL,
	[Length] [int] NULL,
	[IsKey] [bit] NOT NULL,
 CONSTRAINT [PK_tblCompositeChildAttribute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEntity]    Script Date: 29/10/2016 5:26:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEntity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[DiagramId] [int] NOT NULL,
 CONSTRAINT [PK_tblEntity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblRelationship]    Script Date: 29/10/2016 5:26:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRelationship](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[PKId] [int] NOT NULL,
	[FKId] [int] NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[diagramId] [int] NOT NULL,
	[reltype] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblRelationship] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[SavedDiagram] ON 

GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1, N'D01')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (2, N'D002')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1002, N'Dg001')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1003, N'DiagramLast')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1004, N'CompositeDiagram')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1005, N'CompositeDiagram2')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1006, N'CompositeDiagram3')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1007, N'CompositeDiagram4')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1008, N'CompositeDiagram5')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1009, N'CompositeDiagram6')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1010, N'CompositeDiagram7')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1011, N'CompositeDiagram8')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1012, N'CompositeDiagram9')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1013, N'CompositeDiagram10')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1014, N'CompositeDiagram11')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1015, N'CompositeDiagram12')
GO
INSERT [dbo].[SavedDiagram] ([Id], [Name]) VALUES (1018, N'CompositeDiagram13')


GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1, N'Id', N'Student_Id', 1, N'Int', 0, 1, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (2, N'Id', N'Teacher_Id', 2, N'Int', 0, 1, N'derived')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (3, N'Phone', N'Teacher_Phone', 2, N'Nvarchar', 50, 0, N'multi')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1002, N'Name', N'Student_Name', 2, N'Nvarchar', 50, 0, N'multi')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1003, N'id', N'Student_id', 1003, N'Int', 0, 1, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1004, N'SchoolId', N'Student_SchoolId', 1003, N'Int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1005, N'ClassId', N'Student_ClassId', 1003, N'Int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1006, N'id', N'school_id', 1004, N'Int', 0, 1, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1007, N'id', N'class_id', 1005, N'Int', 0, 1, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1008, N'id', N'Student_id', 1006, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1009, N'address', N'Student_address', 1006, N'', 0, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1010, N'id', N'Student_id', 1007, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1011, N'id', N'Student_id', 1008, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1012, N'id', N'Student_id', 1009, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1013, N'id', N'Student_id', 1010, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1014, N'id', N'Student_id', 1011, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1015, N'id', N'Student_id', 1012, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1016, N'address', N'Student_address', 1012, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1017, N'id', N'Student_id', 1013, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1018, N'address', N'Student_address', 1013, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1019, N'id', N'Student_id', 1014, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1020, N'address', N'Student_address', 1014, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1021, N'id', N'Student_id', 1015, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1022, N'address', N'Student_address', 1015, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1023, N'id', N'Student_id', 1016, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1024, N'address', N'Student_address', 1016, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1025, N'id', N'Student_id', 1017, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1026, N'address', N'Student_address', 1017, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1027, N'id', N'Student_id', 1018, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1028, N'address', N'Student_address', 1018, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1029, N'id', N'Student_id', 1019, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1030, N'address', N'Student_address', 1019, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1031, N'Name', N'Student_Name', 1019, N'Nvarchar', 50, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1032, N'id', N'School_id', 1020, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1033, N'SName', N'School_SName', 1020, N'Nvarchar', 50, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1034, N'id', N'Student_id', 1021, N'int', 0, 0, N'normal')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1035, N'address', N'Student_address', 1021, NULL, NULL, 0, N'composite')
GO
INSERT [dbo].[tblAttribute] ([Id], [Name], [GivenId], [ParentEntityId], [DataType], [Length], [IsKey], [AttributeType]) VALUES (1036, N'Name', N'Student_Name', 1021, N'Nvarchar', 50, 0, N'normal')


GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (1, N'city', N'Student_address_city', 1010, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (2, N'country', N'Student_address_country', 1010, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (3, N'city', N'Student_address_city', 1016, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (4, N'country', N'Student_address_country', 1016, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (5, N'city', N'Student_address_city', 1013, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (6, N'country', N'Student_address_country', 1013, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (7, N'city', N'Student_address_city', 1017, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (8, N'country', N'Student_address_country', 1017, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (9, N'city', N'Student_address_city', 1028, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (10, N'city', N'Student_address_city', 1030, N'Nvarchar', 50, 0)
GO
INSERT [dbo].[tblCompositeChildAttribute] ([Id], [Name], [GivenId], [ParentAttributeId], [DataType], [Length], [IsKey]) VALUES (11, N'city', N'Student_address_city', 1035, N'Nvarchar', 50, 0)



GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1, N'Student', N'normal', 1)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (2, N'Teacher', N'normal', 1)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (3, N'Student', N'normal', 2)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1002, N'Student', N'normal', 1002)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1003, N'Student', N'normal', 1003)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1004, N'school', N'normal', 1003)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1005, N'class', N'normal', 1003)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1006, N'Student', N'normal', 1004)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1007, N'Student', N'normal', 1005)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1008, N'Student', N'normal', 1006)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1009, N'Student', N'normal', 1007)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1010, N'Student', N'normal', 1008)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1011, N'Student', N'normal', 1009)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1012, N'Student', N'normal', 1010)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1013, N'Student', N'normal', 1011)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1014, N'Student', N'normal', 1012)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1015, N'Student', N'normal', 1013)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1016, N'Student', N'normal', 1014)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1017, N'Student', N'normal', 1015)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1018, N'Student', N'normal', 1016)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1019, N'Student', N'normal', 1017)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1020, N'School', N'normal', 1017)
GO
INSERT [dbo].[tblEntity] ([Id], [Name], [Type], [DiagramId]) VALUES (1021, N'Student', N'normal', 1018)

GO
INSERT [dbo].[tblRelationship] ([Id], [Name], [PKId], [FKId], [type], [diagramId], [reltype]) VALUES (1, N'rel1', 1, 2, N'One-To-Many', 1, NULL)
GO
INSERT [dbo].[tblRelationship] ([Id], [Name], [PKId], [FKId], [type], [diagramId], [reltype]) VALUES (3, N'rel1', 1004, 1006, N'N:1', 1003, NULL)
GO
INSERT [dbo].[tblRelationship] ([Id], [Name], [PKId], [FKId], [type], [diagramId], [reltype]) VALUES (4, N'rel1', 1005, 1007, N'N:1', 1003, NULL)
GO