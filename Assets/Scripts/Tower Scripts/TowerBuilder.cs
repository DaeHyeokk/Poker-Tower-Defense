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

    public void BuildTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        // 타워를 지을 위치의 타일에 타워가 이미 지어져있는지 여부를 확인하고,
        // 타워를 짓기 위해 카드를 뽑은 상태인지 확인한다.
        if (!tile.isBuildTower && _cardDrawer.isDraw)
        {
            // 타워가 지어져있지 않고 카드를 뽑은 상태라면 해당 타일에 타워를 건설한 다음, isBuildTower 값을 true로 설정한다.
            TowerData towerData = _towerDatas[(int)_cardDrawer.drawHand];
            GameObject clone = Instantiate(towerData.towerPrefab, tileTransform.position + Vector3.back, Quaternion.identity);
            clone.GetComponent<TowerWeapon>().Setup(towerData, _enemySpawner);

            tile.isBuildTower = true;

            // 뽑았던 카드를 초기화 한다.
            _cardDrawer.ResetDrawer();
        }
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
 * 기존에 랜덤으로 생성하던 방식에서 CardDrawer 가 뽑은 포커 족보와 일치하는 타워를 생성하는 방식으로 바꿨다.
 * 플레이어의 화면에 카드를 뽑는 장면이 모두 나온 다음 타워를 짓도록 하는것이 논리적으로 맞다고 판단하였기 때문에 
 * 플레이어의 돌발 행동에 대비하기 위하여 CardDrawer.isDraw 값을 확인 후 타워를 지을 준비가 다 되고나서 타워를 지을 수 있도록 구현하였다.
 */