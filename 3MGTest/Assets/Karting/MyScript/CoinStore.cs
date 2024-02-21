using TMPro;
using UnityEngine;

public class CoinStore : MonoBehaviour
{   
    public TextMeshProUGUI txtCoinCount;

    void Start()
    {
        txtCoinCount.text = ""+ Util.Coins;
    }
}