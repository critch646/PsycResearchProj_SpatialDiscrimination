using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Serilog;
using Serilog.Configuration;
using Serilog.Settings.Configuration;

namespace VerticalDominance
{
    public class AppLogging
    {
        private static ILogger instance = new LoggerConfiguration().ReadFrom.Configuration(App.Configuration, sectionName: "AppSerilog").CreateLogger();

        public event EventHandler<NewItemEventArgs> LogChanged;

        /// <summary>
        /// Logs passed text at 'information' level.
        /// </summary>
        /// <param name="text">Text to be logged.</param>
        public void Information(string text)
        {
            instance.Information(text);
            NewLogAdded(text);
        }


        /// <summary>
        /// Logs passed text at 'debug' level.
        /// </summary>
        /// <param name="text">Text to be logged.</param>
        public void Debug(string text)
        {
            instance.Debug(text);
            NewLogAdded(text);
        }


        /// <summary>
        /// Logs passed text at 'error' level.
        /// </summary>
        /// <param name="text">Text to be logged.</param>
        public void Error(string text)
        {
            instance.Error(text);
            NewLogAdded(text);
        }


        /// <summary>
        /// Logs passed text at 'warning' level.
        /// </summary>
        /// <param name="text">Text to be logged.</param>
        public void Warning(string text)
        {
            instance.Warning(text);
            NewLogAdded(text);
        }


        /// <summary>
        /// Adds new log entry with DateTime prepended to log message
        /// </summary>
        /// <param name="text">Message to be added to the log</param>
        private void NewLogAdded(string text)
        {
            EventHandler<NewItemEventArgs> handler = LogChanged;
            if (handler != null)
            {
                NewItemEventArgs e = new()
                {
                    Log = DateTime.Now.ToLongTimeString() + ":" + text
                };
                handler(this, e);
            }
        }
    }
}
