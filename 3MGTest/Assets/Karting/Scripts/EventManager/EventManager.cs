using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class EventManager : MonoBehaviour
    {
        public bool IsTreckGoogleAnalyticsEvent = true;
        public bool IsTreckAmplitudeEvent = true;

        public static EventManager Instance;

        private void Awake()
        {
            Instance = this;
        }
        public void TrackEvent(string sEvent)
        {

#if UNITY_EDITOR
            Debug.LogError(" TrackEvent << EVENT >> : " + sEvent);
         //   return;
#endif
           
            if (IsTreckGoogleAnalyticsEvent)
            {
                DreamTeamMobile.GoogleAnalytics.Instance.TrackEvent(sEvent);
            }

            if (IsTreckAmplitudeEvent)
            {
                AmplitudeManager.Instance.TrackEvent(sEvent);
            }
        }
    }

