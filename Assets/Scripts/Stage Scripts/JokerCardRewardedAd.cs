using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class JokerCardRewardedAd : MonoBehaviour
{
    [SerializeField]
    private GameObject _admobLoadingPanelObject;
    [SerializeField]
    private GameObject _admobLoadingFailedPanelObject;

    private RewardStringBuilder _rewardStringBuilder = new();

    private bool _isEarned;

    public bool isEarned => _isEarned;

    private void Awake()
    {
        _rewardStringBuilder.Set(0, 0, 2);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (_isEarned)
            {
                StageManager.instance.jokerCard += 2;
                StageManager.instance.HidePauseUIPanel();
                SoundManager.instance.ResumeBGM();
                //StageUIManager.instance.ShowJokerCardRewardedAdGetText("<color=\"red\">광고보상 획득 완료!</color>\n" + _rewardStringBuilder.ToString());
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable() => Destroy(this.gameObject);

    public void OnClickBonusRewardAdsButton()
    {
        ShowRewardAd();
    }

    private void ShowRewardAd()
    {
        RewardedAd rewardedAd = GoogleAdsManager.instance.rewardedAd;

        // 사용자가 동영상 시청에 대한 보상을 받아야 하는 경우 수행할 메소드 등록.
        // isEarned 값을 True로 바꿔준다.
        rewardedAd.OnUserEarnedReward += (sender, args) => _isEarned = true;

        // 광고를 로딩하기까지 3초를 기다리고 3초가 지나도 로딩이 되지 않으면 로딩 실패.
        StartCoroutine(WaitLoadingRewardedAd());
        IEnumerator WaitLoadingRewardedAd()
        {
            _admobLoadingPanelObject.SetActive(true);

            float timeOutDelay = 0f;
            bool isLoadFailed = false;

            while (!rewardedAd.IsLoaded() && !isLoadFailed)
            {
                yield return new WaitForSeconds(0.2f);
                timeOutDelay += 0.2f;

                if (timeOutDelay >= 3f)
                    isLoadFailed = true;
            }

            _admobLoadingPanelObject.SetActive(false);

            if (!isLoadFailed)
            {
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
