USE [MySMTPProgect]
GO
/****** Object:  Table [dbo].[Templates]    Script Date: 07/30/2017 20:31:24 ******/
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
<p style="margin: 0px; font-size: 22px; line-height: 76px; text-align: center;">LexisNexis Risk Solutions ggg</p>
</div>
</div>
<div style="background-color: #80a8c0;">
<div style="color: #ffffff;">
<p style="margin: 0px; text-align: center;"><span style="line-height: 56px; font-size: 18px;">Site Feedback Form</span></p>
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
<p style="margin: 30px 0px 10px; font-size: 14px; line-height: 17px; text-align: center;">Copyright &copy; 2017 LexisNexis. All rights reserved.</p>
</div>
</div>
</div>
</div>', NULL)
SET IDENTITY_INSERT [dbo].[Templates] OFF
