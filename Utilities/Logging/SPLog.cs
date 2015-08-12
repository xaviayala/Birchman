using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birchman.RemoteProvisioning.Utilities
{
    public class SharePointLog : SPDiagnosticsServiceBase
    {
        #region Attributes

        private SharePointLog() : base("Birchman - SharePoint Logging Service", SPFarm.Local) { }

        private const string APPLICATION_NAME = "Birchman RemoteProvisioning Application";

        internal enum Level
        {
            INFO,
            HIGH,
            ERROR,
            CRITICAL
        }
        private static SharePointLog _current;

        public static SharePointLog Current
        {
            get
            {
                if (_current == null)
                    _current = new SharePointLog();
                return _current;
            }
        }

        #endregion Attributes

        #region Overrides

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>{
                    new SPDiagnosticsArea(APPLICATION_NAME, new List<SPDiagnosticsCategory>{
                    new SPDiagnosticsCategory(SharePointLog.Level.INFO.ToString(), TraceSeverity.Verbose, EventSeverity.Information),
                    new SPDiagnosticsCategory(SharePointLog.Level.HIGH.ToString(), TraceSeverity.High, EventSeverity.Warning),
                    new SPDiagnosticsCategory(SharePointLog.Level.ERROR.ToString(), TraceSeverity.Unexpected, EventSeverity.Error),
                    new SPDiagnosticsCategory(SharePointLog.Level.CRITICAL.ToString(), TraceSeverity.Unexpected, EventSeverity.ErrorCritical)
                    })
            };

            return areas;
        }

        #endregion Overrides

        #region Methods



        /// <summary>
        /// Writes Message in the ULS SharePoint Log with High severity level.
        /// Also writes the entry in Event Viewer as Warning message.
        /// </summary>
        /// <param name="applicationName">Name of the application. It will be included on the message</param>
        /// <param name="currentClass">Name of the current class. It will be included on the message</param>
        /// <param name="currentFunction">Name of the current function. It will be included on the message</param>
        /// <param name="message">Message that you want to trace</param>
        public static void LogWarning(string applicationName, string currentClass, string currentFunction, string message)
        {
            try
            {
                //LogTrace(applicationName, currentClass, currentFunction, message, TraceSeverity.High);
                SPDiagnosticsCategory category = SharePointLog.Current.Areas[APPLICATION_NAME].Categories[SharePointLog.Level.HIGH.ToString()];
                SPDiagnosticsService.Local.WriteTrace(0, category, TraceSeverity.High, "(" + applicationName + ") " + currentClass + " - [" + currentFunction + "] - " + message, null);
            }
            catch { }
        }

        /// <summary>
        /// Writes Message in the ULS SharePoint Log with Unexpected severity level. 
        /// Also writes the entry in Event Viewer as an Error.
        /// </summary>
        /// <param name="applicationName">Name of the application. It will be included on the message</param>
        /// <param name="currentClass">Name of the current class. It will be included on the message</param>
        /// <param name="currentFunction">Name of the current function. It will be included on the message</param>
        /// <param name="message">Message that you want to trace</param>
        public static void LogError(string applicationName, string currentClass, string currentFunction, string message)
        {
            try
            {
                SPDiagnosticsCategory category = SharePointLog.Current.Areas[APPLICATION_NAME].Categories[SharePointLog.Level.CRITICAL.ToString()];
                SPDiagnosticsService.Local.WriteTrace(0, category, TraceSeverity.Unexpected, "(" + applicationName + ") " + currentClass + " - [" + currentFunction + "] - " + message, null);
            }
            catch { }
        }

        /// <summary>
        /// Writes Message in the ULS SharePoint Log with Verbose severity level. 
        /// Also writes the entry in Event Viewer as an Information.
        /// Be aware with Service trace level configured on Central Administration.
        /// </summary>
        /// <param name="applicationName">Name of the application. It will be included on the message</param>
        /// <param name="currentClass">Name of the current class. It will be included on the message</param>
        /// <param name="currentFunction">Name of the current function. It will be included on the message</param>
        /// <param name="message">Message that you want to trace</param>
        public static void LogDebug(string applicationName, string currentClass, string currentFunction, string message)
        {
            try
            {
                SPDiagnosticsCategory category = SharePointLog.Current.Areas[APPLICATION_NAME].Categories[SharePointLog.Level.INFO.ToString()];
                SPDiagnosticsService.Local.WriteTrace(0, category, TraceSeverity.Verbose, "(" + applicationName + ") " + currentClass + " - [" + currentFunction + "] - " + message, null);
            }
            catch { }
        }

        /// <summary>
        /// Writes Message in the ULS SharePoint Log with Verbose severity level. 
        /// Also writes the entry in Event Viewer as an Information.
        /// Writes Enter Message.
        /// Be aware with Service trace level configured on Central Administration.
        /// </summary>
        /// <param name="applicationName">Name of the application. It will be included on the message</param>
        /// <param name="currentClass">Name of the current class. It will be included on the message</param>
        /// <param name="currentFunction">Name of the current function. It will be included on the message</param>
        public static void LogEnter(string applicationName, string currentClass, string currentFunction)
        {
            try
            {
                LogDebug(applicationName, currentClass, currentFunction, "Enter.");
            }
            catch { }
        }

        /// <summary>
        /// Writes Message in the ULS SharePoint Log with Verbose severity level. 
        /// Also writes the entry in Event Viewer as an Information.
        /// Writes Exit Message.
        /// Be aware with Service trace level configured on Central Administration.
        /// </summary>
        /// <param name="applicationName">Name of the application. It will be included on the message</param>
        /// <param name="currentClass">Name of the current class. It will be included on the message</param>
        /// <param name="currentFunction">Name of the current function. It will be included on the message</param>
        public static void LogExit(string applicationName, string currentClass, string currentFunction)
        {
            try
            {
                LogDebug(applicationName, currentClass, currentFunction, "Exit.");
            }
            catch { }
        }

        #endregion Methods
    }
}
