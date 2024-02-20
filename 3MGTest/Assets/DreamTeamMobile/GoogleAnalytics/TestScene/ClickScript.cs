using UnityEngine;
using UnityEngine.UI;
using DreamTeamMobile;

public class ClickScript : MonoBehaviour
{
    public Text text;

    void OnMouseDown()
    {
        if (gameObject.tag == "Event1")
        {
            GoogleAnalytics.Instance.TrackEvent("Event_Green");
            text.text = "An event GREEN has been sent to Google Analytics GA4";
        }
        else if (gameObject.tag == "Event2")
        {
            GoogleAnalytics.Instance.TrackEvent("Event_Blue");
            text.text = "An event BLUE has been sent to Google Analytics GA4";
        }
        else if (gameObject.tag == "Event3")
        {
            GoogleAnalytics.Instance.TrackEvent("Event_Red");
            text.text = "An event RED has been sent to Google Analytics GA4";
        }
    }
}
