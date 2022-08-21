using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{ 
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 UIManager 오브젝트를 찾아 할당
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    [Header("Main UI Canvas")]
    [SerializeField]
    private ScreenCover _screenCover;
    [SerializeField]
    private TowerInfomation _towerInfomation;
    [SerializeField]
    private TowerDetailInfoUIController _towerDetailInfoUIController;
    [SerializeField]
    private GambleUIController _gambleUIController;
    [SerializeField]
    private CardSelector _cardSelector;
    [SerializeField]
    private MissionUIController _missionUIController;
    [SerializeField]
    private MissionBossUIController _missionBossUIController;
    [SerializeField]
    private ColorUpgradeUIController _colorUpgradeUIController;
    [SerializeField]
    private GameMenuUIController _gameMenuUIController;
    [SerializeField]
    private GameDefeatUIController _gameDefeatUIController;

    [Header("Fade Text UI")]
    [SerializeField]
    private GameObject _systemMessagePrefab;
    private ObjectPool<SystemMessage> _systemMessagePool;
    [SerializeField]
    private GameObject _damageTakenTextPrefab;
    private ObjectPool<DamageTakenText> _damageTakenTextPool;
    [SerializeField]
    private GameObject _rewardTextPrefab;
    private ObjectPool<RewardText> _rewardTextPool;

    private Queue<RewardText> _missionRewardTextQueue = new();
    private bool _isReadyShowMissionReward = true;

    /**************************** 언젠가 쓰이게 될 수도 있음 **********************************************
    public ScreenCover screenCover => _screenCover;
    public TowerInfomation towerInfomation => _towerInfomation;
    public TowerDetailInfoUIController towerDetailInfoUIController => _towerDetailInfoUIController;
    public GambleUIController gambleUIController => _gambleUIController;
    public CardSelector cardSelector => _cardSelector;
    public MissionBossUIController missionBossUIController => _missionBossUIController;
    public ColorUpgradeUIController colorUpgradeUIController => _colorUpgradeUIController;
    ******************************************************************************************************/
    public MissionUIController missionUIController => _missionUIController;

    public ObjectPool<SystemMessage> systemMessagePool => _systemMessagePool;
    public ObjectPool<DamageTakenText> damageTakenTextPool => _damageTakenTextPool;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);    // 자신을 파괴

        _systemMessagePool = new(_systemMessagePrefab, 5);
        _damageTakenTextPool = new(_damageTakenTextPrefab, 10);
        _rewardTextPool = new(_rewardTextPrefab, 10);

        SetupMissionTower();
        StartCoroutine(ShowMissionRewardTextCoroutine());
    }

    private void SetupMissionTower()
    {
        _missionUIController.gameObject.SetActive(true);
        _missionUIController.gameObject.SetActive(false);
    }

    public void GameStartScreenCoverFadeOut()
    {
        _screenCover.gameObject.SetActive(true);
        _screenCover.FadeOut(Color.black, 0.5f);
    }

    public void ShowSystemMessage(SystemMessage.MessageType messageType)
    {
        SystemMessage systemMessage = _systemMessagePool.GetObject();

        systemMessage.Setup(messageType);
        systemMessage.StartAnimation();
    }

    public void ShowDamageTakenText(float damage, Transform target, DamageTakenType damageTakenType)
    {
        DamageTakenText damageTakenText = _damageTakenTextPool.GetObject();

        damageTakenText.transform.position = target.position + new Vector3(0f, 0.15f, 0f);
        damageTakenText.textMeshPro.text = Mathf.Round(damage).ToString();
        damageTakenText.damageTakenType = damageTakenType;

        damageTakenText.StartAnimation();
    }

    public void ShowEnemyDieRewardText(string reward, Transform target)
    {
        RewardText enemyDieRewardText = _rewardTextPool.GetObject();

        enemyDieRewardText.transform.localScale = Vector3.one;
        enemyDieRewardText.transform.position = target.position + new Vector3(0.1f, 0f, 0f);
        enemyDieRewardText.textMeshPro.text = reward;
        enemyDieRewardText.textObjectFadeAnimation.lerpSpeed = 1f;
        enemyDieRewardText.movement2D.Move();

        enemyDieRewardText.StartAnimation();
    }

    public void ShowWaveRewardText(string reward)
    {
        RewardText waveRewardText = _rewardTextPool.GetObject();

        waveRewardText.transform.localScale = new Vector3(2f, 2f, 2f);
        waveRewardText.transform.position = new Vector3(0f, 0.8f, 0f);
        waveRewardText.textMeshPro.text = reward;
        waveRewardText.textObjectFadeAnimation.lerpSpeed = 1f;
        waveRewardText.movement2D.Move();

        waveRewardText.StartAnimation();
    }

    public void ShowTowerSalesRewardText(string reward)
    {
        RewardText towerSalesRewardText = _rewardTextPool.GetObject();

        towerSalesRewardText.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        towerSalesRewardText.transform.position = Vector3.zero;
        towerSalesRewardText.textMeshPro.text = reward;
        towerSalesRewardText.textObjectFadeAnimation.lerpSpeed = 1f;
        towerSalesRewardText.movement2D.Move();

        towerSalesRewardText.StartAnimation();
    }

    public void reservateMissionReward(string reward)
    {
        RewardText missionRewardText = _rewardTextPool.GetObject();

        missionRewardText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        missionRewardText.transform.position = Vector3.zero;
        missionRewardText.textMeshPro.text = reward;
        missionRewardText.textObjectFadeAnimation.lerpSpeed = 0.4f;
        missionRewardText.movement2D.Stop();
        missionRewardText.gameObject.SetActive(false);

        _missionRewardTextQueue.Enqueue(missionRewardText);
    }

    private IEnumerator ShowMissionRewardTextCoroutine()
    {
        RewardText missionRewardText;

        while(true)
        {
            // 미션리워드를 화면에 띄울 준비가 되어 있고, 큐에 대기중인 미션리워드가 있는 경우 수행.
            if (_isReadyShowMissionReward && _missionRewardTextQueue.TryDequeue(out missionRewardText))
            {
                _isReadyShowMissionReward = false;
                missionRewardText.gameObject.SetActive(true);
                missionRewardText.StartAnimation();

                // 미션리워드를 화면에 표시함과 동시에 미션 완료 사운드 플레이.
                SoundManager.instance.PlaySFX("Mission Complete Sound");
            }

            yield return null;
        }
    }

    public void ShowMissionRewardText(string reward)
    {
        RewardText missionRewardText = _rewardTextPool.GetObject();

        missionRewardText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        missionRewardText.transform.position = Vector3.zero;
        missionRewardText.textMeshPro.text = reward;
        missionRewardText.textObjectFadeAnimation.lerpSpeed = 0.5f;
        missionRewardText.movement2D.Stop();
        missionRewardText.gameObject.SetActive(false);

    }

    public void ReturnMissionRewardText(RewardText missionRewardText)
    {
        _isReadyShowMissionReward = true;
        _rewardTextPool.ReturnObject(missionRewardText);
    }

    public void ShowTowerInfo(Tower tower)
    {
        _towerInfomation.Setup(tower);

        _gambleUIController.gameObject.SetActive(false);
        _towerInfomation.gameObject.SetActive(true);
    }

    public void HideTowerInfo()
    {
        _towerInfomation.gameObject.SetActive(false);
        _gambleUIController.gameObject.SetActive(true);
    }

    public void ShowTowerDetailInfo()
    {
        _towerDetailInfoUIController.gameObject.SetActive(true);
    }

    public void HideTowerDetailInfo()
    {
        _towerDetailInfoUIController.gameObject.SetActive(false);
    }

    public void ShowGameMenu()
    {
        _gameMenuUIController.gameObject.SetActive(true);
    }
    public void HideGameMenu()
    {
        _gameMenuUIController.gameObject.SetActive(false);
    }

    public void ShowGameDefeatPanel()
    {
        _gameDefeatUIController.gameObject.SetActive(true);
    }
}


/*
 * File : UIManager.cs
 * First Update : 2022/04/27 WED 23:10
 * 게임에서 사용되는 모든 UI의 변경을 담당한다.
 * 
 * Update : 2022/06/02 THU 16:30
 * 모든 UI의 변경을 담당하던 로직들을 다른 UI Controller 스크립트에서 담당하도록 변경.
 */