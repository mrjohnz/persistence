// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INHibernateConfigurer.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate
{
   public interface INHibernateConfigurer
   {
      void Configure(global::NHibernate.Cfg.Configuration configuration);
   }
}
