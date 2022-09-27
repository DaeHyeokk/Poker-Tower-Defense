using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BonusReward : MonoBehaviour
{
    [SerializeField]
    private GameObject _bonusRewardAdsButtonObject;
    [SerializeField]
    private GameObject _bonusRewardActivateTextObject;
    [SerializeField]
    private GameObject _bonusRewardInfoCanvasObject;
    [SerializeField]
    private GameObject _bonusRewardInfoPanelObject;
    [SerializeField]
    private GameObject _admobLoadingPanelObject;
    [SerializeField]
    private GameObject _admobLoadingFailedPanelObject;

    public bool isBonusRewardActived
    {
        get => GameManager.instance.playerGameData.isBonusRewardActived;
        set => GameManager.instance.playerGameData.isBonusRewardActived = value;
    }

    private void Start()
    {
        SetBonusRewardPanelUI();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (isBonusRewardActived)
                SetBonusRewardPanelUI();

            SoundManager.instance.ResumeBGM();
        }
    }

    private void SetBonusRewardPanelUI()
    {
        // 로그인한 플레이어에게만 보상형 광고 UI를 보여줌.
        if (Social.localUser.authenticated)
        {
            // 보너스리워드가 활성화 된 상태일 때 수행.
            if (isBonusRewardActived)
            {
                _bonusRewardAdsButtonObject.SetActive(false);
                _bonusRewardActivateTextObject.SetActive(true);
            }
            else
            {
                _bonusRewardAdsButtonObject.SetActive(true);
                _bonusRewardActivateTextObject.SetActive(false);
            }
        }
        // 로그인 하지 않은 플레이어에게는 보여주지 않음.
        else
        {
            _bonusRewardAdsButtonObject.SetActive(false);
            _bonusRewardActivateTextObject.SetActive(false);
        }
    }

    public void OnClickBonusRewardAdsButton()
    {
        ShowRewardAd();
    }

    private void ShowRewardAd()
    {
        RewardedAd rewardedAd = GoogleAdsManager.instance.rewardedAd;

        // 사용자가 동영상 시청에 대한 보상을 받아야 하는 경우 수행할 메소드 등록.
        // isBonusRewarded 값을 True로 바꿔준다.
        rewardedAd.OnUserEarnedReward += (sender, args) => isBonusRewardActived = true;

        // 광고를 로딩하기까지 3초를 기다리고 3초가 지나도 로딩이 되지 않으면 로딩 실패.
        StartCoroutine(WaitLoadingRewardedAd());
        IEnumerator WaitLoadingRewardedAd()
        {
            _admobLoadingPanelObject.SetActive(true);

            float timeOutDelay = 0f;
            bool isLoadFailed = false;

            while (!rewardedAd.IsLoaded() && !isLoadFailed)
            {
                yield return new WaitForSeconds(0.1f);
                timeOutDelay += 0.1f;

                if (timeOutDelay >= 3f)
                    isLoadFailed = true;
            }

            _admobLoadingPanelObject.SetActive(false);

            if (!isLoadFailed)
            {
                _bonusRewardInfoCanvasObject.SetActive(false);

                SoundManager.instance.PauseBGM();
                rewardedAd.Show();
            }
            else
            {
                _admobLoadingFailedPanelObject.SetActive(true);
                GoogleAdsManager.instance.ReloadRewardedAd();
            }
        }
    }
}
