using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialBossEnemy : Enemy
{
    [SerializeField]
    private Rotater2D _rotater2D;
    [SerializeField]
    private TextMeshProUGUI _healthText;

    public int level { get; set; }

    protected override void Awake()
    {
        base.Awake();
        enemySpawner.IsLevelupSpecialBoss();
    }
    public override void Setup(EnemyData enemyData)
    {
        base.Setup(enemyData);
        _healthText.text = Mathf.Round(enemyData.health).ToString();
    }

    public override void TakeDamage(float damage, DamageTakenType damageTakenType)
    {
        base.TakeDamage(damage, damageTakenType);
        _healthText.text = Mathf.Round(health).ToString();
    }

    // Special Bass�� ���ϰ� ���ο� ������� ���� ���� //
    public override void TakeStun(float duration)
    {
        return;
    }

    public override void TakeSlowing(float slowingRate, float duration)
    {
        return;
    }
    ///////////////////////////////////////////////////////////////////

    protected override void Die()
    {
        base.Die();
        Respawn();
    }

    private void Respawn()
    {
        gameObject.SetActive(false);

        if (enemySpawner.IsLevelupSpecialBoss())
            gameObject.SetActive(true);
    }
}
