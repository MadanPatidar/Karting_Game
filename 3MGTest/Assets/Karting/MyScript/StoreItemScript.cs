using System;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemScript : MonoBehaviour
{
    public Item item;

    public Image imageColor;
    public Button ItemButton;
    public Text txtName;
    public Text txtPrice;
    public GameObject mTickMark;
    public Image imgLock;

    Action<GameObject> itemTapCallBack;

    void Awake()
    {

    }
    void Start()
    {
        ItemButton.onClick.AddListener(ItemButtonOnTap);
    }

    public void SetItem(Item _item, Action<GameObject> _itemTapCallBack)
    {
        item = _item;
        this.itemTapCallBack = _itemTapCallBack;
        txtName.text = item.Name;
        txtPrice.text = item.Price + " Coins";

        imageColor.color = Util.GetColorByCode(item.Color);

        bool bTick = _item.Color.Equals(LocalStorage.KartColor);        

        imgLock.gameObject.SetActive(!item.IsUnLock);
        mTickMark.SetActive(bTick);   
    }

    public void SelecteTick()
    {
        mTickMark.SetActive(true);
        imgLock.gameObject.SetActive(!item.IsUnLock);

    }
    public void UnSelecte()
    {
        mTickMark.SetActive(false);
        imgLock.gameObject.SetActive(!item.IsUnLock);
    }

    void ItemButtonOnTap()
    {
        if (!item.IsUnLock)
        {
            if (LocalStorage.Coins >= item.Price)
            {
                LocalStorage.Coins -= item.Price;
                item.IsUnLock = true;  
                imgLock.gameObject.SetActive(!item.IsUnLock);
            }
            else
            {
                Debug.Log(" <insufficient coins>");
                return;
            }
        }

        LocalStorage.KartColor = item.Color;
        if (itemTapCallBack != null)
            itemTapCallBack(gameObject);
    }
}