using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;

namespace DreamTeamMobile
{
    /// <summary>
    /// Implementation of Google Analytics GA4 Measurement Protocol
    /// https://developers.google.com/analytics/devguides/collection/protocol/ga4/reference?client_type=gtag#payload
    /// </summary>
    public class GoogleAnalyticsGA4Api
    {
        private const string GA4ApiEndpoint = "https://www.google-analytics.com/mp/collect";
        private readonly string _measurementId;
        private readonly string _apiSecret;
        private readonly string _deviceId;
        private readonly int _defaultEngagementTimeInSec;
        private readonly string _sessionId;

        public GoogleAnalyticsGA4Api(string measurementId, string apiSecret, string deviceId, int defaultEngagementTimeInSec = 100)
        {
            if (string.IsNullOrWhiteSpace(measurementId))
                throw new ArgumentNullException(nameof(measurementId));

            if (string.IsNullOrWhiteSpace(apiSecret))
                throw new ArgumentNullException(nameof(apiSecret));

            _measurementId = measurementId;
            _apiSecret = apiSecret;
            _deviceId = deviceId;
            _defaultEngagementTimeInSec = defaultEngagementTimeInSec;
            _sessionId = Guid.NewGuid().ToString();
        }

        public void TrackEvent(string name)
        {
            Track(name);            
        }

        public void TrackEventWihtParam(string name, Dictionary<string, string> dicString, Dictionary<string, int> dicNum)
        {
            if (dicString == null)
                dicString = new Dictionary<string, string>();

            if (dicNum == null)
                dicNum = new Dictionary<string, int>();

            dicString.Add("session_id", _sessionId);
            dicNum.Add("engagement_time_msec", _defaultEngagementTimeInSec);

            string sParams = string.Empty;
            foreach (string sKey in dicString.Keys)
            {
                string sValue = dicString[sKey];

                if (sParams.Equals(string.Empty))
                    sParams = string.Format("\"{0}\":\"{1}\"", sKey, sValue);
                else
                    sParams = string.Format("{0},\"{1}\":\"{2}\"", sParams, sKey, sValue);
            }

          //  Debug.LogError("sParams : " + sParams);

            foreach (string sKey in dicNum.Keys)
            {
                int sValue = dicNum[sKey];

                if (sParams.Equals(string.Empty))
                    sParams = string.Format("\"{0}\":{1}", sKey, sValue);
                else
                    sParams = string.Format("{0},\"{1}\":{2}", sParams, sKey, sValue);
            }
           // Debug.LogError("sParams : " + sParams);
           
            string s1 = "{\"client_id\":\"";
            string s2 = "\",\"events\":[{\"name\":\"";
            string s3 = "\",\"params\": {";
            string s4 = "}}]}";

            string sJsonData = string.Format("{0}{1}{2}{3}{4}{5}{6}", s1, _deviceId, s2, name, s3, sParams, s4);

            //Debug.LogError("sJsonData : " + sJsonData);

            Track(name, sJsonData);
        }

        private void Track(string name, string sJsonData = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("category");

            try
            {
                var postDataString ="";

                if (sJsonData == null)
                {
                    var postData = new GA4Data
                    {
                        client_id = _deviceId,
                        events = new GA4DataEvent[]
                        {
                        new GA4DataEvent
                        {
                            name = Uri.EscapeUriString(name),
                            @params = new GA4DataEventParams
                            {
                                session_id = _sessionId,
                                engagement_time_msec = _defaultEngagementTimeInSec
                            }
                        }
                        }
                    };
                    postDataString = JsonUtility.ToJson(postData);
                }
                else
                {
                    postDataString = sJsonData;
                }

                Debug.Log("[Event] postDataString -> : " + postDataString);               

                var url = $"{GA4ApiEndpoint}?measurement_id={_measurementId}&api_secret={_apiSecret}";
                var stringContent = new StringContent(postDataString, Encoding.UTF8, "application/json");
                new HttpClient().PostAsync(url, stringContent)
                    .ContinueWith(t =>
                    {
                        var response = t.Result;
                        if (!response.IsSuccessStatusCode)
                            UnityEngine.Debug.Log($"[DTM GA4] Failed to submit GA event: {response.StatusCode}");
                    });
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log($"[DTM GA4] Failed to track GA event: {ex}");
            }
        }
    }
}

[Serializable]
public class GA4Data
{
    public string client_id;
    public GA4DataEvent[] events;
}

[Serializable]
public class GA4DataEvent
{
    public string name;
    public GA4DataEventParams @params;
}

[Serializable]
public class GA4DataEventParams
{
    public string session_id;
    public int engagement_time_msec;
}