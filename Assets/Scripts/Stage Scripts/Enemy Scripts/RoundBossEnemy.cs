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
        // ���� ������ ������ ��� ���ӿ��� �й��Ѵ�.
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
