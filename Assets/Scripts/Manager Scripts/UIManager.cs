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
                // ������ UIManager ������Ʈ�� ã�� �Ҵ�
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
    private GambleUIController _gambleUIController;
    [SerializeField]
    private CardSelector _cardSelector;
    [SerializeField]
    private MissionBossUIController _missionBossUIController;
    [SerializeField]
    private ColorUpgradeUIController _colorUpgradeUIController;
    [SerializeField]
    private GameMenuUIController _gameMenuUIController;
    [SerializeField]
    private GameDefeatUIController _gameDefeatUIController;

    [Header("PopUp UI")]
    [SerializeField]
    private Canvas _popupUICanvas;
    [SerializeField]
    private TowerDetailInfoUIController _towerDetailInfoUIController;

    [Header("Fade Text UI")]
    [SerializeField]
    private SystemMessage _systemMessage;
    [SerializeField]
    private GameObject _damageTakenTextPrefab;
    private ObjectPool<DamageTakenText> _damageTakenTextPool;
    [SerializeField]
    private GameObject _enemyDieRewardTextPrefab;
    private ObjectPool<EnemyDieRewardText> _enemyDieRewardTextPool;

    private readonly WaitForSecondsRealtime _gameStartFadeOutDelay = new(0.2f);

    /**************************** ������ ���̰� �� ���� ���� **********************************************
    public ScreenCover screenCover => _screenCover;
    public TowerInfomation towerInfomation => _towerInfomation;
    public TowerDetailInfoUIController towerDetailInfoUIController => _towerDetailInfoUIController;
    public GambleUIController gambleUIController => _gambleUIController;
    public CardSelector cardSelector => _cardSelector;
    public MissionBossUIController missionBossUIController => _missionBossUIController;
    public ColorUpgradeUIController colorUpgradeUIController => _colorUpgradeUIController;
    /*****************************************************************************************************/

    public ObjectPool<DamageTakenText> damageTakenTextPool => _damageTakenTextPool;
    public ObjectPool<EnemyDieRewardText> enemyDieRewardTextPool => _enemyDieRewardTextPool;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);    // �ڽ��� �ı�

        _damageTakenTextPool = new(_damageTakenTextPrefab, 10);
        _enemyDieRewardTextPool = new(_enemyDieRewardTextPrefab, 10);
    }

    public void GameStartScreenCoverFadeOut()
    {
        _screenCover.gameObject.SetActive(true);
        StartCoroutine(GameStartFadeOutDelayCoroutine());
    }
    private IEnumerator GameStartFadeOutDelayCoroutine()
    { 
        yield return _gameStartFadeOutDelay;
        _screenCover.FadeOut(Color.black, 1f);
    }

    public void ShowSystemMessage(SystemMessage.MessageType messageType)
    {
        if (_systemMessage.gameObject.activeSelf)
            _systemMessage.gameObject.SetActive(false);

        _systemMessage.Setup(messageType);
        _systemMessage.gameObject.SetActive(true);
        _systemMessage.StartAnimation();
    }

    public void ShowDamageTakenText(float damage, Transform target, DamageTakenType damageTakenType)
    {
        DamageTakenText damageTakenText = _damageTakenTextPool.GetObject();

        damageTakenText.transform.position = target.position + new Vector3(0f, 0.15f, 0f);
        damageTakenText.textMeshPro.text = Mathf.Round(damage).ToString();
        damageTakenText.damageTakenType = damageTakenType;

        damageTakenText.StartAnimation();
    }

    public void ShowEnemyDieRewardText(StringBuilder reward, Transform target)
    {
        EnemyDieRewardText enemyDieGoldText = _enemyDieRewardTextPool.GetObject();

        enemyDieGoldText.transform.position = target.position + new Vector3(0.1f, 0f, 0f);
        enemyDieGoldText.textMeshPro.text = reward.ToString();

        enemyDieGoldText.StartAnimation();

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
        _popupUICanvas.gameObject.SetActive(true);
        _towerDetailInfoUIController.gameObject.SetActive(true);
    }

    public void HideTowerDetailInfo()
    {
        _popupUICanvas.gameObject.SetActive(false);
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
 * ���ӿ��� ���Ǵ� ��� UI�� ������ ����Ѵ�.
 * 
 * Update : 2022/06/02 THU 16:30
 * ��� UI�� ������ ����ϴ� �������� �ٸ� UI Controller ��ũ��Ʈ���� ����ϵ��� ����.
 */