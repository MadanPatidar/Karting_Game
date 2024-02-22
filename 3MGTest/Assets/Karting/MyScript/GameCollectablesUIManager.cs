using UnityEngine;

public class GameCollectablesUIManager : MonoBehaviour
{
    public GameObject mCollectables;
    void Start()
    {
        mCollectables.SetActive(LocalStorage.isEventEnable);
    }   
}