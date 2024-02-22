using TMPro;
using UnityEngine;

public class CoinStore : MonoBehaviour
{
    public TextMeshProUGUI txtCoinCount;
    void Start()
    {
        txtCoinCount.text = "" + LocalStorage.Coins;
        InvokeRepeating("UpdateCoinsCount", 1, 1);
    }

    void UpdateCoinsCount()
    {
        txtCoinCount.text = "" + LocalStorage.Coins;
    }
}