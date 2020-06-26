CREATE TABLE [dbo].[LegislativeItems]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [SessionId] [int] NOT NULL,
    [Title] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [VoteNumber] [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [VoteDate] [datetime] NULL,
    [ModifyDate] [datetime] NULL,
    [VoteQuestionText] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [VoteDocumentText] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [MajorityRequirement] [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DocumentCongress] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DocumentType] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DocumentNumber] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DocumentName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DocumentTitle] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DocumentShortTitle] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [AmendmentNumber] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [AmendmentToDocumentNumber] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [AmendmentPurpose] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Issue] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [IssueLink] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Question] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Result] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [YeaCount] [int] NOT NULL,
    [NayCount] [int] NOT NULL,
    [PresentCount] [int] NOT NULL,
    [AbsentCount] [int] NOT NULL,
    [TieBreakerByWhom] [nvarchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [TieBreakerVote] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    CONSTRAINT [PK_dbo.LegislativeItems] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[LegislativeItems]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LegislativeItems_dbo.Sessions_SessionId] FOREIGN KEY([SessionId])
REFERENCES [dbo].[Sessions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LegislativeItems] CHECK CONSTRAINT [FK_dbo.LegislativeItems_dbo.Sessions_SessionId]
GO

CREATE UNIQUE NONCLUSTERED INDEX [Idx_FindLegislativeItem] ON [dbo].[LegislativeItems]
(
	[SessionId] ASC,
	[VoteNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

