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

    private readonly string _waveRewardString = "<color=\"white\">���̺� ���� ����</color>\n";

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

        // ���� ���� 10�� ��� �� ù ���̺갡 ���۵ȴ�.
        minute = 0;
        second = 10;
    }
    
    private void Update()
    {
        // ���̺갡 ���� ���̺긦 �Ѿ�� �ƹ� ���۵� �������� �ʴ´�.
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
                    // ���� ���̺갡 ���� �� ���� ������ ������ ��� ����. (���� �й�)
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

        // ���̺갡 ���� ���̺긦 �Ѿ�� �ƹ� ���۵� �������� �ʴ´�.
        if (_wave > _finalWave)
            return;

        // ���̺� ���� �� ���̺� ���� ���带 ����Ѵ�.
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.waveStartSound);

        // ���� ��� �г�Ƽ�� Ȱ��ȭ ���¶�� ���� ���̺긦 1 ���ҽ�Ų��.
        if (_goldPenalty.gameObject.activeSelf)
            _goldPenalty.remainWave--;
        // ��� �г�Ƽ�� �ް��ִ� ���°� �ƴ϶�� �÷��̾�� ��带 �����Ѵ�.
        else
            GiveWaveReward();

        if (wave % 10 != 0)
        {
            // ���̺갡 ���۵����� �˸��� �޼����� ������.(?)
            if (onWaveStart != null)
                onWaveStart();

            // ���� ���̺갡 �������̺꿴�� ��� ����.
            if (_isBossWave)
            {
                _isBossWave = false;
                // ���� ���̺� ���� BGM���� ���� BGM���� ��ȯ�Ѵ�.
                SoundManager.instance.PlayBGM(SoundFileNameDictionary.mainBGM);
            }
            _enemySpawner.SpawnEnemy();

            minute = _defaultLimitMinute;
            second = _defaultLimitSecond;
        }
        // 10�� ��� ���̺�� ���� ���̺�.
        else
        {
            // �������̺갡 ���۵����� �˸��� �޼����� ������.(?)
            if (onBossWaveStart != null)
                onBossWaveStart();

            // �������̺갡 ���۵Ǹ� ���� ���̺� ���� BGM�� ����Ѵ�.
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
        // 200��� ����
        StageManager.instance.gold += 200;
        StageUIManager.instance.ShowWaveRewardText(_waveRewardString + _waveRewardStringBuilder.ToString());
    }
}
