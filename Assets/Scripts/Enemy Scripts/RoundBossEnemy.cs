using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBossEnemy : FieldBossEnemy
{
    public override void OnMissing()
    {
        // 라운드 보스는 못잡을 경우 게임에서 패배한다.
        GameManager.instance.DefeatGame();

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
        EnemySpawner.instance.roundEnemyList.Remove(this);
        this.gameObject.SetActive(false);
    }
}
