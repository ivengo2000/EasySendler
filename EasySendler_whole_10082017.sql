USE [MySMTPProgect]
GO
/****** Object:  ForeignKey [FK_RecipientListsRelation_RecipientLists]    Script Date: 08/10/2017 14:38:09 ******/
ALTER TABLE [dbo].[RecipientListsRelation] DROP CONSTRAINT [FK_RecipientListsRelation_RecipientLists]
GO
/****** Object:  ForeignKey [FK_RecipientListsRelation_Recipients]    Script Date: 08/10/2017 14:38:09 ******/
ALTER TABLE [dbo].[RecipientListsRelation] DROP CONSTRAINT [FK_RecipientListsRelation_Recipients]
GO
/****** Object:  StoredProcedure [dbo].[sp_getRecipientCountByListId]    Script Date: 08/10/2017 14:38:10 ******/
DROP PROCEDURE [dbo].[sp_getRecipientCountByListId]
GO
/****** Object:  StoredProcedure [dbo].[sp_getRecipientsByListId]    Script Date: 08/10/2017 14:38:10 ******/
DROP PROCEDURE [dbo].[sp_getRecipientsByListId]
GO
/****** Object:  StoredProcedure [dbo].[sp_SaveConfiguredRecipientList]    Script Date: 08/10/2017 14:38:10 ******/
DROP PROCEDURE [dbo].[sp_SaveConfiguredRecipientList]
GO
/****** Object:  Table [dbo].[RecipientListsRelation]    Script Date: 08/10/2017 14:38:09 ******/
ALTER TABLE [dbo].[RecipientListsRelation] DROP CONSTRAINT [FK_RecipientListsRelation_RecipientLists]
GO
ALTER TABLE [dbo].[RecipientListsRelation] DROP CONSTRAINT [FK_RecipientListsRelation_Recipients]
GO
DROP TABLE [dbo].[RecipientListsRelation]
GO
/****** Object:  Table [dbo].[Recipients]    Script Date: 08/10/2017 14:38:09 ******/
DROP TABLE [dbo].[Recipients]
GO
/****** Object:  Table [dbo].[Templates]    Script Date: 08/10/2017 14:38:09 ******/
DROP TABLE [dbo].[Templates]
GO
/****** Object:  UserDefinedTableType [dbo].[udtIntArray]    Script Date: 08/10/2017 14:38:10 ******/
DROP TYPE [dbo].[udtIntArray]
GO
/****** Object:  Table [dbo].[MailSettings]    Script Date: 08/10/2017 14:38:09 ******/
ALTER TABLE [dbo].[MailSettings] DROP CONSTRAINT [DF_MailSettings_EnableSSL]
GO
DROP TABLE [dbo].[MailSettings]
GO
/****** Object:  Table [dbo].[RecipientLists]    Script Date: 08/10/2017 14:38:09 ******/
DROP TABLE [dbo].[RecipientLists]
GO
/****** Object:  Table [dbo].[RecipientLists]    Script Date: 08/10/2017 14:38:09 ******/
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
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (1, N'All', N'All Contacts')
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (2, N'Cycles', N'All CyclesUser')
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (3, N'Guitars', N'AllGuitarsUser')
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (4, N'Teachers', N'AllTeachers')
INSERT [dbo].[RecipientLists] ([rlId], [Name], [Description]) VALUES (5, N'Students', N'AllStudents')
SET IDENTITY_INSERT [dbo].[RecipientLists] OFF
/****** Object:  Table [dbo].[MailSettings]    Script Date: 08/10/2017 14:38:09 ******/
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
	[EnableSSL] [bit] NOT NULL CONSTRAINT [DF_MailSettings_EnableSSL]  DEFAULT ((0)),
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
/****** Object:  UserDefinedTableType [dbo].[udtIntArray]    Script Date: 08/10/2017 14:38:10 ******/
CREATE TYPE [dbo].[udtIntArray] AS TABLE(
	[Value] [int] NOT NULL
)
GO
/****** Object:  Table [dbo].[Templates]    Script Date: 08/10/2017 14:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Templates](
	[TemplateId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Body] [nvarchar](max) NULL,
	[Thumbnail] [image] NULL,
 CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Templates] ON
INSERT [dbo].[Templates] ([TemplateId], [Name], [Description], [Body], [Thumbnail]) VALUES (1, N'First template', N'The first template', N'<div style="min-width: 320px; margin: 0px auto; background-color: #ffffff; font-family: Arial, Helvetica, sans-serif;">
<div style="background-color: #197ab3;">
<div style="color: #ffffff;">
<p style="margin: 0px; font-size: 22px; line-height: 76px; text-align: center;">Dear {{name}} !</p>
</div>
</div>
<div style="background-color: #80a8c0;">
<div style="color: #ffffff;">
<p style="margin: 0px; text-align: center;"><span style="font-size: 18px;">This email is for you!</span></p>
</div>
</div>
<div style="background-color: transparent; margin: 20px 0px;">
<div style="margin: 0px auto; min-width: 320px; max-width: 500px; width: calc(19000% - 98300px); overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: transparent;">
<div>
<p style="margin: 0px 0px 10px 10px; font-size: 18px;"><span style="line-height: 21px; font-weight: bolder; text-align: left; color: #555555;">Message:</span></p>
</div>
<div style="color: #aaaaaa; text-align: left;">
<p style="margin: 0px 10px; font-size: 14px; line-height: 17px;">{{message}}</p>
</div>
</div>
</div>
<div style="background-color: #444444;">
<div style="margin: 0px auto; min-width: 320px; max-width: 500px; width: calc(19000% - 98300px); overflow-wrap: break-word; word-wrap: break-word; word-break: break-word; background-color: transparent; padding: 10px;">
<div style="color: #bbbbbb; text-align: left; margin: 0px 10px;">
<p><span style="font-weight: bolder; font-size: 16px;">Email: {{email}}</span></p>
</div>
<div style="color: #bbbbbb; text-align: left; margin: 0px 10px;">
<p><span style="font-weight: bolder; font-size: 16px;">Site: {{site}}</span></p>
</div>
<div style="color: #bbbbbb; text-align: left; margin: 0px 10px;">
<p><span style="font-weight: bolder; font-size: 16px;">Purpose: {{purpose}}</span></p>
</div>
<div style="color: #ffffff;">
<p style="margin: 30px 0px 10px; font-size: 14px; line-height: 17px; text-align: center;">Copyright &copy; 2017 EasySendler. All rights reserved.</p>
</div>
</div>
</div>
</div>', NULL)
SET IDENTITY_INSERT [dbo].[Templates] OFF
/****** Object:  Table [dbo].[Recipients]    Script Date: 08/10/2017 14:38:09 ******/
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
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (2, N'artanger666@gmail.com', N'Arthur', N'Ivanov')
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (3, N'big-maining@bigmir.net', N'qwer', N'qwerty')
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (4, N'mybigmir24@bigmir.net', N'asdfg', N'qweer')
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (5, N'qwerty592@bigmir.net', N'qwerty', N'asdfasd')
INSERT [dbo].[Recipients] ([RecipientId], [Email], [Name], [SureName]) VALUES (6, N'sergey@brainjocks.com', N'Sergey', N'Ivanov')
SET IDENTITY_INSERT [dbo].[Recipients] OFF
/****** Object:  Table [dbo].[RecipientListsRelation]    Script Date: 08/10/2017 14:38:09 ******/
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
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (12, 4, 3)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (15, 4, 5)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (19, 4, 5)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (31, 2, 5)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (32, 2, 3)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (40, 3, 2)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (41, 1, 1)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (42, 1, 2)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (43, 1, 3)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (44, 1, 4)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (45, 1, 5)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (46, 5, 3)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (47, 5, 4)
INSERT [dbo].[RecipientListsRelation] ([Id], [rlId], [rId]) VALUES (48, 5, 5)
SET IDENTITY_INSERT [dbo].[RecipientListsRelation] OFF
/****** Object:  StoredProcedure [dbo].[sp_SaveConfiguredRecipientList]    Script Date: 08/10/2017 14:38:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[sp_getRecipientsByListId]    Script Date: 08/10/2017 14:38:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_getRecipientsByListId]
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
GO
/****** Object:  StoredProcedure [dbo].[sp_getRecipientCountByListId]    Script Date: 08/10/2017 14:38:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_getRecipientCountByListId]
(
	@rlId int 
)
AS
select
	count(r.RecipientId)

from Recipients r 
	join RecipientListsRelation rlr on r.RecipientId = rlr.rId
	join dbo.RecipientLists rl on rlr.rlId = rl.rlId
where rl.rlId = @rlId
GO
/****** Object:  ForeignKey [FK_RecipientListsRelation_RecipientLists]    Script Date: 08/10/2017 14:38:09 ******/
ALTER TABLE [dbo].[RecipientListsRelation]  WITH CHECK ADD  CONSTRAINT [FK_RecipientListsRelation_RecipientLists] FOREIGN KEY([rlId])
REFERENCES [dbo].[RecipientLists] ([rlId])
GO
ALTER TABLE [dbo].[RecipientListsRelation] CHECK CONSTRAINT [FK_RecipientListsRelation_RecipientLists]
GO
/****** Object:  ForeignKey [FK_RecipientListsRelation_Recipients]    Script Date: 08/10/2017 14:38:09 ******/
ALTER TABLE [dbo].[RecipientListsRelation]  WITH CHECK ADD  CONSTRAINT [FK_RecipientListsRelation_Recipients] FOREIGN KEY([rId])
REFERENCES [dbo].[Recipients] ([RecipientId])
GO
ALTER TABLE [dbo].[RecipientListsRelation] CHECK CONSTRAINT [FK_RecipientListsRelation_Recipients]
GO
