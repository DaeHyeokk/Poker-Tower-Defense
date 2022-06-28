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
        ReturnObject();
    }

    protected override void ReturnObject()
    {
        enemySpawner.missionBossEnemyList.Remove(this);
        this.gameObject.SetActive(false);
    }
}
