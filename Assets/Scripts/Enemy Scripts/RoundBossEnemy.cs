using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBossEnemy : FieldBossEnemy
{
    public override void OnMissing()
    {
        // ���� ������ ������ ��� ���ӿ��� �й��Ѵ�.
        GameManager.instance.DefeatGame();

        ReturnObject();
    }

    protected override void ReturnObject()
    {
        EnemySpawner.instance.roundEnemyList.Remove(this);
        this.gameObject.SetActive(false);
    }
}
