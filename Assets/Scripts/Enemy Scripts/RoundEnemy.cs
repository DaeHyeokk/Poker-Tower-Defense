public class RoundEnemy : FieldEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnMissing()
    {
        GameManager.instance.DecreaseLife(1);
        ReturnObject();
    }

    protected override void Die()
    {
        base.Die();
        ReturnObject();
    }

    protected override void ReturnObject()
    {
        enemySpawner.roundEnemyList.Remove(this);
        enemySpawner.roundEnemyPool.ReturnObject(this);
    }
}


/*
 * File : RoundEnemy.cs
 * First Update : 2022/05/10 THU 04:43
 * 기존의 Enemy 클래스를 세분화 하면서 새롭게 생성한 스크립트.
 * 라운드마다 기본으로 생성되는 몬스터의 컴포넌트로 부착되며, 놓칠 경우 라이프가 1 감소한다.
 */