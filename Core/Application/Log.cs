using System;
using System.Collections.Generic;
using System.Linq;
using Cortex.Utilities;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace LeadingEdge.Curator.Core
{
	public static class Log
	{
		private static string AppPath { get; set; }
		private static string ApplicationInsightInstrumentationKey { get; set; }
		private static Dictionary<string, string> ApplicationMonitoringInfo { get; set; }
		private static TelemetryClient TelClient = new TelemetryClient();
		private static bool UsesFile { get { return AppPath != null; } }
		private static bool UsesAI { get { return ApplicationInsightInstrumentationKey != null; } }
		private static readonly List<string> LogExclusions = new List<string>
		{
			"Error executing child request for handler 'System.Web.Mvc.HttpHandlerUtil+ServerExecuteHttpHandlerWrapper'",
			"Authentication: Could not find user ID."
		};

		public static void Initialise(string appPath)
		{
			BaseLog.Initialise(appPath);
		}

		public static void Initialise(string appPath, string instrumentationKey, string appName, string appDescription)
		{
			AppPath = appPath;

			if (UsesFile) BaseLog.Initialise(AppPath);

			if (instrumentationKey == null) return;

			ApplicationInsightInstrumentationKey = instrumentationKey;

			ApplicationMonitoringInfo = new Dictionary<string, string> { { "application-name", appName }, { "application-description", appDescription } };

			TelemetryConfiguration.Active.InstrumentationKey = ApplicationInsightInstrumentationKey;
		}

		public static void Write(string text, params object[] args)
		{
			if (UsesFile) BaseLog.Write(String.Format(text, args));
			if (UsesAI) TelClient.TrackTrace(text, SeverityLevel.Information, ApplicationMonitoringInfo);
		}

		public static void Error(Exception ex)
		{
			if (ex == null || String.IsNullOrEmpty(ex.Message)) return;

			if (LogExclusions.Any(error => ex.Message.ToLower().Contains(error.ToLower()))) return;

			if (UsesFile) BaseLog.Error(ex);
			if (UsesAI) TelClient.TrackException(ex, ApplicationMonitoringInfo);
			//SendEmail(ex);
		}

		public static void Error(Exception ex, string text, params object[] args)
		{
			if (ex == null || String.IsNullOrEmpty(ex.Message)) return;

			if (LogExclusions.Any(error => ex.Message.ToLower().Contains(error.ToLower()))) return;

			if (UsesFile) BaseLog.Error(ex, true, text, args);
			if (UsesAI) TelClient.TrackException(ex, ApplicationMonitoringInfo);
			//SendEmail(ex);
		}

		public static void Archive()
		{
			BaseLog.Archive();
		}

		private static void SendEmail(Exception exception)
		{
			try
			{
				string html = BaseLog.GetErrorEmailHtml(exception);
				if (String.IsNullOrEmpty(html)) return;

				EmailManager.InsertEmail("support@cortex.co.nz", null, null, null, "LEADING EDGE CURATOR SYSTEM ERROR", html, 0, 0, "ERROR", 0);
			}
			catch (Exception ex)
			{
				BaseLog.Error(ex);
			}
		}
	}
}
