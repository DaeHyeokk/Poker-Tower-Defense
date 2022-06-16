public class RoundEnemy : FieldEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Die()
    {
        base.Die();
        GameManager.instance.gold += 5;
        ReturnObject();
    }

    protected override void ReturnObject()
    {
        EnemySpawner.instance.roundEnemyList.Remove(this);
        EnemySpawner.instance.roundEnemyPool.ReturnObject(this);
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