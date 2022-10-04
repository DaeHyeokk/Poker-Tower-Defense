using System.Collections;
using System.Collections.Generic;

public class RoundEnemy : FieldEnemy
{
    // 필드 위에 소환된 RoundEnemy 연결리스트에 삽입 및 삭제 될 때 사용되는 노드.
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

        // 생성할 Enemy의 체력 설정 (현재 스테이지 난이도에 비례)
        _maxHealth = enemyData.health * StageManager.instance.roundEnemyHpPercentage;
        _health = _maxHealth;
        _enemyHealthbar.maxHealth = _maxHealth;
        _enemyHealthbar.health = _maxHealth;
    }

    protected override void GiveReward()
    {
        // 일반 몬스터는 보상을 주지 않음.
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
 * 기존의 Enemy 클래스를 세분화 하면서 새롭게 생성한 스크립트.
 * 라운드마다 기본으로 생성되는 몬스터의 컴포넌트로 부착되며, 놓칠 경우 라이프가 1 감소한다.
 * 
 * Update : 2022/06/16 THU 22:15
 * 게임의 패배 조건 변경 : 라이프가 0이 될 때 -> 필드에 몬스터가 80마리 이상이 될 때.
 * 따라서 라이프를 감소시키는 로직을 제거함.
 */