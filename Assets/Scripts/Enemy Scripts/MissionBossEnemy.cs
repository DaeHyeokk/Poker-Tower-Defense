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

    private WaitForSeconds _waitOneSecond = new(1f);

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

            // 남은 시간이 10초 미만이 되면 텍스트 색깔을 빨간색으로 변경
            if (limitTime == 9)
                _limitTimeText.color = Color.red;

            // 남은 시간 텍스트 업데이트
            _limitTimeText.text = limitTime.ToString();
        }

        if (this.gameObject.activeSelf)
            Missing();
    }

    public override void Missing()
    {
        /*
         * 보스 잡기 실패 패널티를 주는 로직이 들어갈 곳
         * 
         */

        ReturnObject();
    }

    protected override void ReturnObject()
    {
        this.gameObject.SetActive(false);
    }
}
