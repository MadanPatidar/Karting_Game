using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public Selectable defaultSelection;
    public Button shopButton;
    public GameObject mStoreScreen;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
        shopButton.onClick.AddListener(ShopButtonOnTap);
    }

    void ShopButtonOnTap()
    {
        mStoreScreen.SetActive(true);       
    }

    void LateUpdate()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.GetButtonDown(GameConstants.k_ButtonNameSubmit)
                || Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal) != 0
                || Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
            {
                EventSystem.current.SetSelectedGameObject(defaultSelection.gameObject);
            }
        }
    }
}
