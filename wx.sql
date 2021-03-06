USE [diduu_wx]
GO
/****** 对象:  Table [dbo].[FreeVIP]    脚本日期: 09/05/2015 23:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FreeVIP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TId] [int] NOT NULL DEFAULT ((0)),
	[Account] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL DEFAULT (''),
	[Pwd] [nvarchar](30) COLLATE Chinese_PRC_CI_AS NOT NULL DEFAULT (''),
	[StartTime] [datetime] NOT NULL DEFAULT (getdate()),
	[ValidTime] [int] NOT NULL DEFAULT ((6)),
	[IsEnable] [bit] NOT NULL DEFAULT ((1)),
	[Notes] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NOT NULL DEFAULT (''),
	[IPAddr] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF_FreeVIP_IPAddr]  DEFAULT (''),
	[AddTime] [datetime] NOT NULL CONSTRAINT [DF_FreeVIP_AddTime]  DEFAULT (getdate()),
	[UseTime] [datetime] NOT NULL CONSTRAINT [DF_FreeVIP_UseTime]  DEFAULT (getdate()),
	[OpenId] [nvarchar](512) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF_FreeVIP_OpenId]  DEFAULT (''),
	[UserAgent] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF_FreeVIP_UserAgent]  DEFAULT (''),
	[Latitude] [decimal](10, 10) NOT NULL CONSTRAINT [DF_FreeVIP_Latitude]  DEFAULT ((0.0)),
	[Longitude] [decimal](10, 10) NOT NULL CONSTRAINT [DF_FreeVIP_Longitude]  DEFAULT ((0.0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

-----------------------------------------------------------


USE [diduu_wx]
GO
/****** 对象:  Table [dbo].[VIPType]    脚本日期: 09/05/2015 23:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VIPType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VIPName] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL DEFAULT (''),
	[Notes] [nvarchar](2000) COLLATE Chinese_PRC_CI_AS NOT NULL DEFAULT (''),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
