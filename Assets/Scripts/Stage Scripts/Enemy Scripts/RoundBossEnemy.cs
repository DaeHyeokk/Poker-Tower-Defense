using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBossEnemy : FieldBossEnemy
{
    [SerializeField]
    private WaveSystem _waveSystem;

    private void Update()
    {
        if (_waveSystem.isFinalWave)
            StageManager.instance.bossKilledTakenTime += Time.deltaTime;
    }

    public override void OnMissing()
    {
        // 라운드 보스는 못잡을 경우 게임에서 패배한다.
        StageManager.instance.DefeatGame();

        ReturnObject();
    }

    protected override void Die(Tower fromTower)
    {
        base.Die(fromTower);

        if (_waveSystem.isFinalWave)
        {
            StageManager.instance.ClearGame();
            ReturnObject();
        }
    }

    protected override void ReturnObject()
    {
        this.gameObject.SetActive(false);
    }
}
