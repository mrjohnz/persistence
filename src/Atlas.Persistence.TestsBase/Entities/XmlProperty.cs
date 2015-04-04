// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlProperty.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase.Entities
{
   using System.Xml.Linq;

   public class XmlProperty
   {
      public virtual long ID { get; protected set; }

      public virtual XElement Xml { get; set; }
   }
}
