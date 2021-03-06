USE [master]
GO
/****** Object:  Database [MySMTPProgect]    Script Date: 05/19/2017 20:43:14 ******/
CREATE DATABASE [MySMTPProgect] ON  PRIMARY 
( NAME = N'MySMTPProgect', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MySMTPProgect.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MySMTPProgect_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MySMTPProgect_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MySMTPProgect] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MySMTPProgect].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MySMTPProgect] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [MySMTPProgect] SET ANSI_NULLS OFF
GO
ALTER DATABASE [MySMTPProgect] SET ANSI_PADDING OFF
GO
ALTER DATABASE [MySMTPProgect] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [MySMTPProgect] SET ARITHABORT OFF
GO
ALTER DATABASE [MySMTPProgect] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [MySMTPProgect] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [MySMTPProgect] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [MySMTPProgect] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [MySMTPProgect] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [MySMTPProgect] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [MySMTPProgect] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [MySMTPProgect] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [MySMTPProgect] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [MySMTPProgect] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [MySMTPProgect] SET  DISABLE_BROKER
GO
ALTER DATABASE [MySMTPProgect] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [MySMTPProgect] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [MySMTPProgect] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [MySMTPProgect] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [MySMTPProgect] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [MySMTPProgect] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [MySMTPProgect] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [MySMTPProgect] SET  READ_WRITE
GO
ALTER DATABASE [MySMTPProgect] SET RECOVERY FULL
GO
ALTER DATABASE [MySMTPProgect] SET  MULTI_USER
GO
ALTER DATABASE [MySMTPProgect] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [MySMTPProgect] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'MySMTPProgect', N'ON'
GO
USE [MySMTPProgect]
GO
/****** Object:  Table [dbo].[RecipientLists]    Script Date: 05/19/2017 20:43:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecipientLists](
	[rlId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL,
 CONSTRAINT [PK_RecipientLists] PRIMARY KEY CLUSTERED 
(
	[rlId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[RecipientLists] ON
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (1, N'All', N'AllContacts')
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (2, N'Cycles', N'AllCyclesUser')
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (3, N'Guitars', N'AllGuitarsUser')
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (4, N'Teachers', N'AllTeachers')
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (5, N'Students', N'AllStudents')
SET IDENTITY_INSERT [dbo].[RecipientLists] OFF
/****** Object:  Table [dbo].[MailSettings]    Script Date: 05/19/2017 20:43:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailSettings](
	[MailSettingsId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[Password] [nvarchar](200) NULL,
	[SMTP] [nvarchar](200) NOT NULL,
	[SMTPPort] [nvarchar](200) NOT NULL,
	[Pop3] [nvarchar](200) NULL,
	[Pop3Port] [nvarchar](200) NULL,
	[EnableSSL] [bit] NOT NULL,
	[Imap] [nvarchar](200) NULL,
	[ImapPort] [nvarchar](200) NULL,
 CONSTRAINT [PK_MailSettings] PRIMARY KEY CLUSTERED 
(
	[MailSettingsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_MailSettings] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[MailSettings] ON
INSERT [dbo].[MailSettings] ([MailSettingsId], [Email], [Password], [SMTP], [SMTPPort], [Pop3], [Pop3Port], [EnableSSL], [Imap], [ImapPort]) VALUES (1, N'artmagnus884@gmail.com', N'zaq147xsw258', N'smtp.gmail.com', N'465', N'', N'', 1, N'imap.gmail.com', N'993')
INSERT [dbo].[MailSettings] ([MailSettingsId], [Email], [Password], [SMTP], [SMTPPort], [Pop3], [Pop3Port], [EnableSSL], [Imap], [ImapPort]) VALUES (2, N'artimag666@gmail.com', N'03meGArul93', N'smtp.gmail.com', N'465', NULL, NULL, 1, N'imap.gmail.com', N'993')
SET IDENTITY_INSERT [dbo].[MailSettings] OFF
/****** Object:  Table [dbo].[Templates]    Script Date: 05/19/2017 20:43:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Templates](
	[TemplateId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
 CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipients]    Script Date: 05/19/2017 20:43:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipients](
	[RecipientId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[SureName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Recipients] PRIMARY KEY CLUSTERED 
(
	[RecipientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Recipients] ON
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (1, N'artanger@bigmir.net', N'qwe', N'rty')
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (2, N'artanger666@gmail.com', N'qwe', N'rty')
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (3, N'big-maining@bigmir.net', N'qwer', N'qwerty')
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (4, N'mybigmir24@bigmir.net', N'asdfg', N'qweer')
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (5, N'qwerty592@bigmir.net', N'qwerty', N'asdfasd')
SET IDENTITY_INSERT [dbo].[Recipients] OFF
/****** Object:  Table [dbo].[RecipientListsRelation]    Script Date: 05/19/2017 20:43:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecipientListsRelation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[rlId] [int] NOT NULL,
	[rId] [int] NOT NULL,
 CONSTRAINT [PK_RecipientListsRelation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[RecipientListsRelation] ON
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (1, 1, 1)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (2, 1, 2)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (3, 1, 3)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (4, 1, 4)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (9, 1, 5)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (10, 2, 1)
SET IDENTITY_INSERT [dbo].[RecipientListsRelation] OFF
/****** Object:  Default [DF_MailSettings_EnableSSL]    Script Date: 05/19/2017 20:43:15 ******/
ALTER TABLE [dbo].[MailSettings] ADD  CONSTRAINT [DF_MailSettings_EnableSSL]  DEFAULT ((0)) FOR [EnableSSL]
GO
/****** Object:  ForeignKey [FK_RecipientListsRelation_RecipientLists]    Script Date: 05/19/2017 20:43:15 ******/
ALTER TABLE [dbo].[RecipientListsRelation]  WITH CHECK ADD  CONSTRAINT [FK_RecipientListsRelation_RecipientLists] FOREIGN KEY([rlId])
REFERENCES [dbo].[RecipientLists] ([rlId])
GO
ALTER TABLE [dbo].[RecipientListsRelation] CHECK CONSTRAINT [FK_RecipientListsRelation_RecipientLists]
GO
/****** Object:  ForeignKey [FK_RecipientListsRelation_Recipients]    Script Date: 05/19/2017 20:43:15 ******/
ALTER TABLE [dbo].[RecipientListsRelation]  WITH CHECK ADD  CONSTRAINT [FK_RecipientListsRelation_Recipients] FOREIGN KEY([rId])
REFERENCES [dbo].[Recipients] ([RecipientId])
GO
ALTER TABLE [dbo].[RecipientListsRelation] CHECK CONSTRAINT [FK_RecipientListsRelation_Recipients]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_getRecipientsByListId]
(
	@rlId int 
)
AS
select
	r.RecipientId,
	r.Email,
	1 as Selected
from Recipients r 
	join RecipientListsRelation rlr on r.RecipientId = rlr.rId
	join dbo.RecipientLists rl on rlr.rlId = rl.rlId
where rl.rlId = @rlId

union  

select
	rr.RecipientId, 
	rr.Email, 
	0 as Selected
from Recipients rr 
where rr.RecipientId NOT in
(
	select
		r.RecipientId
	from Recipients r 
		join RecipientListsRelation rlr on r.RecipientId = rlr.rId
		join dbo.RecipientLists rl on rlr.rlId = rl.rlId
	where rl.rlId = @rlId
)


CREATE TYPE [dbo].[udtIntArray] AS TABLE(
	[Value] [int] NOT NULL
)
GO

CREATE PROCEDURE [dbo].[sp_SaveConfiguredRecipientList]
(
	@listId int,
	@ids dbo.udtIntArray readonly
)	
AS
BEGIN
	BEGIN TRAN
		delete from RecipientListsRelation where rlId = @listId
		insert into RecipientListsRelation select @listId as rlId, [Value] as rId from @ids
	COMMIT;
END
GO