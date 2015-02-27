// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPersistenceLogger.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;

   public interface IPersistenceLogger
   {
      void LogError(string format, Exception exception, params object[] args);

      void LogWarning(string format, params object[] args);

      void LogInfo(string format, params object[] args);

      void LogDebug(string format, params object[] args);
   }
}
