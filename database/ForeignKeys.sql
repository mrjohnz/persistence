USE $(database)
GO

print 'Creating Foreign Keys on BaseClass'
--
ALTER TABLE [dbo].[BaseClass] WITH CHECK
ADD CONSTRAINT [FK_BaseClass_Foo]
FOREIGN KEY ([FooID])
REFERENCES [Foo] ([FooID])
GO

print 'Creating Foreign Keys on BaseClassPartitioned'
--
ALTER TABLE [dbo].[BaseClassPartitioned] WITH CHECK
ADD CONSTRAINT [FK_BaseClassPartitioned_FooPartitioned]
FOREIGN KEY([FooID])
REFERENCES [FooPartitioned] ([FooID])
GO

print 'Creating Foreign Keys on GuidChild'
--
ALTER TABLE [dbo].[GuidChild] WITH CHECK
ADD CONSTRAINT [FK_GuidChild_GuidParent]
FOREIGN KEY ([GuidParentID])
REFERENCES [GuidParent] ([GuidParentID])
GO

print 'Creating Foreign Keys on SubClass'
--
ALTER TABLE [dbo].[SubClass] WITH CHECK
ADD CONSTRAINT [FK_SubClass_BaseClass]
FOREIGN KEY ([BaseClassID])
REFERENCES [BaseClass] ([BaseClassID])
GO

print 'Creating Foreign Keys on SubClassPartitioned'
--
ALTER TABLE [dbo].[SubClassPartitioned] WITH CHECK
ADD CONSTRAINT [FK_SubClassPartitioned_BaseClassPartitioned]
FOREIGN KEY ([BaseClassID])
REFERENCES [BaseClassPartitioned] ([BaseClassID])
GO
