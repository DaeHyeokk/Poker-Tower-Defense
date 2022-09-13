using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageUIManager : MonoBehaviour
{ 
    private static StageUIManager _instance;
    public static StageUIManager instance
    {
        get
        {
            if (_instance == null)
            {
                // ������ StageUIManager ������Ʈ�� ã�� �Ҵ�
                _instance = FindObjectOfType<StageUIManager>();
            }

            return _instance;
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

    private Queue<RewardText> _missionRewardTextQueue = new();
    private bool _isReadyShowMissionReward = true;

    /**************************** ������ ���̰� �� ���� ���� **********************************************
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
            Destroy(gameObject);    // �ڽ��� �ı�

        _systemMessagePool = new(_systemMessagePrefab, 5);
        _damageTakenTextPool = new(_damageTakenTextPrefab, 10);
        _rewardTextPool = new(_rewardTextPrefab, 10);

        SetupMissionTower();
    }

    private void Update()
    {
        RewardText missionRewardText;

        // �̼Ǹ����带 ȭ�鿡 ��� �غ� �Ǿ� �ְ�, ť�� ������� �̼Ǹ����尡 �ִ� ��� ����.
        if (_isReadyShowMissionReward && _missionRewardTextQueue.TryDequeue(out missionRewardText))
        {
            _isReadyShowMissionReward = false;
            missionRewardText.gameObject.SetActive(true);
            missionRewardText.StartAnimation();

            // �̼Ǹ����带 ȭ�鿡 ǥ���԰� ���ÿ� �̼� �Ϸ� ���� �÷���.
            SoundManager.instance.PlaySFX(SoundFileNameDictionary.missionCompleteSound);
        }
    }

    // MissionTower ������Ʈ�� Awake() �̺�Ʈ�޼ҵ带 ȣ���ϱ� ���� �̼� UI ��Ʈ�ѷ� ������Ʈ�� Ȱ��ȭ �ߴٰ� �ٽ� ��Ȱ��ȭ �Ѵ�.
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
}


/*
 * File : StageUIManager.cs
 * First Update : 2022/04/27 WED 23:10
 * ���ӿ��� ���Ǵ� ��� UI�� ������ ����Ѵ�.
 * 
 * Update : 2022/06/02 THU 16:30
 * ��� UI�� ������ ����ϴ� �������� �ٸ� UI Controller ��ũ��Ʈ���� ����ϵ��� ����.
 */