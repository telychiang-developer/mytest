using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;
using System.Reflection;

namespace iSTMC.Common
{
   public class TraceLogger
   {
      private ILog m_Log = null;

      public TraceLogger(string logName, string logPrefix, log4net.Core.Level logLevel)
      {
         string LOG_PATTERN = "%-5level %date{HH:mm:ss} - %message%newline";
         string fileName = string.Format(@"./Logs/{0}.txt", logPrefix);

         Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
         hierarchy.Name = logName;

         PatternLayout patternLayout = new PatternLayout();
         patternLayout.ConversionPattern = LOG_PATTERN;
         patternLayout.ActivateOptions();

         ConsoleAppender consoleAppender = new ConsoleAppender();
         consoleAppender.Layout = patternLayout;
         consoleAppender.ActivateOptions();

         RollingFileAppender rollingFileAppender = new RollingFileAppender();
         rollingFileAppender.Layout = patternLayout;
         rollingFileAppender.AppendToFile = true;
         rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;

         rollingFileAppender.DatePattern = "(yyyy-MM-dd)";
         rollingFileAppender.PreserveLogFileNameExtension = true;
         rollingFileAppender.File = fileName;
         rollingFileAppender.StaticLogFileName = false;
         rollingFileAppender.ActivateOptions();

         // OFF > FATAL > ERROR > WARN > INFO > DEBUG  > ALL 
         hierarchy.Root.AddAppender(consoleAppender);
         hierarchy.Root.AddAppender(rollingFileAppender);
         hierarchy.Root.Level = logLevel;
         hierarchy.Configured = true;

         m_Log = LogManager.GetLogger(logName);
      }

      public ILog Logger { get { return m_Log; } }

      public void TraceDebug(string sFormatTrace, params object[] args)
      {
         Trace(log4net.Core.Level.Debug, true, "", sFormatTrace, args);
      }

      public void TraceInfo(string sFormatTrace, params object[] args)
      {
         Trace(log4net.Core.Level.Info, true, "", sFormatTrace, args);
      }

      public void TraceWarn(string sFormatTrace, params object[] args)
      {
         Trace(log4net.Core.Level.Warn, true, "", sFormatTrace, args);
      }

      public void TraceError(string sFormatTrace, params object[] args)
      {
         Trace(log4net.Core.Level.Error, true, "", sFormatTrace, args);
      }

      public void TraceFatal(string sFormatTrace, params object[] args)
      {
         Trace(log4net.Core.Level.Fatal, true, "", sFormatTrace, args);
      }

      public void Trace(log4net.Core.Level logLevel, string sFormatTrace, params object[] args)
      {
         Trace(logLevel, true, "", sFormatTrace, args);
      }

      public void TraceNoTime(log4net.Core.Level logLevel, string sFormatTrace, params object[] args)
      {
         Trace(logLevel, false, "", sFormatTrace, args);
      }

      public void Trace(log4net.Core.Level logLevel, bool bLogTime, string category, string sFormatTrace, params object[] args)
      {
         string msg = string.Format(sFormatTrace, args);

         if (bLogTime)
            category = DateTime.Now.ToString("hh:mm:ss") + "   " + category;

         if (logLevel == log4net.Core.Level.Debug)
            m_Log.DebugFormat(msg, category);
         else if (logLevel == log4net.Core.Level.Info)
            m_Log.InfoFormat(msg, category);
         else if (logLevel == log4net.Core.Level.Warn)
            m_Log.WarnFormat(msg, category);
         else if (logLevel == log4net.Core.Level.Error)
            m_Log.ErrorFormat(msg, category);
         else if (logLevel == log4net.Core.Level.Fatal)
            m_Log.FatalFormat(msg, category);
      }

      public static log4net.Core.Level LevelByName(string level)
      {
         switch (level)
         {
            case "INFO":
               return log4net.Core.Level.Info;
            case "WARN":
               return log4net.Core.Level.Warn;
            case "ERROR":
               return log4net.Core.Level.Error;
            case "FATAL":
               return log4net.Core.Level.Fatal;
            case "DEBUG":
               return log4net.Core.Level.Debug;
            case "ALL":
               return log4net.Core.Level.All;
            case "ALERT":
               return log4net.Core.Level.Alert;
            case "CRITICAL":
               return log4net.Core.Level.Critical;
            case "EMERGENCY":
               return log4net.Core.Level.Emergency;
            case "FINE":
               return log4net.Core.Level.Fine;
            case "FINER":
               return log4net.Core.Level.Finer;
            case "FINEST":
               return log4net.Core.Level.Finest;
            case "NOTICE":
               return log4net.Core.Level.Notice;
            case "OFF":
               return log4net.Core.Level.Off;
            case "SEVERE":
               return log4net.Core.Level.Severe;
            case "TRACE":
               return log4net.Core.Level.Trace;
            case "VERBOSE":
               return log4net.Core.Level.Verbose;
            default:
               return log4net.Core.Level.Info;
         }
      }
   }
}
