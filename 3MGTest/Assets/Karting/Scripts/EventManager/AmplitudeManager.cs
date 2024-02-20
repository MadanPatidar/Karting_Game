using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplitudeManager : MonoBehaviour
{
    public static AmplitudeManager Instance;

    Amplitude amplitude = null;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        amplitude = Amplitude.getInstance();
        amplitude.init("8970b385e9a718ad40ae35a5f3b640af");
    }

    public void TrackEvent(string sEvent)
    {
        if (amplitude != null)
        {       
            Debug.LogError("TrackEvent <amplitude> : " + amplitude + " <Event> : "+ sEvent);
            amplitude.logEvent(sEvent);
        }    
    }

}
