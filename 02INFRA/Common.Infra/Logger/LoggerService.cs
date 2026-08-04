using Common.App.Contracts;
using Common.Domain.DTO.Model.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NLog;
using System.Reflection;

namespace Common.Infra.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly LoggerSettings loggerSettings;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public LoggerService()
        {
            loggerSettings = new LoggerSettings();
        }
        public LoggerService(IOptions<LoggerSettings> _loggerSettings)
        {
            loggerSettings = _loggerSettings.Value;
            if (loggerSettings == null)
                loggerSettings = new LoggerSettings();
        }
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(Environment.NewLine + message + Environment.NewLine);
        }

        public void LogInfo(string message)
        {
            //if (loggerSettings.EnableInfoLog)
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message + Environment.NewLine);
        }
        public void LogTrace(string message)
        {
            logger.Trace(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fileName"></param>
        public void LogFile(string message, string fileName, string folderName = "Transactions")
        {
            if (!string.IsNullOrEmpty(message) && loggerSettings.EnableFileLog)
            {
                var m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                try
                {
                    if (!Directory.Exists(Path.Combine(m_exePath, "Logs", folderName)))
                        Directory.CreateDirectory(Path.Combine(m_exePath, "Logs", folderName));

                    fileName = !string.IsNullOrEmpty(fileName) ? fileName : DateTime.Now.Ticks.ToString();
                    using (StreamWriter w = File.AppendText(Path.Combine(m_exePath, "Logs", folderName, fileName + ".txt")))
                    {
                        Log(message, w);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        private void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("\r\nLog Entry : {0} {1}: ", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                //txtWriter.WriteLine("  :");
                txtWriter.WriteLine("   {0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
