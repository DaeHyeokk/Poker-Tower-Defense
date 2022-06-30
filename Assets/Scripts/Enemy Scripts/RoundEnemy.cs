public class RoundEnemy : FieldEnemy
{
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
 * 
 * Update : 2022/06/16 THU 22:15
 * 게임의 패배 조건 변경 : 라이프가 0이 될 때 -> 필드에 몬스터가 80마리 이상이 될 때.
 * 따라서 라이프를 감소시키는 로직을 제거함.
 */