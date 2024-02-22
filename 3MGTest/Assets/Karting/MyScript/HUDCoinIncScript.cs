using TMPro;
using UnityEngine;

public class HUDCoinIncScript : MonoBehaviour
{
    public TextMeshProUGUI txtCoinCount;

    private void Awake()
    {
        gameObject.SetActive(LocalStorage.isEventEnable);
    }
    void Start()
    {
        txtCoinCount.text = "" + LocalStorage.Coins;  
    }

    private void OnEnable()
    {
        if (LocalStorage.isEventEnable)
        {
            ObserverEventManager.OnCoinUpdate += UpdateCoins;
        }
    }

    private void OnDisable()
    {
        if (LocalStorage.isEventEnable)
        {
            ObserverEventManager.OnCoinUpdate -= UpdateCoins;
        }
    }

    void UpdateCoins()
    {
        txtCoinCount.text = "" + LocalStorage.Coins;
    }
}