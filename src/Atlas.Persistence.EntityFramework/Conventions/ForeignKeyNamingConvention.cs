// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForeignKeyNamingConvention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Conventions
{
   using System.Data.Entity.Core.Metadata.Edm;
   using System.Data.Entity.Infrastructure;
   using System.Data.Entity.ModelConfiguration.Conventions;

   public class ForeignKeyNamingConvention : IStoreModelConvention<AssociationType>
   {
      public void Apply(AssociationType associationType, DbModel model)
      {
         if (!associationType.IsForeignKey)
         {
            return;
         }

         var fromProperties = associationType.Constraint.FromProperties;
         var toProperties = associationType.Constraint.ToProperties;

         if (toProperties.Count != fromProperties.Count)
         {
            // TODO: Can this happen? Should we throw an error?
            return;
         }

         for (var i = 0; i < toProperties.Count; i++)
         {
            var columnName = toProperties[i].Name;
            var underscore = columnName.IndexOf('_');

            if (underscore == -1)
            {
               continue;
            }

            var navigationName = columnName.Substring(0, underscore);
            var primaryKeyName = fromProperties[i].Name;

            if (primaryKeyName.StartsWith(navigationName))
            {
               toProperties[i].Name = primaryKeyName;
            }
            else
            {
               toProperties[i].Name = navigationName + primaryKeyName;
            }
         }
      }
   }
}
