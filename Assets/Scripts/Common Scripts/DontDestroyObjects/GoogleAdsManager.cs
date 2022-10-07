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

    // ���鱤�� ��������� 10������ ����
    private readonly float _interstitialAdDefaultDelay = 600f;
    private float _interstitialAdRemainDelay = 300f;

    // Delay�� 0���� �۰ų� ���� ��� False, Ŭ ��� True ����.
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

        // �÷��̾ �������� �������� �������� �ʾҴٸ� ���鱤��� ��ʱ��� �ε��Ѵ�.
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
            // �÷��̾ ���� ���Ÿ� �����ߴµ� ���� ��ʱ��� �ε�� ���¶�� �����Ѵ�.
            if (_bannerView != null)
            {
                _bannerView.Hide();
                _bannerView.Destroy();
                _bannerView = null;
            }
            // �÷��̾ ���� ���Ÿ� �����ߴµ� ���� ���鱤�� �ε�� ���¶�� �����Ѵ�.
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
        // ���ο� ���� ���۵Ǹ� ��� ���� Ȱ��ȭ �Ѵ�.
        if(_bannerView != null) 
            _bannerView.Show();
    }

    private void LoadBanner()
    {
        string adUnitId;

#if UNITY_ANDROID
        //adUnitId = "ca-app-pub-3940256099942544/6300978111";  // �׽�Ʈ
        adUnitId = "ca-app-pub-4057138975242646/2983804995";    // ������
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // ������ �����ߴ� ��ʱ��� ��ü�� ������ �����Ѵ�.
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
        //adUnitId = "ca-app-pub-3940256099942544/1033173712";  // �׽�Ʈ
        adUnitId = "ca-app-pub-4057138975242646/7113895344";    // ������
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        adUnitId = "unexpected_platform";
#endif

        // ������ �����ߴ� ���鱤�� ��ü�� ������ �����ϰ� ���� �����Ѵ�. (iOS�� ��ü ������ �Ұ����ϱ� ����)
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
            //adUnitId = "ca-app-pub-3940256099942544/5224354917";    // �׽�Ʈ
            adUnitId = "ca-app-pub-4057138975242646/8300367399";    // ������
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
