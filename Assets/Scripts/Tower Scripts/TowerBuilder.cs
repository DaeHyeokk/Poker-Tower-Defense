using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField]
    private TowerData[] _towerDatas;
    [SerializeField]
    private CardDrawer _cardDrawer;
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private GameObject[] _towerPrefabs;
    
    private ObjectPool<TowerWeapon> _towerPool;

    private void Awake()
    {
        _towerPool = new ObjectPool<TowerWeapon>(_towerPrefabs, 10);
    }

    public void BuildTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        // Ÿ���� ���� ��ġ�� Ÿ�Ͽ� Ÿ���� �̹� �������ִ��� ���θ� Ȯ���ϰ�,
        // Ÿ���� ���� ���� ī�带 ���� �������� Ȯ���Ѵ�.
        if (!tile.isBuildTower && _cardDrawer.isDraw)
        {
            // Ÿ���� ���������� �ʰ� ī�带 ���� ���¶�� �ش� Ÿ�Ͽ� Ÿ���� �Ǽ��� ����, isBuildTower ���� true�� �����Ѵ�.
            TowerData towerData = _towerDatas[(int)_cardDrawer.drawHand];
            TowerWeapon towerWeaponObject = _towerPool.GetObject((int)_cardDrawer.drawHand);

            towerWeaponObject.Setup(towerData, _enemySpawner, _cardDrawer.drawHand);
            towerWeaponObject.transform.position = tileTransform.position + Vector3.back;

            tile.isBuildTower = true;

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
 * ������ �������� �����ϴ� ��Ŀ��� CardDrawer �� ���� ��Ŀ ������ ��ġ�ϴ� Ÿ���� �����ϴ� ������� �ٲ��.
 * �÷��̾��� ȭ�鿡 ī�带 �̴� ����� ��� ���� ���� Ÿ���� ������ �ϴ°��� �������� �´ٰ� �Ǵ��Ͽ��� ������ 
 * �÷��̾��� ���� �ൿ�� ����ϱ� ���Ͽ� CardDrawer.isDraw ���� Ȯ�� �� Ÿ���� ���� �غ� �� �ǰ��� Ÿ���� ���� �� �ֵ��� �����Ͽ���.
 * 
 * Update : 2022/04/28 THU 23:55
 * ������ Ÿ���� ���� Instantiate() �ϴ� ��Ŀ��� ������ƮǮ���� ������Ʈ�� �������� ������� ����.
 */