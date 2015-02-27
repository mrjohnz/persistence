USE $(database)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

PRINT 'Creating Table Audit'
--
CREATE TABLE [dbo].[Audit]
(
   [AuditID]          [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]             [uniqueidentifier],
   [CreatedDateTime]  [datetime2]        NOT NULL,
   [CreatedUserGuid]  [uniqueidentifier] NOT NULL,
   [ModifiedDateTime] [datetime2]        NOT NULL,
   [ModifiedUserGuid] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table AuditCreated'
--
CREATE TABLE [dbo].[AuditCreated]
(
   [AuditCreatedID]  [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]            [uniqueidentifier],
   [CreatedDateTime] [datetime2]        NOT NULL,
   [CreatedUserGuid] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table AuditCreatedAtOnly'
--
CREATE TABLE [dbo].[AuditCreatedAtOnly]
(
   [AuditCreatedAtOnlyID] [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]                 [uniqueidentifier],
   [CreatedDateTime]      [datetime2]        NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table AuditCreatedByOnly'
--
CREATE TABLE [dbo].[AuditCreatedByOnly]
(
   [AuditCreatedByOnlyID] [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]                 [uniqueidentifier],
   [CreatedUserGuid]      [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table AuditModified'
--
CREATE TABLE [dbo].[AuditModified]
(
   [AuditModifiedID]  [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]             [uniqueidentifier],
   [ModifiedDateTime] [datetime2]        NOT NULL,
   [ModifiedUserGuid] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table AuditModifiedAtOnly'
--
CREATE TABLE [dbo].[AuditModifiedAtOnly]
(
   [AuditModifiedAtOnlyID] [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]                  [uniqueidentifier],
   [ModifiedDateTime]      [datetime2]        NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table AuditModifiedByOnly'
--
CREATE TABLE [dbo].[AuditModifiedByOnly]
(
   [AuditModifiedByOnlyID] [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]                  [uniqueidentifier],
   [ModifiedUserGuid]      [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table BaseClass'
--
CREATE TABLE [dbo].[BaseClass]
(
   [BaseClassID] [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]        [uniqueidentifier] NOT NULL,
   [FooID]       [bigint],
   [IntEnum]     [int]              NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table BaseClassPartitioned'
--
CREATE TABLE [dbo].[BaseClassPartitioned]
(
   [BaseClassID]   [bigint]           NOT NULL IDENTITY(1,1),
   [Guid]          [uniqueidentifier] NOT NULL,
   [PartitionGuid] [uniqueidentifier] NOT NULL,
   [FooID]         [bigint],
   [IntEnum]       [int]              NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table Foo'
--
CREATE TABLE [dbo].[Foo]
(
   [FooID]         [bigint]                NOT NULL IDENTITY(1,1),
   [Guid]          [uniqueidentifier]      NOT NULL,
   [IntEnum]       [int]                   NOT NULL,
   [IntValue]      [int],
   [DateTimeValue] [datetime2],
	 [StringValue]   [nvarchar]         (50)
) ON [PRIMARY]
GO

PRINT 'Creating Table FooPartitioned'
--
CREATE TABLE [dbo].[FooPartitioned]
(
   [FooID]         [bigint]                NOT NULL IDENTITY(1,1),
   [Guid]          [uniqueidentifier]      NOT NULL,
   [PartitionGuid] [uniqueidentifier]      NOT NULL,
   [IntEnum]       [int]                   NOT NULL,
   [IntValue]      [int],
   [DateTimeValue] [datetime2],
	 [StringValue]   [nvarchar]         (50)
) ON [PRIMARY]
GO

PRINT 'Creating Table GuidChild'
--
CREATE TABLE [dbo].[GuidChild]
(
   [GuidChildID]  [uniqueidentifier] NOT NULL,
   [GuidParentID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table GuidParent'
--
CREATE TABLE [dbo].[GuidParent]
(
   [GuidParentID]    [uniqueidentifier]      NOT NULL,
   [Name]            [nvarchar]         (50),
   [CreatedDateTime] [datetime2]             NOT NULL
) ON [PRIMARY]
GO

PRINT 'Creating Table Optimistic'
--
CREATE TABLE [dbo].[Optimistic]
(
   [OptimisticID]  [bigint]         NOT NULL IDENTITY(1,1),
   [StringValue]   [nvarchar]  (50),
   [IntValue]      [int],
   [DateTimeValue] [datetime2],
   [Version]       rowversion
) ON [PRIMARY]
GO

PRINT 'Creating Table SubClass'
--
CREATE TABLE [dbo].[SubClass]
(
   [BaseClassID] [bigint]        NOT NULL,
   [Name]        [nvarchar] (50)
) ON [PRIMARY]
GO

PRINT 'Creating Table SubClassPartitioned'
--
CREATE TABLE [dbo].[SubClassPartitioned]
(
   [BaseClassID] [bigint]        NOT NULL,
   [Name]        [nvarchar] (50)
) ON [PRIMARY]
GO

PRINT 'Creating Table XmlProperty'
--
CREATE TABLE [dbo].[XmlProperty]
(
   [XmlPropertyID] [bigint] NOT NULL IDENTITY(1,1),
   [Xml]           [xml]
) ON [PRIMARY]
GO
