﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrencyTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.UnitOfWork
{
   using Atlas.Persistence.Testing;

   using NUnit.Framework;

   [TestFixture]
   public class ConcurrencyTests : ConcurrencyTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         return Helper.CreateUnitOfWorkFactory();
      }
   }
}
