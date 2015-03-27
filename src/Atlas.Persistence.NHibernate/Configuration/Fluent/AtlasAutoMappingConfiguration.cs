// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AtlasAutoMappingConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.Fluent
{
   using System;
   using System.Reflection;

   using FluentNHibernate;
   using FluentNHibernate.Automapping;

   public class AtlasAutoMappingConfiguration : DefaultAutomappingConfiguration
   {
      private Func<Type, bool> shouldMapTypeFunc;
      private Func<PropertyInfo, bool> shouldMapPropertyFunc;

      public AtlasAutoMappingConfiguration ShouldMapType(Func<Type, bool> shouldMapType)
      {
         this.shouldMapTypeFunc = shouldMapType;

         return this;
      }

      public AtlasAutoMappingConfiguration ShouldMapProperty(Func<PropertyInfo, bool> shouldMapProperty)
      {
         this.shouldMapPropertyFunc = shouldMapProperty;

         return this;
      }

      public override bool ShouldMap(Type type)
      {
         if (!base.ShouldMap(type))
         {
            return false;
         }

         if (this.shouldMapTypeFunc == null)
         {
            return true;
         }

         return this.shouldMapTypeFunc(type);
      }

      public override bool ShouldMap(Member member)
      {
         if (!base.ShouldMap(member))
         {
            return false;
         }

         if (this.shouldMapPropertyFunc == null)
         {
            return true;
         }

         var propertyInfo = (PropertyInfo)member.MemberInfo;

         return this.shouldMapPropertyFunc(propertyInfo);
      }
   }
}
