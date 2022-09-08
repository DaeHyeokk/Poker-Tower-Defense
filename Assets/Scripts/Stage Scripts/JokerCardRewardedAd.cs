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
                //StageUIManager.instance.ShowJokerCardRewardedAdGetText("<color=\"red\">������ ȹ�� �Ϸ�!</color>\n" + _rewardStringBuilder.ToString());
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

        // ����ڰ� ������ ��û�� ���� ������ �޾ƾ� �ϴ� ��� ������ �޼ҵ� ���.
        // isEarned ���� True�� �ٲ��ش�.
        rewardedAd.OnUserEarnedReward += (sender, args) => _isEarned = true;

        // ���� �ε��ϱ���� 3�ʸ� ��ٸ��� 3�ʰ� ������ �ε��� ���� ������ �ε� ����.
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
