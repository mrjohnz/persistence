// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProxyConfigurer.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration
{
   using Atlas.Persistence.NHibernate;

   using global::NHibernate.Bytecode;
   using global::NHibernate.Cfg;

   public class ProxyConfigurer<TProxyFactoryFactory> : INHibernateConfigurer
      where TProxyFactoryFactory : IProxyFactoryFactory
   {
      public void Configure(Configuration configuration)
      {
         configuration.Proxy(c => c.ProxyFactoryFactory<TProxyFactoryFactory>());
      }
   }
}
