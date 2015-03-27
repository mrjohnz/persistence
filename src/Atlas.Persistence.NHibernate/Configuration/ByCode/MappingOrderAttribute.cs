//-----------------------------------------------------------------------
// <copyright file="MappingOrderAttribute.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.ByCode
{
   using System;

   public class MappingOrderAttribute : Attribute
   {
      public MappingOrderAttribute(int order)
      {
         this.Order = order;
      }

      public int Order { get; private set; }
   }
}
