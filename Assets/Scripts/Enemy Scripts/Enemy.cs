using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    // Enemy�� ���� ��� �÷��̾�� ���޵Ǵ� ���
    [SerializeField]
    private int _rewardGold;
    // Enemy�� ���� ��� �÷��̾�� ���޵Ǵ� ī�屳ȯ��
    [SerializeField]
    private int _rewardChangeChance;

    private Slider _healthSlider;
    private SpriteRenderer _enemySprite;
    private WaitForSeconds _takeDamageAnimationDelay;
    private StringBuilder _rewardText;

    protected Slider healthSlider => _healthSlider;
    protected SpriteRenderer enemySprite => _enemySprite;
    protected float maxHealth { get; set; }  // Enemy�� �ִ� ü��
    protected float health { get; set; }     // Enemy�� ���� ü��
  
    protected virtual void Awake()
    {
        _healthSlider = GetComponentInChildren<Slider>();
        _enemySprite = GetComponentInChildren<SpriteRenderer>();
        _takeDamageAnimationDelay = new WaitForSeconds(0.05f);
        _rewardText = new();

        SetRewardText();
    }

    public virtual void TakeDamage(float damage, DamageTakenType damageTakenType)
    {
        StartCoroutine(EnemyTakeDamageAnimationCoroutine());
    }

    public abstract void TakeIncreaseReceivedDamage(float increaseReceivedDamageRate, float duration);
    public abstract void TakeStun(float duration);
    public abstract void TakeSlowing(float slowingRate, float duration);

    protected virtual void Die()
    {
        ParticlePlayer.instance.PlayEnemyDie(this.transform);
        GiveReward();
    }

    private void GiveReward()
    {
        GameManager.instance.gold += _rewardGold;
        if (_rewardChangeChance > 0)
            GameManager.instance.changeChance += _rewardChangeChance;

        UIManager.instance.ShowEnemyDieRewardText(_rewardText, this.transform);
    }

    private void SetRewardText()
    {
        _rewardText.Append('+');
        _rewardText.Append(_rewardGold.ToString());
        _rewardText.Append('G');

        if(_rewardChangeChance > 0)
        {
            _rewardText.Append('\n');
            _rewardText.Append("ī�屳ȯ�� ");
            _rewardText.Append('+');
            _rewardText.Append(_rewardChangeChance.ToString());
        }
    }

    private IEnumerator EnemyTakeDamageAnimationCoroutine()
    {
        _enemySprite.color = Color.red;

        yield return _takeDamageAnimationDelay;

        _enemySprite.color = Color.white;
    }
}

/*
 * File : Enemy.cs
 * First Update : 2022/04/20 WED 14:57
 * Enemy ������Ʈ�� ������ �� �̵��� Waypoint ��ǥ�� �����ϰ�,
 * ��ǥ ������ ������� �̵��ϸ� ������ WayPoint�� �������� �� ������Ʈ�� �ı��Ѵ�.
 * 
 * Update : 2022/04/21 THU 13:20
 * ������ƮǮ�� ����� �����Ͽ��� ������ ���� ���忡�� ����� ������Ʈ�� ����ؼ� �����ϰ� ��
 * ���� Setup() �޼��忡�� ������ WayPoints �迭�� �Ҵ�� ���� �ִ��� Ȯ���ϴ� ������ �߰��Ͽ� ���ʿ��� ������ ���� �ʵ��� �����Ͽ���,
 * WayPoints �迭�� ù��° �ε������� �ٽ� �̵��ϱ� ���� ������ ����ߴ� currentIndex ���� 0���� �ٲٴ� ������ �߰���
 * 
 * Update : 2022/04/22 FRI 02:25
 * maxHealth, health, enemyMovement.moveSpped ���� �߰��Ͽ���
 * Setup() �޼��忡�� �Ű������� �޴� EnemyData�� ���� �������� ���� �����鿡 ���� �Ҵ���
 * Enemy�� ��ǥ ������ ������ ��� ó���ؾ� �� ������ ��������Ʈ Action�� ���� �̷�������� ����
 * 
 * Update : 2022/04/28 THU 16:40
 * Enemy�� ȸ���� �� ª�Զ� ���� �ɸ��� ��� ���� ��θ� �������� ���ϰ� ��θ� ��Ż�ϴ� Enemy�� �߻��ϴ� ���װ� �߰ߵ�.
 * ���� enemy�� ��ǥ������ �Ÿ��� nowDistance, �� ������ �� enemy�� ��ǥ������ �Ÿ��� lastDistance ������ �����ϰ�, �� ���� �������ν�
 * nowDistance ���� lastDistance ������ Ŭ ��� ��ǥ���� �־����� �ִٰ� �Ǵ��ϰ� enemy�� position�� ��ǥ waypoint�� position������ �ٲ�
 * ��Ż�� ��θ� �ٽ� ����ִ� ������ �߰��Ͽ� �̸� �ذ��Ͽ���.
 * 
 * Update : 2022/05/01 SUN 22:10
 * �ʵ� ������ �������� ������ ��������Ʈ�� �����ϸ� �������� �������� �ٽ� ù��° ��������Ʈ�� ���� �̵��ϵ��� �����Ͽ���.
 * 
 * Update : 2022/05/09 MON 03:05
 * Enemy ��ü�� ���� �̻�(����, ���ο�) ����.
 */
