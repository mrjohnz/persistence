// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentAutoMapperConfigurer.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.Fluent
{
   using System;
   using System.Collections.Generic;
   using System.Reflection;

   using Atlas.Persistence.NHibernate;

   using FluentNHibernate.Automapping;
   using FluentNHibernate.Conventions;

   using global::NHibernate.Cfg;

   public class FluentAutoMapperConfigurer : INHibernateConfigurer
   {
      private readonly IList<Assembly> autoMapAssemblies = new List<Assembly>();
      private readonly IList<Assembly> overrideAssemblies = new List<Assembly>();
      private readonly IList<Type> conventionTypes = new List<Type>();
      private readonly IList<Action<AutoPersistenceModel>> overrides = new List<Action<AutoPersistenceModel>>();

      private IAutomappingConfiguration autoMappingConfiguration;

      public FluentAutoMapperConfigurer AutoMappingConfiguration(IAutomappingConfiguration autoMappingConfiguration)
      {
         this.autoMappingConfiguration = autoMappingConfiguration;

         return this;
      }

      public FluentAutoMapperConfigurer AutoMapEntitiesFromAssemblyOf<T>()
      {
         return this.AutoMapEntitiesFromAssembly(typeof(T).Assembly);
      }

      public FluentAutoMapperConfigurer AutoMapEntitiesFromAssembly(Assembly assembly)
      {
         this.autoMapAssemblies.Add(assembly);

         return this;
      }

      public FluentAutoMapperConfigurer RegisterOverridesFromAssemblyOf<T>()
      {
         return this.RegisterOverridesFromAssembly(typeof(T).Assembly);
      }

      public FluentAutoMapperConfigurer RegisterOverridesFromAssembly(Assembly assembly)
      {
         this.overrideAssemblies.Add(assembly);

         return this;
      }

      public FluentAutoMapperConfigurer RegisterConvention<TConvention>() where TConvention : IConvention
      {
         this.conventionTypes.Add(typeof(TConvention));

         return this;
      }

      public FluentAutoMapperConfigurer RegisterOverride<T>(Action<AutoMapping<T>> autoMapping)
      {
         this.overrides.Add(model => model.Override(autoMapping));

         return this;
      }

      public void Configure(Configuration configuration)
      {
         var autoPersistenceModel = new AutoPersistenceModel(this.autoMappingConfiguration);

         foreach (var conventionType in this.conventionTypes)
         {
            autoPersistenceModel.Conventions.Add(conventionType);
         }

         foreach (var assembly in this.autoMapAssemblies)
         {
            autoPersistenceModel.AddEntityAssembly(assembly);
         }

         foreach (var assembly in this.overrideAssemblies)
         {
            autoPersistenceModel.UseOverridesFromAssembly(assembly);
         }

         foreach (var @override in this.overrides)
         {
            @override(autoPersistenceModel);
         }

         autoPersistenceModel.Configure(configuration);
      }
   }
}
