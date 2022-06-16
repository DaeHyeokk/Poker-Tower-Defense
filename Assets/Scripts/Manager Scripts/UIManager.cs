using System.Collections;
using System.Collections.Generic;
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
    private TowerInfomation _towerInfomation;
    [SerializeField]
    private GambleUIController _gambleUIController;
    [SerializeField]
    private MissionBossUIController _missionBossUIController;
    [SerializeField]
    private ColorUpgradeUIController _colorUpgradeUIController;
    [SerializeField]
    private GameMenuUIController _gameMenuUIController;

    [Header("Fade Text UI")]
    [SerializeField]
    private GameObject _systemMessagePrefab;
    private ObjectPool<SystemMessage> _systemMessagePool;
    [SerializeField]
    private GameObject _damageTakenTextPrefab;
    private ObjectPool<DamageTakenText> _damageTakenTextPool;


    public TowerInfomation towerInfomation => _towerInfomation;
    public GambleUIController gambleUIController => _gambleUIController;
    public MissionBossUIController missionBossUIController => _missionBossUIController;
    public ColorUpgradeUIController colorUpgradeUIController => _colorUpgradeUIController;

    public ObjectPool<SystemMessage> systemMessagePool => _systemMessagePool;
    public ObjectPool<DamageTakenText> damageTakenTextPool => _damageTakenTextPool;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);    // 자신을 파괴

        _systemMessagePool = new(_systemMessagePrefab, 5);
        _damageTakenTextPool = new(_damageTakenTextPrefab, 10);
    }

    public void ShowSystemMessage(string message)
    {
        SystemMessage systemMessage = _systemMessagePool.GetObject();

        systemMessage.transform.position = Vector3.zero;
        systemMessage.messageText.text = message;
    }

    public void ShowDamageTakenText(float damage, Transform target, DamageTakenType damageTakenType)
    {
        DamageTakenText damageTakenText = _damageTakenTextPool.GetObject();

        damageTakenText.transform.position = target.position;
        damageTakenText.damageTakenText.text = Mathf.Round(damage).ToString();
        damageTakenText.damageTakenType = damageTakenType;
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

    public void ShowGameMenu()
    {
        _gameMenuUIController.gameObject.SetActive(true);
    }
    public void HideGameMenu()
    {
        _gameMenuUIController.gameObject.SetActive(false);
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