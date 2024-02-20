using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DreamTeamMobile
{
    public class GoogleAnalytics
    {
        private GoogleAnalyticsSettings _settings;
        private readonly GoogleAnalyticsGA4Api _gaClient;
        private static GoogleAnalytics _instance;

        public static GoogleAnalytics Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GoogleAnalytics();
                return _instance;
            }
        }

        private GoogleAnalytics()
        {
            _settings = Resources.LoadAll<GoogleAnalyticsSettings>("GoogleAnalyticsSettings").FirstOrDefault();
            EnsureValidSettings();

            _gaClient = new GoogleAnalyticsGA4Api(_settings.GA4MeasurementId, _settings.GA4StreamApiSecret, SystemInfo.deviceUniqueIdentifier);

            if (_settings.TrackApplicationErrors)
            {
                Application.logMessageReceived += Application_logMessageReceived;
            }

            if (_settings.TrackSceneChangeEvents)
            {
                SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            }
        }

        public void TrackError(Exception ex, [CallerMemberName] string memberName = "")
        {
            if (ex == null)
                return;

            EnsureValidSettings();
            Debug.LogError($"[DTM GA4] Tracking error: {ex}");
            _gaClient.TrackEvent($"Error_{memberName}");
        }

        public void TrackEvent(string eventName)
        {
            EnsureValidSettings();
            Debug.LogError($"[DTM GA4] Tracking event: {eventName}");
            _gaClient.TrackEvent(eventName);
        }

        public void TrackEventWithParam(string eventName, Dictionary<string, string> dicString, Dictionary<string, int> dicNum)
        {
            EnsureValidSettings();
            Debug.Log($"[DTM GA4] Tracking event: {eventName}");
            _gaClient.TrackEventWihtParam(eventName, dicString, dicNum);
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            TrackEvent($"SceneLoaded_{arg1.name}");
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType logType)
        {
            if (logType == LogType.Exception || logType == LogType.Error)
            {
                var ex = new Exception(condition);
                ex.Data[nameof(stackTrace)] = stackTrace;
                ex.Data[nameof(logType)] = logType;
                TrackError(ex);
            }
        }

        private void EnsureValidSettings()
        {
            if (_settings == null)
            {
                throw new InvalidOperationException("GoogleAnalytics settings are not found. Please use the Window/GoogleAnalytics menu option to generate the settings.");
            }

            if (string.IsNullOrWhiteSpace(_settings.GA4MeasurementId))
            {
                throw new InvalidOperationException("GoogleAnalytics GA4MeasurementId is not configured properly. Please use the Window/GoogleAnalytics menu option to locate and configure the settings.");
            }


            if (string.IsNullOrWhiteSpace(_settings.GA4StreamApiSecret))
            {
                throw new InvalidOperationException("GoogleAnalytics GA4StreamApiSecret is not configured properly. Please use the Window/GoogleAnalytics menu option to locate and configure the settings.");
            }
        }
    }
}