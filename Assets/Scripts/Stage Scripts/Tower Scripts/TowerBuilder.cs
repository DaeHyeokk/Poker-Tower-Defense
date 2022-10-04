using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _towerSprites;
    [SerializeField]
    private GameObject[] _towerPrefabs;
    [SerializeField]
    private GameObject _followTowerPrefab;
    [SerializeField]
    private Transform _towerSpawnPoint;
    [SerializeField]
    private Button[] _towerSelectSpawnButtons;

    private LinkedList<Tower> _towerList = new();
    private List<ObjectPool<Tower>> _towerPoolList = new();
    private FollowTower _followTower;

    public Sprite[] towerSprites => _towerSprites;
    public LinkedList<Tower> towerList => _towerList;
    public List<ObjectPool<Tower>> towerPoolList => _towerPoolList;
    public FollowTower followTower => _followTower;

    private void Awake()
    {
        _towerPoolList = new List<ObjectPool<Tower>>();
        for(int i=0; i<_towerPrefabs.Length; i++)
            _towerPoolList.Add(new ObjectPool<Tower>(_towerPrefabs[i], 5));

        _followTower = Instantiate(_followTowerPrefab).GetComponent<FollowTower>();
        _followTower.gameObject.SetActive(false);

        for (int i = 0; i < _towerSelectSpawnButtons.Length; i++) 
        {
            int towerIndex = i;
            _towerSelectSpawnButtons[i].onClick.AddListener(() => BuildTower(towerIndex, 2));
        }
    }

    public void BuildTower(int towerIndex)
    {
        Tower tower = _towerPoolList[towerIndex].GetObject();
        _towerList.AddLast(tower.towerNode);

        // Ÿ���� ȭ���� ���߾ӿ��� ���� �ȴ�.
        tower.transform.position = _towerSpawnPoint.position;
        tower.Setup();

        _towerSpawnPoint.position += new Vector3(0f, 0f, -0.000001f);

        SoundManager.instance.PlaySFX(SoundFileNameDictionary.towerBuildSound);
    }

    public void BuildTower(int towerIndex, int towerLevel)
    {
        Tower tower = _towerPoolList[towerIndex].GetObject();
        _towerList.AddLast(tower.towerNode);

        // Ÿ���� ȭ���� ���߾ӿ��� ���� �ȴ�.
        tower.transform.position = _towerSpawnPoint.position;
        tower.Setup(towerLevel);

        _towerSpawnPoint.position += new Vector3(0f, 0f, -0.000001f);

        SoundManager.instance.PlaySFX(SoundFileNameDictionary.towerBuildSound);
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