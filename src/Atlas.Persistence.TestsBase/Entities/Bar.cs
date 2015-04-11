// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bar.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase.Entities
{
   public class Bar
   {
      public virtual long ID { get; protected set; }

      public virtual string Name { get; set; }
   }
}
