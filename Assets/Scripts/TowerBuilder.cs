using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField]
    private TowerData[] towerDatas;

    public void BuildTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        // 타워를 지을 위치의 타일에 타워가 이미 지어져있는지 여부를 확인
        if (!tile.isBuildTower)
        {
            // 타워가 지어져있지 않다면 해당 타일에 타워를 건설하고, isBuildTower 값을 true로 설정
            TowerData towerData = towerDatas[Random.Range(0, 10)];
            GameObject clone = Instantiate(towerData.towerPrefab, tileTransform.position + Vector3.back, Quaternion.identity);
            clone.GetComponent<TowerType>().Setup(towerData);

            tile.isBuildTower = true;
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
 */