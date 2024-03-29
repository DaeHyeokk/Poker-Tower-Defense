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

    public override void Setup(BossEnemyData enemyData)
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

            // 남은 시간이 10초 미만이 되면 텍스트 색깔을 빨간색으로 변경
            if (limitTime == 9)
                _limitTimeText.color = Color.red;

            // 남은 시간 텍스트 업데이트
            _limitTimeText.text = limitTime.ToString();
        }

        if (this.gameObject.activeSelf)
            OnMissing();
    }

    public override void OnMissing()
    {
        // 2웨이브동안 골드패널티를 부여한다.
        _goldPenalty.remainWave += 2;
        StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.MissingBossPenalty);
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.missionBossMissingSound);
        ReturnObject();
    }

    protected override void ReturnObject()
    {
        this.gameObject.SetActive(false);
    }
}
