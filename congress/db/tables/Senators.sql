CREATE TABLE [dbo].[Senators]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [FullName] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [LastName] [nvarchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [FirstName] [nvarchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Party] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [State] [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Address] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Phone] [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Website] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Class] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [BioguideId] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [LisMemberId] [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    CONSTRAINT [PK_dbo.Senators] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [Idx_LisMemberId] ON [dbo].[Senators]
(
	[LisMemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO