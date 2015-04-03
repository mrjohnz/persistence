// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserContext.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;

   public interface IUserContext
   {
      Guid UserGuid { get; }
   }
}
