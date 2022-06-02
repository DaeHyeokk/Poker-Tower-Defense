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

    [Header("Tower Infomation UI")]
    [SerializeField]
    private TowerInfomation _towerInfo;
    [SerializeField]
    private GameObject _cardGambleCanvas;
    [SerializeField]
    private GameObject _gambleButtonCanvas;

    [Header("Fade Text UI")]
    [SerializeField]
    private GameObject _systemMessagePrefab;
    private ObjectPool<SystemMessage> _systemMessagePool;
    [SerializeField]
    private GameObject _damageTakenTextPrefab;
    private ObjectPool<DamageTakenText> _damageTakenTextPool;

    public ObjectPool<SystemMessage> systemMessagePool => _systemMessagePool;
    public ObjectPool<DamageTakenText> damageTakenTextPool => _damageTakenTextPool;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);    // 자신을 파괴
            return;
        }

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
        _towerInfo.Setup(tower);

        _cardGambleCanvas.SetActive(false);
        _gambleButtonCanvas.SetActive(false);
        _towerInfo.gameObject.SetActive(true);
    }

    public void HideTowerInfo()
    {
        _towerInfo.gameObject.SetActive(false);
        _cardGambleCanvas.SetActive(true);
        _gambleButtonCanvas.SetActive(true);
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