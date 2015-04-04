//-----------------------------------------------------------------------
// <copyright file="IProperty.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   public interface IProperty<T_ENTITY>
      where T_ENTITY : class
   {
      string Name { get; }

      bool IsReadOnly { get; }

      void PullValue(T_ENTITY entity);

      void PushValue(T_ENTITY entity);

      void AssertAreEqual(T_ENTITY entity);
   }
}
