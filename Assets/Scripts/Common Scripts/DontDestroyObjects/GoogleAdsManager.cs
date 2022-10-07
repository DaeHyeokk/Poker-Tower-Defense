using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class GoogleAdsManager : MonoBehaviour
{
    public enum BannerAdSizeType { Standard, Adaptive }
    private static GoogleAdsManager s_instance;
    public static GoogleAdsManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = FindObjectOfType<GoogleAdsManager>();
            

            return s_instance;
        }
    }

    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;

    // 전면광고 노출딜레이 10분으로 설정
    private readonly float _interstitialAdDefaultDelay = 600f;
    private float _interstitialAdRemainDelay = 300f;

    // Delay가 0보다 작거나 같을 경우 False, 클 경우 True 리턴.
    private bool _isInterstitialDelayRemained => _interstitialAdRemainDelay > 0f;

    public InterstitialAd interstitialAd
    {
        get
        {
            if (_isInterstitialDelayRemained)
                return null;
            else
                return _interstitialAd;
        }
    }
    public RewardedAd rewardedAd => _rewardedAd;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            DontDestroyOnLoad(instance);

        MobileAds.Initialize(initStatus => { });

        // 플레이어가 광고제거 아이템을 구매하지 않았다면 전면광고와 배너광고를 로드한다.
        if (!IAPManager.instance.HadPurchashed(IAPManager.instance.productAdsRemove))
        {
            CreateAndLoadInterstitialAd();
            LoadBanner();
        }

        CreateAndLoadRewardedAd();
    }

    private void Update()
    {
        if (IAPManager.instance.HadPurchashed(IAPManager.instance.productAdsRemove))
        {
            // 플레이어가 광고 제거를 구매했는데 현재 배너광고가 로드된 상태라면 제거한다.
            if (_bannerView != null)
            {
                _bannerView.Hide();
                _bannerView.Destroy();
                _bannerView = null;
            }
            // 플레이어가 광고 제거를 구매했는데 현재 전면광고가 로드된 상태라면 제거한다.
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            return;
        }

        if (_isInterstitialDelayRemained)
            _interstitialAdRemainDelay -= Time.unscaledDeltaTime;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬이 시작되면 배너 광고를 활성화 한다.
        if(_bannerView != null) 
            _bannerView.Show();
    }

    private void LoadBanner()
    {
        string adUnitId;

#if UNITY_ANDROID
        //adUnitId = "ca-app-pub-3940256099942544/6300978111";  // 테스트
        adUnitId = "ca-app-pub-4057138975242646/2983804995";    // 배포용
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // 이전에 생성했던 배너광고 객체가 있으면 삭제한다.
        if (_bannerView != null)
            _bannerView.Destroy();

        AdSize adSize;

        adSize = AdSize.Banner;

        _bannerView = new BannerView(adUnitId, adSize, AdPosition.Top);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        _bannerView.LoadAd(request);
    }

    public void CreateAndLoadInterstitialAd()
    {
        string adUnitId;

#if UNITY_ANDROID
        //adUnitId = "ca-app-pub-3940256099942544/1033173712";  // 테스트
        adUnitId = "ca-app-pub-4057138975242646/7113895344";    // 배포용
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        adUnitId = "unexpected_platform";
#endif

        // 이전에 생성했던 전면광고 객체가 있으면 삭제하고 새로 생성한다. (iOS은 객체 재사용이 불가능하기 때문)
        if (_interstitialAd != null)
            _interstitialAd.Destroy();

        _interstitialAd = new InterstitialAd(adUnitId);

        _interstitialAd.OnAdClosed += (sender, args) => _interstitialAdRemainDelay = _interstitialAdDefaultDelay;
        _interstitialAd.OnAdClosed += (sender, args) => CreateAndLoadInterstitialAd();

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        _interstitialAd.LoadAd(request);
    }

    public void ReloadInterstitialAd()
    {       
        if (_interstitialAd != null)
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            _interstitialAd.LoadAd(request);
        }
    }

    private void CreateAndLoadRewardedAd()
    {
        string adUnitId;

#if UNITY_ANDROID
            //adUnitId = "ca-app-pub-3940256099942544/5224354917";    // 테스트
            adUnitId = "ca-app-pub-4057138975242646/8300367399";    // 배포용
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        if (_rewardedAd != null)
            _rewardedAd.Destroy();

        _rewardedAd = new RewardedAd(adUnitId);

        _rewardedAd.OnAdClosed += (sender, args) => CreateAndLoadRewardedAd();

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        _rewardedAd.LoadAd(request);
    }

    public void ReloadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            _rewardedAd.LoadAd(request);
        }
    }
}
