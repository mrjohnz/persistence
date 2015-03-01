// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log4NetPersistenceLogger.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Log4Net
{
   using System;

   using log4net;

   public class Log4NetPersistenceLogger : IPersistenceLogger
   {
      private readonly ILog log;

      public Log4NetPersistenceLogger(ILog log)
      {
         this.log = log;
      }

      public Log4NetPersistenceLogger()
         : this(LogManager.GetLogger("Atlas.Persistence"))
      {
      }

      public static IPersistenceLogger FromConfig()
      {
         log4net.Config.XmlConfigurator.Configure();

         return new Log4NetPersistenceLogger();
      }

      public void LogError(string format, Exception exception, params object[] args)
      {
         var message = string.Format(format, args);
         this.log.Error(message, exception);
      }

      public void LogWarning(string format, params object[] args)
      {
         this.log.WarnFormat(format, args);
      }

      public void LogInfo(string format, params object[] args)
      {
         this.log.InfoFormat(format, args);
      }

      public void LogDebug(string format, params object[] args)
      {
         this.log.DebugFormat(format, args);
      }
   }
}
