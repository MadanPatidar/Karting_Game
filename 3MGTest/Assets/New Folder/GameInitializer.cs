using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour
{
	[SerializeField]
	private GameObject
		m_AmplitudeManager;

	[SerializeField]
	private GameObject
		m_EventManager;

	void Awake ()
	{			
		if (GameObject.Find("AmplitudeManager") == null && m_AmplitudeManager != null)
		{
			GameObject go = (GameObject)Instantiate(m_AmplitudeManager, Vector3.zero, Quaternion.identity);
			go.name = "AmplitudeManager";
		}

		if (GameObject.Find("EventManager") == null && m_EventManager != null)
		{
			GameObject go = (GameObject)Instantiate(m_EventManager, Vector3.zero, Quaternion.identity);
			go.name = "EventManager";
		}
	}

	void Start ()
	{
		Awake();
	}
}