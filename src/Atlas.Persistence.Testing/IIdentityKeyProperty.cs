//-----------------------------------------------------------------------
// <copyright file="IIdentityKeyProperty.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   public interface IIdentityKeyProperty<T_ENTITY> : IKeyProperty<T_ENTITY>
      where T_ENTITY : class
   {
      void AssertNonZeroID();
   }
}
