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

        // 타워는 화면의 정중앙에서 생성 된다.
        tower.transform.position = _towerSpawnPoint.position;
        tower.Setup();

        _towerSpawnPoint.position -= new Vector3(0f, 0f, 0.000001f);
    }
}


/*
 * File : TowerBuilder.cs
 * First Update : 2022/04/25 MON 11:07
 * Tile 오브젝트 위에 타워를 건설하는 작업을 수행하는 BuilderTower 스크립트
 * Tile 오브젝트 위에 타워가 건설되어 있는지 여부를 체크하여 비어있을 경우 
 * 해당 Tile보다 (0, 0, -1)만큼 떨어진 좌표에 타워를 생성한다. 
 *      => 타워를 타일보다 위에 생성함으로써 Raycast 광선이 타일이 아닌 타워와 충돌하도록 함
 * 
 * Update : 2022/04/27 WED 23:20
 * 기존에 랜덤으로 생성하던 방식에서 CardDrawer 가 뽑은 포커 족보와 일치하는 타워를 생성하는 방식으로 변경.
 * 플레이어의 화면에 카드를 뽑는 장면이 모두 나온 다음 타워를 짓도록 하는것이 논리적으로 맞다고 판단하였기 때문에 
 * 플레이어의 돌발 행동에 대비하기 위하여 CardDrawer.isDraw 값을 확인 후 타워를 지을 준비가 다 되고나서 타워를 지을 수 있도록 구현하였음.
 * 
 * Update : 2022/04/28 THU 23:55
 * 기존에 타워를 직접 Instantiate() 하던 방식에서 오브젝트풀에서 오브젝트를 꺼내오는 방식으로 변경.
 * 
 * Update : 2022/04/30 SAT 22:30
 * 타워 오브젝트의 구조를 재설계 하면서 Tower 오브젝트도 추가로 생성하는걸로 바뀜.
 * 오브젝트를 생성한 뒤 TowerWeapon 오브젝트를 Tower 오브젝트의 자식으로 바꿔줌으로써 두 오브젝트가 서로의 컴포넌트에 접근할 수 있도록 구현함.
 * SetParent() 이후에 TowerWeapon 오브젝트의 localPosition 을 Vector3.zero 로 변경함으로써 Tower 오브젝트와 TowerWeapon 오브젝트의 위치를 동기화함.
 * 
 * Update : 2022/05/02 MON 18:12
 * 타워가 맵 중앙에 생성되도록 변경.
 * 
 * Update : 2022/05/22 SUN 17:30
 * FollowTower 프리팹을 인스턴스하는 로직 추가.
 */