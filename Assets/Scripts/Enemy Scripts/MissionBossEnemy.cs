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

    public override void Setup(Transform[] wayPoints, EnemyData enemyData)
    {
        base.Setup(wayPoints, enemyData);
        StartCoroutine(LimitTimerCoroutine());
    }

    private IEnumerator LimitTimerCoroutine()
    {
        int missingCount = _limitTime;
        _limitTimeText.color = Color.black;
        _limitTimeText.text = missingCount.ToString();

        while (missingCount > 0)
        {
            yield return _waitOneSecond;
            missingCount--;

            // 남은 시간이 10초 미만이 되면 텍스트 색깔을 빨간색으로 변경
            if (missingCount == 9)
                _limitTimeText.color = Color.red;

            // 남은 시간 텍스트 업데이트
            _limitTimeText.text = missingCount.ToString();
        }

        if (this.gameObject.activeInHierarchy)
            OnMissing();
    }

    public override void OnMissing()
    {

        ReturnObject();
    }

    protected override void Die()
    {
        base.Die();

        switch (_bossLevel)
        {
            case 1:
                GameManager.instance.gold += 100;
                break;
            case 2:
                GameManager.instance.gold += 200;
                break;
            case 3:
                GameManager.instance.gold += 500;
                break;
        }

        ReturnObject();
    }

    protected override void ReturnObject()
    {
        EnemySpawner.instance.missionBossEnemyList.Remove(this);
        this.gameObject.SetActive(false);
    }
}
