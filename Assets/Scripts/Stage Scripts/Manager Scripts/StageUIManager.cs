using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageUIManager : MonoBehaviour
{ 
    private static StageUIManager s_instance;
    public static StageUIManager instance
    {
        get
        {
            if (s_instance == null)
            {
                // 씬에서 StageUIManager 오브젝트를 찾아 할당
                s_instance = FindObjectOfType<StageUIManager>();
            }

            return s_instance;
        }
    }

    [Header("Main UI Canvas")]
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

    [SerializeField]
    private TextMeshProUGUI tempText;

    private Queue<RewardText> _screenCenterRewardTextQueue = new();
    private bool _isReadyShowScreenCenterReward = true;

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
    }

    private void Update()
    {
        RewardText screenCenterRewardText;

        // 화면 중앙에 리워드를 띄울 준비가 되어 있고, 큐에 대기중인 리워드가 있는 경우 수행.
        if (_isReadyShowScreenCenterReward && _screenCenterRewardTextQueue.TryDequeue(out screenCenterRewardText))
        {
            _isReadyShowScreenCenterReward = false;
            screenCenterRewardText.gameObject.SetActive(true);
            screenCenterRewardText.StartAnimation();

            // 미션리워드를 화면에 표시함과 동시에 미션 완료 사운드 플레이.
            SoundManager.instance.PlaySFX(SoundFileNameDictionary.screenCenterRewardShowSound);
        }
    }

    // MissionTower 컴포넌트의 Awake() 이벤트메소드를 호출하기 위해 미션 UI 컨트롤러 오브젝트를 활성화 했다가 다시 비활성화 한다.
    private void SetupMissionTower()
    {
        _missionUIController.gameObject.SetActive(true);
        _missionUIController.gameObject.SetActive(false);
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

        damageTakenText.transform.position = target.position + new Vector3(0f, 0.42f, 0f);
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
        waveRewardText.transform.position = new Vector3(0f, 0.5f, 0f);
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

    public void ReservateScreenCenterReward(string reward)
    {
        RewardText rewardText = _rewardTextPool.GetObject();

        rewardText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        rewardText.transform.position = Vector3.zero;
        rewardText.textMeshPro.text = reward;
        rewardText.textObjectFadeAnimation.lerpSpeed = 0.4f;
        rewardText.movement2D.Stop();
        rewardText.gameObject.SetActive(false);

        _screenCenterRewardTextQueue.Enqueue(rewardText);
    }

    public void ReturnMissionRewardText(RewardText missionRewardText)
    {
        _isReadyShowScreenCenterReward = true;
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
}


/*
 * File : StageUIManager.cs
 * First Update : 2022/04/27 WED 23:10
 * 게임에서 사용되는 모든 UI의 변경을 담당한다.
 * 
 * Update : 2022/06/02 THU 16:30
 * 모든 UI의 변경을 담당하던 로직들을 다른 UI Controller 스크립트에서 담당하도록 변경.
 */