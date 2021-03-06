﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INHibernatePersistenceConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate
{
   using System.Data;

   using global::NHibernate;

   public interface INHibernatePersistenceConfiguration : IPersistenceConfiguration
   {
      INHibernatePersistenceConfiguration RegisterConfigurer(INHibernateConfigurer configurer);

      ISessionFactory CreateSessionFactory();

      void CreateSchema(IDbConnection connection);
   }
}
