using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionBossEnemy : FieldBossEnemy
{
    [SerializeField]
    private int _bossLevel; 
    [SerializeField]
    private int _limitTime;
    [SerializeField]
    private TextMeshProUGUI _limitTimeText;

    private GoldPenalty _goldPenalty;
    private WaitForSeconds _waitOneSecond = new(1f);

    protected override void Awake()
    {
        base.Awake();
        _goldPenalty = FindObjectOfType<WaveSystem>().goldPenalty;
    }

    public override void Setup(EnemyData enemyData)
    {
        base.Setup(enemyData);
        StartCoroutine(LimitTimerCoroutine());
    }

    private IEnumerator LimitTimerCoroutine()
    {
        int limitTime = _limitTime;
        _limitTimeText.color = Color.black;
        _limitTimeText.text = limitTime.ToString();

        while (limitTime > 0)
        {
            yield return _waitOneSecond;
            limitTime--;

            // ���� �ð��� 10�� �̸��� �Ǹ� �ؽ�Ʈ ������ ���������� ����
            if (limitTime == 9)
                _limitTimeText.color = Color.red;

            // ���� �ð� �ؽ�Ʈ ������Ʈ
            _limitTimeText.text = limitTime.ToString();
        }

        if (this.gameObject.activeSelf)
            OnMissing();
    }

    public override void OnMissing()
    {
        // 2���̺굿�� ����г�Ƽ�� �ο��Ѵ�.
        _goldPenalty.remainWave += 2;
        UIManager.instance.ShowSystemMessage(SystemMessage.MessageType.MissingBossPenalty);
        SoundManager.instance.PlaySFX("Mission Boss Missing Sound");
        ReturnObject();
    }

    protected override void ReturnObject()
    {
        this.gameObject.SetActive(false);
    }
}
