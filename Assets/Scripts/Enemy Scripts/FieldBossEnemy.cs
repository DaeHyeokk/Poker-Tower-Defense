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

            // 남은 시간이 10초 미만이 되면 텍스트 색깔을 빨간색으로 변경
            if (missingCount == 9)
                _limitTimeText.color = Color.red;

            // 남은 시간 텍스트 업데이트
            _limitTimeText.text = missingCount.ToString();
        }

        if(this.gameObject.activeInHierarchy)
            OnMissing();
    }
}
