using System;
using System.Collections;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private int _defaultLimitMinute;
    [SerializeField]
    private int _defaultLimitSecond;
    [SerializeField]
    private int _bossLimitMinute;
    [SerializeField]
    private int _bossLimitSecond;
    [SerializeField]
    private WaveSystemUIController _waveSystemUIController;
    [SerializeField]
    private WaveStartMessage _waveStartMessage;
    [SerializeField]
    private GoldPenalty _goldPenalty;
    [SerializeField]
    private EnemySpawner _enemySpawner;

    private RewardStringBuilder _waveRewardStringBuilder = new();
    private int _wave;
    private int _minute;
    private int _second;
    private int _finalWave = 40;

    private float _oneSecond = 1f;

    private bool _isBossWave;

    private readonly string _waveRewardString = "<color=\"white\">웨이브 보상 지급</color>\n";

    public event Action onWaveStart;
    public event Action onBossWaveStart;

    public GoldPenalty goldPenalty => _goldPenalty;

    public int wave
    {
        get => _wave;
        set
        {
            _wave = value;
            _waveSystemUIController.SetWaveText(value);
        }
    }

    public int minute
    {
        get => _minute;
        set
        {
            _minute = value;
            _waveSystemUIController.SetMinuteText(value);
        }
    }

    public int second
    {
        get => _second;
        set
        {
            _second = value;
            _waveSystemUIController.SetSecondText(value);
        }
    }

    public bool isBossWave => _isBossWave;
    public bool isFinalWave => _finalWave == _wave ? true : false;

    private void Awake()
    {
        wave = 0;
        _waveRewardStringBuilder.Set(200, 0);

        // 게임 시작 10초 경과 후 첫 웨이브가 시작된다.
        minute = 0;
        second = 10;
    }
    
    private void Update()
    {
        // 웨이브가 최종 웨이브를 넘어서면 아무 동작도 수행하지 않는다.
        if (_wave > _finalWave)
            return;

        _oneSecond -= Time.deltaTime;

        if (_oneSecond <= 0f)
        {
            _oneSecond = 1f;

            if (second == 0)
            {
                if (minute <= 0)
                {
                    // 보스 웨이브가 끝날 때 까지 보스를 못잡을 경우 실행. (게임 패배)
                    if (_isBossWave && _enemySpawner.roundBossEnemy.gameObject.activeSelf)
                        _enemySpawner.roundBossEnemy.OnMissing();
                    else
                        IncreaseWave();
                }
                else
                {
                    minute--;
                    second = 59;
                }
            }
            else
                second--;
        }
    }
    
    private void IncreaseWave()
    {
        wave++;

        // 웨이브가 최종 웨이브를 넘어서면 아무 동작도 수행하지 않는다.
        if (_wave > _finalWave)
            return;

        // 웨이브 시작 시 웨이브 시작 사운드를 재생한다.
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.waveStartSound);

        // 현재 골드 패널티가 활성화 상태라면 남은 웨이브를 1 감소시킨다.
        if (_goldPenalty.gameObject.activeSelf)
            _goldPenalty.remainWave--;
        // 골드 패널티를 받고있는 상태가 아니라면 플레이어에게 골드를 지급한다.
        else
            GiveWaveReward();

        if (wave % 10 != 0)
        {
            // 웨이브가 시작됐음을 알리는 메세지를 보낸다.(?)
            if (onWaveStart != null)
                onWaveStart();

            // 이전 웨이브가 보스웨이브였을 경우 수행.
            if (_isBossWave)
            {
                _isBossWave = false;
                // 보스 웨이브 전용 BGM에서 메인 BGM으로 전환한다.
                SoundManager.instance.PlayBGM(SoundFileNameDictionary.mainBGM);
            }
            _enemySpawner.SpawnEnemy();

            minute = _defaultLimitMinute;
            second = _defaultLimitSecond;
        }
        // 10의 배수 웨이브는 보스 웨이브.
        else
        {
            // 보스웨이브가 시작됐음을 알리는 메세지를 보낸다.(?)
            if (onBossWaveStart != null)
                onBossWaveStart();

            // 보스웨이브가 시작되면 보스 웨이브 전용 BGM을 재생한다.
            SoundManager.instance.PlayBGM(SoundFileNameDictionary.bossWaveBGM);

            _isBossWave = true;
            _enemySpawner.SpawnRoundBoss();

            minute = _bossLimitMinute;
            second = _bossLimitSecond;
        }

        _waveStartMessage.gameObject.SetActive(true);
    }

    private void GiveWaveReward()
    {
        // 200골드 지급
        StageManager.instance.gold += 200;
        StageUIManager.instance.ShowWaveRewardText(_waveRewardString + _waveRewardStringBuilder.ToString());
    }
}
