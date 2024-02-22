using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class SeasonalEvent : MonoBehaviour
{
    public Image imgBg;
    public CanvasGroup canvasGroup;

    public Color colorEnable;
    public Color colorDisable;

    void Start()
    {

        if (LocalStorage.isEventEnable)
        {
            SetEventEnable();
        }
        else
        {
            SetEventDisable();
        }
    }

    private void OnEnable()
    {
        ObserverEventManager.OnEventUpdate += OnEventUpdate;
    }

    private void OnDisable()
    {
        ObserverEventManager.OnEventUpdate -= OnEventUpdate;
    }

    void OnEventUpdate()
    {
        if (LocalStorage.isEventEnable)
        {
            SetEventEnable();
        }
        else
        {
            SetEventDisable();
        }
    }

    void SetEventEnable()
    {
        canvasGroup.DOFade(1f, 0.5f);
        imgBg.color = colorEnable;
    }

    void SetEventDisable()
    {
        canvasGroup.DOFade(0.75f, 0.5f);
        imgBg.color = colorDisable;
    }
}