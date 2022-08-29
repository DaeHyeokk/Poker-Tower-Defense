using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialBossEnemy : Enemy
{
    [SerializeField]
    private TextMeshProUGUI _healthText;
    [SerializeField]
    private TextMeshProUGUI _levelText;

    public int level { get; set; }

    protected override void Awake()
    {
        base.Awake();
        enemySpawner.LevelupSpecialBoss();
    }

    public void Setup(SpecialBossData enemyData)
    {
        base.Setup(enemyData);
        //UpdateHealthText();

        _rewardGold = enemyData.rewardGold;
        _rewardChangeChance = enemyData.rewardChangeChance;
        _rewardJokerCard = enemyData.rewardJokerCard;

        _rewardStringBuilder.Set(_rewardGold, _rewardChangeChance, _rewardJokerCard);
        _healthText.text = ((int)Mathf.Round(maxHealth)).ToString();
        _levelText.text = "Lv " + level.ToString();
    }

    private void UpdateHealthText()
    {
        float healthPercent = _enemyHealthbar.healthPercent;

        if (healthPercent >= 1f)
            _healthText.text = Mathf.Round(healthPercent).ToString() + '%';
        // ���� ü���� 1% �̸��� ��� �Ҽ��� ù° �ڸ����� ǥ��.
        else
            _healthText.text = (Mathf.Round(healthPercent * 10f) / 10f).ToString() + '%';
    }

    public override void TakeDamage(Tower fromTower, float damage, DamageTakenType damageTakenType)
    {
        base.TakeDamage(fromTower, damage, damageTakenType);
        //UpdateHealthText();
        _healthText.text = ((int)Mathf.Round(health)).ToString();
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

    protected override void Die(Tower fromTower)
    {
        base.Die(fromTower);
        Respawn();
    }

    private void Respawn()
    {
        gameObject.SetActive(false);
        enemySpawner.LevelupSpecialBoss();
    }
}
