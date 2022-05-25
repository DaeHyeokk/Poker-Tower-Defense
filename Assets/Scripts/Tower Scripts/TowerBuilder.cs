using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _towerPrefabs;
    [SerializeField]
    private GameObject[] _followTowerPrefabs;
    [SerializeField]
    private Transform _towerSpawnPoint;

    private List<ObjectPool<Tower>> _towerPoolList;
    private FollowTower[] _followTowers;

    public List<ObjectPool<Tower>> towerPoolList => _towerPoolList;
    public FollowTower[] followTowers => _followTowers;

    private void Awake()
    {
        _towerPoolList = new List<ObjectPool<Tower>>();
        for(int i=0; i<_towerPrefabs.Length; i++)
            _towerPoolList.Add(new ObjectPool<Tower>(_towerPrefabs[i], 5));

        _followTowers = new FollowTower[_followTowerPrefabs.Length];
        for (int i = 0; i < _followTowerPrefabs.Length; i++)
        {
            _followTowers[i] = Instantiate(_followTowerPrefabs[i]).GetComponent<FollowTower>();
            _followTowers[i].gameObject.SetActive(false);
        }
    }

    public void BuildTower(int towerIndex)
    {
        Tower tower = _towerPoolList[towerIndex].GetObject();

        // Ÿ���� ȭ���� ���߾ӿ��� ���� �ȴ�.
        tower.transform.position = _towerSpawnPoint.position;
        tower.Setup();

        _towerSpawnPoint.position -= new Vector3(0f, 0f, 0.000001f);
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
 * 
 * Update : 2022/05/02 MON 18:12
 * Ÿ���� �� �߾ӿ� �����ǵ��� ����.
 * 
 * Update : 2022/05/22 SUN 17:30
 * FollowTower �������� �ν��Ͻ��ϴ� ���� �߰�.
 */