using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBossEnemy : FieldBossEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnMissing()
    {
        GameManager.instance.life -= 20;

        // 마지막 라운드 보스는 못잡을 경우 남은 라이프에 상관없이 바로 패배한다.
        if (GameManager.instance.IsFinalWave())
            GameManager.instance.life -= 100;

        ReturnObject();
    }

    protected override void Die()
    {
        base.Die();
        // 100골드, 카드 변환권 주기
        GameManager.instance.gold += 100;
        GameManager.instance.changeChance += 2;
        ReturnObject();
    }

    protected override void ReturnObject()
    {
        enemySpawner.roundEnemyList.Remove(this);
        this.gameObject.SetActive(false);
    }
}
