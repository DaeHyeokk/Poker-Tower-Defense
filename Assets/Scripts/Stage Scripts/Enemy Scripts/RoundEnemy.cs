using System.Collections;
using System.Collections.Generic;

public class RoundEnemy : FieldEnemy
{
    // �ʵ� ���� ��ȯ�� RoundEnemy ���Ḯ��Ʈ�� ���� �� ���� �� �� ���Ǵ� ���.
    private LinkedListNode<RoundEnemy> _roundEnemyNode;

    public LinkedListNode<RoundEnemy> roundEnemyNode => _roundEnemyNode;

    protected override void Awake()
    {
        base.Awake();
        _roundEnemyNode = new(this);
    }

    public override void Setup(EnemyData enemyData)
    {
        base.Setup(enemyData);

        // ������ Enemy�� ü�� ���� (���� �������� ���̵��� ���)
        _maxHealth = enemyData.health * StageManager.instance.roundEnemyHpPercentage;
        _health = _maxHealth;
        _enemyHealthbar.maxHealth = _maxHealth;
        _enemyHealthbar.health = _maxHealth;
    }

    protected override void GiveReward()
    {
        // �Ϲ� ���ʹ� ������ ���� ����.
        return;
    }

    protected override void ReturnObject()
    {
        enemySpawner.roundEnemyList.Remove(_roundEnemyNode);
        enemySpawner.roundEnemyPool.ReturnObject(this);
    }
}


/*
 * File : RoundEnemy.cs
 * First Update : 2022/05/10 THU 04:43
 * ������ Enemy Ŭ������ ����ȭ �ϸ鼭 ���Ӱ� ������ ��ũ��Ʈ.
 * ���帶�� �⺻���� �����Ǵ� ������ ������Ʈ�� �����Ǹ�, ��ĥ ��� �������� 1 �����Ѵ�.
 * 
 * Update : 2022/06/16 THU 22:15
 * ������ �й� ���� ���� : �������� 0�� �� �� -> �ʵ忡 ���Ͱ� 80���� �̻��� �� ��.
 * ���� �������� ���ҽ�Ű�� ������ ������.
 */