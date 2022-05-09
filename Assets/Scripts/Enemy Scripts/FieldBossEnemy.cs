using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldBossEnemy : FieldEnemy
{
    [SerializeField]
    private int limitTime;
    private WaitForSeconds _oneSecond;
    protected override void Awake()
    {
        base.Awake();
        _oneSecond = new WaitForSeconds(1f);
    }

    public override void Setup(EnemyData enemyData)
    {
        base.Setup(enemyData);
        StartCoroutine(LimitTimer());
    }

    private IEnumerator LimitTimer()
    {
        int missingCount = 60;
        while(missingCount <= 0)
        {
            // ���� �ð� �ؽ�Ʈ ������Ʈ �ϴ� ����

            //////////////////////
            yield return _oneSecond;
            missingCount--;
        }

        if(this.gameObject.activeInHierarchy)
            OnMissing();
    }
}
