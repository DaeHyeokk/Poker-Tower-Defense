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
    [SerializeField]
    private TowerBuilder _towerBuilder;

    private int _rewardTowerLevel;
    public int level { get; set; }

    protected override void Awake()
    {
        base.Awake();
        enemySpawner.LevelupSpecialBoss();
    }

    public void Setup(BossEnemyData enemyData)
    {
        base.Setup(enemyData);
        //UpdateHealthText();

        // 생성할 Enemy의 체력 설정
        _maxHealth = enemyData.health * StageManager.instance.specialBossHpPercentage;
        _health = _maxHealth;
        _enemyHealthbar.maxHealth = _maxHealth;
        _enemyHealthbar.health = _maxHealth;

        _rewardGold = enemyData.rewardGold;
        _rewardChangeChance = enemyData.rewardChangeChance;
        _rewardTowerLevel = enemyData.rewardTowerLevel;

        _rewardStringBuilder.Set(_rewardGold, _rewardChangeChance, _rewardTowerLevel);

        _healthText.text = ((int)Mathf.Round(maxHealth)).ToString();
        _levelText.text = "Lv " + level.ToString();
    }
 
    private void UpdateHealthText()
    {
        float healthPercent = _enemyHealthbar.healthPercent;

        if (healthPercent >= 1f)
            _healthText.text = Mathf.Round(healthPercent).ToString() + '%';
        // 남은 체력이 1% 미만일 경우 소수점 첫째 자리까지 표시.
        else
            _healthText.text = (Mathf.Round(healthPercent * 10f) / 10f).ToString() + '%';
    }

    public override void TakeDamage(Tower fromTower, float damage, DamageTakenType damageTakenType)
    {
        base.TakeDamage(fromTower, damage, damageTakenType);
        //UpdateHealthText();
        _healthText.text = ((int)Mathf.Round(health)).ToString();
    }

    // Special Bass는 스턴과 슬로우 디버프를 받지 않음 //
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
    protected override void GiveReward()
    {
        StageManager.instance.gold += _rewardGold;

        if (_rewardChangeChance > 0)
            StageManager.instance.changeChance += _rewardChangeChance;

        // 스폐셜보스 레벨에 비례하는 레벨의 랜덤 타워를 플레이어에게 지급한다.
        int towerIndex = Random.Range(0, 10);
        _towerBuilder.BuildTower(towerIndex, _rewardTowerLevel);

        StageUIManager.instance.ReservateScreenCenterReward("<color=\"red\">행성파괴 보상!</color>\n" + _rewardStringBuilder.ToString());

        //StageUIManager.instance.ShowEnemyDieRewardText(_rewardStringBuilder.ToString(), this.transform);
    }

    private void Respawn()
    {
        gameObject.SetActive(false);
        enemySpawner.LevelupSpecialBoss();
    }
}
