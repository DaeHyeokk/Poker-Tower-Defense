using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField]
    private CardDrawer _cardDrawer;
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private GameObject _towerPrefab;
    [SerializeField]
    private GameObject[] _weaponPrefabs;


    private ObjectPool<TowerWeapon> _weaponPool;
    private ObjectPool<Tower> _towerPool;


    private void Awake()
    {
        _weaponPool = new ObjectPool<TowerWeapon>(_weaponPrefabs, 10);
        _towerPool = new ObjectPool<Tower>(_towerPrefab, 20);
    }

    public void BuildTower()
    {
        // Ÿ���� ���� ���� ī�带 ���� �������� Ȯ���Ѵ�.
        if (_cardDrawer.isDraw)
        {
            Tower tower = _towerPool.GetObject();
            TowerWeapon towerWeapon = _weaponPool.GetObject((int)_cardDrawer.drawHand);
             
            // Ÿ���� ȭ���� ���߾ӿ��� ���� �ȴ�.
            tower.transform.position = Vector3.zero;
            towerWeapon.transform.SetParent(tower.transform);
            towerWeapon.transform.localPosition = Vector3.zero;
            tower.DefaultSetup(_enemySpawner);

            // �̾Ҵ� ī�带 �ʱ�ȭ �Ѵ�.
            _cardDrawer.ResetDrawer();
        }
    }
}


/*
 * File : TowerBuilder.cs
 * First Update : 2022/04/25 MON 11:07
 * Tile ������Ʈ ���� Ÿ���� �Ǽ��ϴ� �۾��� �����ϴ� BuilderTower ��ũ��Ʈ
 * Tile ������Ʈ ���� Ÿ���� �Ǽ��Ǿ� �ִ��� ���θ� üũ�Ͽ� ������� ��� 
 * �ش� Tile���� (0, 0, -1)��ŭ ������ ��ǥ�� Ÿ���� �����Ѵ�. 
 *      => Ÿ���� Ÿ�Ϻ��� ���� ���������ν� Raycast ������ Ÿ���� �ƴ� Ÿ���� �浹�ϵ��� ��
 * 
 * Update : 2022/04/27 WED 23:20
 * ������ �������� �����ϴ� ��Ŀ��� CardDrawer �� ���� ��Ŀ ������ ��ġ�ϴ� Ÿ���� �����ϴ� ������� ����.
 * �÷��̾��� ȭ�鿡 ī�带 �̴� ����� ��� ���� ���� Ÿ���� ������ �ϴ°��� �������� �´ٰ� �Ǵ��Ͽ��� ������ 
 * �÷��̾��� ���� �ൿ�� ����ϱ� ���Ͽ� CardDrawer.isDraw ���� Ȯ�� �� Ÿ���� ���� �غ� �� �ǰ��� Ÿ���� ���� �� �ֵ��� �����Ͽ���.
 * 
 * Update : 2022/04/28 THU 23:55
 * ������ Ÿ���� ���� Instantiate() �ϴ� ��Ŀ��� ������ƮǮ���� ������Ʈ�� �������� ������� ����.
 * 
 * Update : 2022/04/30 SAT 22:30
 * Ÿ�� ������Ʈ�� ������ �缳�� �ϸ鼭 Tower ������Ʈ�� �߰��� �����ϴ°ɷ� �ٲ�.
 * ������Ʈ�� ������ �� TowerWeapon ������Ʈ�� Tower ������Ʈ�� �ڽ����� �ٲ������ν� �� ������Ʈ�� ������ ������Ʈ�� ������ �� �ֵ��� ������.
 * SetParent() ���Ŀ� TowerWeapon ������Ʈ�� localPosition �� Vector3.zero �� ���������ν� Tower ������Ʈ�� TowerWeapon ������Ʈ�� ��ġ�� ����ȭ��.
 */