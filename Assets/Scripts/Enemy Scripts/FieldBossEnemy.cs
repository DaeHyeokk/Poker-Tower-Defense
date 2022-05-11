using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class FieldBossEnemy : FieldEnemy
{
    [SerializeField]
    private int _limitTime;
    [SerializeField]
    private TextMeshProUGUI _limitTimeText;

    private WaitForSeconds _oneSecond;
    protected override void Awake()
    {
        base.Awake();
        _oneSecond = new WaitForSeconds(1f);
        StartCoroutine(LimitTimer());
    }

    public override void Setup(Transform[] wayPoints, EnemyData enemyData)
    {
        base.Setup(wayPoints, enemyData);
        StartCoroutine(LimitTimer());
    }

    private IEnumerator LimitTimer()
    {
        int missingCount = _limitTime;
        _limitTimeText.color = Color.black;
        _limitTimeText.text = missingCount.ToString();

        while (missingCount > 0)
        {
            yield return _oneSecond;
            missingCount--;

            // ���� �ð��� 10�� �̸��� �Ǹ� �ؽ�Ʈ ������ ���������� ����
            if (missingCount == 9)
                _limitTimeText.color = Color.red;

            // ���� �ð� �ؽ�Ʈ ������Ʈ
            _limitTimeText.text = missingCount.ToString();
        }

        if(this.gameObject.activeInHierarchy)
            OnMissing();
    }
}
