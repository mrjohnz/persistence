USE $(database)
GO

print 'Creating Primary Key on Audit'
--
ALTER TABLE [dbo].[Audit]
ADD CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED 
(
   [AuditID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on AuditCreated'
--
ALTER TABLE [dbo].[AuditCreated]
ADD CONSTRAINT [PK_AuditCreated] PRIMARY KEY CLUSTERED 
(
   [AuditCreatedID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on AuditCreatedAtOnly'
--
ALTER TABLE [dbo].[AuditCreatedAtOnly]
ADD CONSTRAINT [PK_AuditCreatedAtOnly] PRIMARY KEY CLUSTERED 
(
   [AuditCreatedAtOnlyID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on AuditCreatedByOnly'
--
ALTER TABLE [dbo].[AuditCreatedByOnly]
ADD CONSTRAINT [PK_AuditCreatedByOnly] PRIMARY KEY CLUSTERED 
(
   [AuditCreatedByOnlyID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on AuditModified'
--
ALTER TABLE [dbo].[AuditModified]
ADD CONSTRAINT [PK_AuditModified] PRIMARY KEY CLUSTERED 
(
   [AuditModifiedID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on AuditModifiedAtOnly'
--
ALTER TABLE [dbo].[AuditModifiedAtOnly]
ADD CONSTRAINT [PK_AuditModifiedAtOnly] PRIMARY KEY CLUSTERED 
(
   [AuditModifiedAtOnlyID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on AuditModifiedByOnly'
--
ALTER TABLE [dbo].[AuditModifiedByOnly]
ADD CONSTRAINT [PK_AuditModifiedByOnly] PRIMARY KEY CLUSTERED 
(
   [AuditModifiedByOnlyID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on Bar'
--
ALTER TABLE [dbo].[Bar]
ADD CONSTRAINT [PK_Bar] PRIMARY KEY CLUSTERED 
(
   [BarID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on BaseClass'
--
ALTER TABLE [dbo].[BaseClass]
ADD CONSTRAINT [PK_BaseClass] PRIMARY KEY CLUSTERED 
(
   [BaseClassID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on BaseClassPartitioned'
--
ALTER TABLE [dbo].[BaseClassPartitioned]
ADD CONSTRAINT [PK_BaseClassPartitioned] PRIMARY KEY CLUSTERED 
(
   [BaseClassID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on Foo'
--
ALTER TABLE [dbo].[Foo]
ADD CONSTRAINT [PK_Foo] PRIMARY KEY CLUSTERED 
(
   [FooID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on FooPartitioned'
--
ALTER TABLE [dbo].[FooPartitioned]
ADD CONSTRAINT [PK_FooPartitioned] PRIMARY KEY CLUSTERED 
(
   [FooID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on GuidChild'
--
ALTER TABLE [dbo].[GuidChild]
ADD CONSTRAINT [PK_GuidChild] PRIMARY KEY CLUSTERED 
(
   [GuidChildID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on GuidParent'
--
ALTER TABLE [dbo].[GuidParent]
ADD CONSTRAINT [PK_GuidParent] PRIMARY KEY CLUSTERED 
(
   [GuidParentID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on Optimistic'
--
ALTER TABLE [dbo].[Optimistic]
ADD CONSTRAINT [PK_Optimistic] PRIMARY KEY CLUSTERED 
(
   [OptimisticID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on SubClass'
--
ALTER TABLE [dbo].[SubClass]
ADD CONSTRAINT [PK_SubClass] PRIMARY KEY CLUSTERED 
(
   [BaseClassID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on SubClassPartitioned'
--
ALTER TABLE [dbo].[SubClassPartitioned]
ADD CONSTRAINT [PK_SubClassPartitioned] PRIMARY KEY CLUSTERED 
(
   [BaseClassID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO

print 'Creating Primary Key on XmlProperty'
--
ALTER TABLE [dbo].[XmlProperty]
ADD CONSTRAINT [PK_XmlProperty] PRIMARY KEY CLUSTERED 
(
   [XmlPropertyID] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
ON [PRIMARY]
GO
