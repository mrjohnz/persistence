// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByCodeMapperConfigurer.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.ByCode
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;

   using Atlas.Persistence.NHibernate;
   using Atlas.Persistence.NHibernate.Configuration.ByCode.Conventions;
   using Atlas.Persistence.NHibernate.UserTypes;

   using global::NHibernate.Caches.RtMemoryCache;
   using global::NHibernate.Cfg;
   using global::NHibernate.Mapping;
   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Type;

   public class ByCodeMapperConfigurer : INHibernateConfigurer
   {
      private readonly IList<Assembly> configurationAssemblies = new List<Assembly>();
      private readonly IList<Type> conventionTypes = new List<Type>();

      public ByCodeMapperConfigurer RegisterEntitiesFromAssemblyOf<T>()
      {
         return this.RegisterEntitiesFromAssembly(typeof(T).Assembly);
      }

      public ByCodeMapperConfigurer RegisterEntitiesFromAssembly(Assembly assembly)
      {
         this.configurationAssemblies.Add(assembly);

         return this;
      }

      public ByCodeMapperConfigurer RegisterConvention<TConvention>() where TConvention : IConvention
      {
         this.conventionTypes.Add(typeof(TConvention));

         return this;
      }

      public void Configure(Configuration configuration)
      {
         var conventions = this.conventionTypes.Select(c => Activator.CreateInstance(c, true)).ToList();

         var mapper = new ModelMapper();

         mapper.BeforeMapProperty += (inspector, member, customizer) =>
            {
               foreach (var convention in conventions.OfType<IPropertyConvention>())
               {
                  if (convention.Accept(member))
                  {
                     convention.Apply(customizer);
                  }
               }
            };

         foreach (var assembly in this.configurationAssemblies)
         {
            var types = assembly.GetExportedTypes().Select(c => new { Type = c, Order = GetMappingOrder(c) }).ToList();
            var orders = types.Select(c => c.Order).Distinct().OrderBy(c => c);

            foreach (var order in orders)
            {
               var safeOrder = order;

               mapper.AddMappings(types.Where(c => c.Order == safeOrder).Select(c => c.Type));
            }
         }

         var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

         configuration.AddDeserializedMapping(mapping, this.GetType().Name);

         foreach (var classMapping in configuration.ClassMappings)
         {
            if (classMapping.Version == null)
            {
               continue;
            }

            var column = classMapping.Version.ColumnIterator.Cast<Column>().First();
            var customType = column.Value.Type as CustomType;

            if (customType != null && customType.UserType is RowVersionType)
            {
               // TODO: This is SQL Server specific
               column.SqlType = "rowversion";
            }
         }

         configuration.Cache(c =>
            {
               c.Provider<RtMemoryCacheProvider>();
               c.UseQueryCache = true;
            });
      }

      private static int? GetMappingOrder(Type type)
      {
         var attribute = type.GetCustomAttributes(typeof(MappingOrderAttribute), false)
            .Cast<MappingOrderAttribute>()
            .SingleOrDefault();

         if (attribute != null)
         {
            return attribute.Order;
         }

         return null;
      }
   }
}
