// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentMapperConfigurer.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.Fluent
{
   using System.Collections.Generic;
   using System.Reflection;

   using Atlas.Persistence.NHibernate;

   using FluentNHibernate.Cfg;
   using FluentNHibernate.Conventions;

   using global::NHibernate.Caches.RtMemoryCache;
   using global::NHibernate.Cfg;

   public class FluentMapperConfigurer : INHibernateConfigurer
   {
      private readonly IList<Assembly> configurationAssemblies = new List<Assembly>();
      private readonly IList<IConvention> conventions = new List<IConvention>();

      public FluentMapperConfigurer()
      {
         this.conventions.Add(FluentNHibernate.Conventions.Helpers.PrimaryKey.Name.Is(c => c.EntityType.Name + "ID"));
         this.conventions.Add(FluentNHibernate.Conventions.Helpers.ForeignKey.EndsWith("ID"));
      }

      public FluentMapperConfigurer RegisterEntitiesFromAssemblyOf<T>()
      {
         return this.RegisterEntitiesFromAssembly(typeof(T).Assembly);
      }

      public FluentMapperConfigurer RegisterEntitiesFromAssembly(Assembly assembly)
      {
         this.configurationAssemblies.Add(assembly);

         return this;
      }

      public FluentMapperConfigurer RegisterConvention<TConvention>() where TConvention : IConvention, new()
      {
         this.conventions.Add(new TConvention());

         return this;
      }

      public FluentMapperConfigurer RegisterConvention<TConvention>(TConvention convention) where TConvention : IConvention
      {
         this.conventions.Add(convention);

         return this;
      }

      public void Configure(Configuration configuration)
      {
         Fluently
            .Configure(configuration)
            .Mappings(mappingConfiguration =>
               {
                  foreach (var convention in this.conventions)
                  {
                     mappingConfiguration.FluentMappings.Conventions.Add(convention);
                  }

                  foreach (var assembly in this.configurationAssemblies)
                  {
                     mappingConfiguration.FluentMappings.AddFromAssembly(assembly);
                  }
               })
            .Cache(c => c.UseQueryCache().UseSecondLevelCache().ProviderClass<RtMemoryCacheProvider>())
            .BuildConfiguration();
      }
   }
}
