using UnityEngine;
using DG.Tweening;

public enum COLLECTABLE
{
    NONE = 0,
    COINE = 1
}

public class Collectable : MonoBehaviour
{
    public GameObject Icon;
    public COLLECTABLE _COLLECTABLE;

    int iConReward = 1;

    void Start()
    {
        Icon.transform.DORotate(Quaternion.AngleAxis(180, Vector3.up).eulerAngles, 2.5f).SetDelay(Random.Range(0f,0.25f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        LocalStorage.Coins += iConReward;
        ObserverEventManager.RaiseCoinUpdate();
        Destroy(gameObject);
    }
}