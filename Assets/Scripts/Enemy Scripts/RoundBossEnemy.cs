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

    protected override void Die()
    {
        base.Die();
        MissionManager.instance.cutItCloseMission.CheckMission();
    }

    protected override void ReturnObject()
    {
        this.gameObject.SetActive(false);
    }
}
