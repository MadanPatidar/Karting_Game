using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour
{
    [Header("Closed-Button")]
    public Button ClosedButton;

    [Space]
    [Header("Panel")]
    public GameObject mBallPanel;

    [Space]
    [Header("Grid")]
    public GameObject mBallGrid;


    [Space]
    [Header("Prefab")]
    public GameObject prfStore;

    [Space]
    [Header("Private")]
    List<GameObject> listBalls = new List<GameObject>();

    void Start()
    {
        ClosedButton.onClick.AddListener(ClosedButtonOnTap);
        DisplayStore();
    }


    Action<bool> CallBack = null;
    public void SetStoreUIUpdateCall(Action<bool> callBack)
    {
        CallBack = callBack;
    }

    void DisplayStore()
    {
        DisplayItems(StoreManager.AllItems, mBallGrid.transform);
    }

    void DisplayItems(List<Item> Items, Transform trParent)
    {
        foreach (Item item in Items)
        {
            GameObject go = Instantiate(prfStore, trParent);
            StoreItemScript storeItemScript = go.GetComponent<StoreItemScript>();
            storeItemScript.SetItem(item, BallItemOnTap);
            listBalls.Add(go);
        }
    }

    void BallItemOnTap(GameObject go)
    {
        foreach (GameObject _go in listBalls)
        {
            StoreItemScript storeItemScript = _go.GetComponent<StoreItemScript>();
            storeItemScript.UnSelecte();
        }

        StoreItemScript _storeItemScript = go.GetComponent<StoreItemScript>();
        _storeItemScript.SelecteTick();
    }

    void ClosedButtonOnTap()
    {
        if (CallBack != null)
            CallBack(true);

        gameObject.SetActive(false);        
    }
}