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
 * ������ Enemy Ŭ������ ����ȭ �ϸ鼭 ���Ӱ� ������ ��ũ��Ʈ.
 * ���帶�� �⺻���� �����Ǵ� ������ ������Ʈ�� �����Ǹ�, ��ĥ ��� �������� 1 �����Ѵ�.
 */