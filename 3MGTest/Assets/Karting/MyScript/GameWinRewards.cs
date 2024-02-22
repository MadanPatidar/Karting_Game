using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine.Audio;

public class GameWinRewards : MonoBehaviour
{
    public TextMeshProUGUI txtCoinCount;
    public GameObject mRewardObject;
    public TextMeshProUGUI txtReward;
    public GameObject mParent;

    int iReward = 50;

    private void Awake()
    {
        mRewardObject.SetActive(false);
    }

    void Start()
    {
        txtCoinCount.text = "" + LocalStorage.Coins;
        StartCoroutine(Rewards());
    }    

    IEnumerator Rewards()
    {
        yield return new WaitForSeconds(1);

        mRewardObject.SetActive(true);
        txtReward.text = "+" + iReward;
        mRewardObject.transform.parent = mParent.transform;
        mRewardObject.transform.DOLocalMove(new Vector3(0, 0, 0), 1.5f).SetDelay(0.75f).OnComplete(() => {

            mRewardObject.transform.DOScale(new Vector3(0, 0, 0), 1).OnComplete(() => {

                mRewardObject.SetActive(false);
                LocalStorage.Coins += iReward;
                txtCoinCount.text = "" + LocalStorage.Coins;

            });

        });
    }
}