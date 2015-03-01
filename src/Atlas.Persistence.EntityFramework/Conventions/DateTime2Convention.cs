// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTime2Convention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Conventions
{
   using System;
   using System.Data.Entity.ModelConfiguration.Conventions;

   public class DateTime2Convention : Convention
   {
      public DateTime2Convention()
      {
         this.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
      }
   }
}
