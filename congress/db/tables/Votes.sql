CREATE TABLE [dbo].[Votes]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [SenatorId] [int] NOT NULL,
    [LegislativeItemId] [int] NOT NULL,
    [VoteCast] [nvarchar](50) NULL,
    CONSTRAINT [PK_dbo.Votes] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Votes_dbo.LegislativeItems_LegislativeItemId] FOREIGN KEY([LegislativeItemId])
REFERENCES [dbo].[LegislativeItems] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_dbo.Votes_dbo.LegislativeItems_LegislativeItemId]
GO

ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Votes_dbo.Senators_SenatorId] FOREIGN KEY([SenatorId])
REFERENCES [dbo].[Senators] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_dbo.Votes_dbo.Senators_SenatorId]
GO

CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-20200705-144814] ON [dbo].[Votes]
(
	[SenatorId] ASC,
	[LegislativeItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
