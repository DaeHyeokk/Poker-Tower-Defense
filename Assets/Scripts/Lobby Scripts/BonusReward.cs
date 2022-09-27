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
        // �α����� �÷��̾�Ը� ������ ���� UI�� ������.
        if (Social.localUser.authenticated)
        {
            // ���ʽ������尡 Ȱ��ȭ �� ������ �� ����.
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
        // �α��� ���� ���� �÷��̾�Դ� �������� ����.
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

        // ����ڰ� ������ ��û�� ���� ������ �޾ƾ� �ϴ� ��� ������ �޼ҵ� ���.
        // isBonusRewarded ���� True�� �ٲ��ش�.
        rewardedAd.OnUserEarnedReward += (sender, args) => isBonusRewardActived = true;

        // ���� �ε��ϱ���� 3�ʸ� ��ٸ��� 3�ʰ� ������ �ε��� ���� ������ �ε� ����.
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
