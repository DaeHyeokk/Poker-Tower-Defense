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
    private bool _isBossWave = false;

    private readonly WaitForSeconds _waitOneSecond = new(1f);
    private readonly string _waveRewardString = "<color=\"white\">���̺� ���� ����</color>\n";
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

    private void Awake()
    {
        wave = 0;
        _waveRewardStringBuilder.Set(200, 0, 0);
        StartCoroutine(StartWaveSystemCoroutine());
    }

    private IEnumerator StartWaveSystemCoroutine()
    {
        // ���� ���� 10�� ��� �� ù ���̺갡 ���۵ȴ�.
        minute = 0;
        second = 10;
        while(second >= 0)
        {
            yield return _waitOneSecond;
     
            if (second == 0)
                break;
            else
                second--;
        }

        while (wave < _finalWave)
        {
            IncreaseWave();

            while (minute >= 0)
            {
                while (second >= 0)
                {
                    yield return _waitOneSecond;

                    if (second == 0)
                        break;
                    else
                        second--;
                }

                if (minute == 0)
                    break;
                else
                {
                    minute--;
                    second = 59;
                }
            }

            // ���� ���̺갡 ���� �� ���� ������ ������ ��� ����.
            if (_isBossWave && _enemySpawner.roundBossEnemy.gameObject.activeSelf)
                _enemySpawner.roundBossEnemy.OnMissing();
        }

        GameManager.instance.ClearGame();
    }

    private void IncreaseWave()
    {
        wave++;

        // ���� ��� �г�Ƽ�� Ȱ��ȭ ���¶�� ���� ���̺긦 1 ���ҽ�Ų��.
        if (_goldPenalty.gameObject.activeSelf)
            _goldPenalty.remainWave--;
        // ��� �г�Ƽ�� �ް��ִ� ���°� �ƴ϶�� �÷��̾�� ��带 �����Ѵ�.
        else
            GiveWaveReward();

        if (wave % 10 != 0)
        {
            if (_isBossWave) _isBossWave = false;
            _enemySpawner.SpawnEnemy(wave);

            minute = _defaultLimitMinute;
            second = _defaultLimitSecond;
        }
        // 10�� ��� ���̺�� ���� ���̺�.
        else
        {
            _isBossWave = true;
            _enemySpawner.SpawnRoundBoss(wave);

            minute = _bossLimitMinute;
            second = _bossLimitSecond;
        }

        _waveStartMessage.gameObject.SetActive(true);
    }

    private void GiveWaveReward()
    {
        // 200��� ����
        GameManager.instance.gold += 200;
        UIManager.instance.ShowWaveRewardText(_waveRewardString + _waveRewardStringBuilder.ToString());
    }
}
