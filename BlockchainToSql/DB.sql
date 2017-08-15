USE [blockchain]
GO

drop table dbo.Inputs
drop table dbo.Outputs
drop table dbo.Transactions
drop table dbo.Blocks
drop table dbo.MetaDatas

GO

USE [blockchain]
GO

/****** Object:  Table [dbo].[MetaDatas]    Script Date: 8/14/2017 10:08:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MetaDatas](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FilePath] [nvarchar](512) NOT NULL,
	[Position] [bigint] NOT NULL,
	[BlockchainPosition] [bigint] NOT NULL,
	[BlockLength] [bigint] NOT NULL,
 CONSTRAINT [PK_metadatas] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Blocks]    Script Date: 6/24/2017 9:51:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blocks](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MetaDataID] [bigint] NOT NULL,
	[Length] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[Nonce] [bigint] NOT NULL,
	[Bits] [int] NOT NULL,
	[TargetDifficulty] [float] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[BlockHash] [binary](32) NOT NULL,
	[PreviousBlockHash] [binary](32) NOT NULL,
	[MerkleRoot] [binary](32) NOT NULL,
 CONSTRAINT [PK_blocks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Inputs]    Script Date: 6/24/2017 9:51:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inputs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionID] [bigint] NOT NULL,
	[TransactionHash] [binary](32) NOT NULL,
	[TransactionIndex] [bigint] NOT NULL,
	[Script] [varbinary](max) NOT NULL,
	[SequenceNumber] [bigint] NOT NULL,
	[Coinbase] [bit] NOT NULL,
 CONSTRAINT [PK_Inputs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Outputs]    Script Date: 6/24/2017 9:51:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Outputs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionID] [bigint] NOT NULL,
	[Value] [bigint] NOT NULL,
	[Script] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Outputs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 6/24/2017 9:51:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BlockID] [bigint] NOT NULL,
	[Version] [bigint] NOT NULL,
	[Coinbase] [bit] NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Blocks]  WITH CHECK ADD  CONSTRAINT [FK_Blocks_Metadatas] FOREIGN KEY([MetaDataID])
REFERENCES [dbo].[Metadatas] ([ID])
GO
ALTER TABLE [dbo].[Inputs]  WITH CHECK ADD  CONSTRAINT [FK_Inputs_Inputs] FOREIGN KEY([TransactionID])
REFERENCES [dbo].[Transactions] ([ID])
GO
ALTER TABLE [dbo].[Inputs] CHECK CONSTRAINT [FK_Inputs_Inputs]
GO
ALTER TABLE [dbo].[Outputs]  WITH CHECK ADD  CONSTRAINT [FK_Outputs_Transactions] FOREIGN KEY([TransactionID])
REFERENCES [dbo].[Transactions] ([ID])
GO
ALTER TABLE [dbo].[Outputs] CHECK CONSTRAINT [FK_Outputs_Transactions]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_blocks] FOREIGN KEY([BlockID])
REFERENCES [dbo].[Blocks] ([ID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_blocks]
GO
