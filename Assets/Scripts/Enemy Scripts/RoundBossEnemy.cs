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
        MissionManager.instance.cutItCloseMission.CheckMission();
    }

    protected override void ReturnObject()
    {
        this.gameObject.SetActive(false);
    }
}
