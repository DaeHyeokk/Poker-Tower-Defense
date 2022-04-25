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

        // Ÿ���� ���� ��ġ�� Ÿ�Ͽ� Ÿ���� �̹� �������ִ��� ���θ� Ȯ��
        if (!tile.isBuildTower)
        {
            // Ÿ���� ���������� �ʴٸ� �ش� Ÿ�Ͽ� Ÿ���� �Ǽ��ϰ�, isBuildTower ���� true�� ����
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
 * Tile ������Ʈ ���� Ÿ���� �Ǽ��ϴ� �۾��� �����ϴ� BuilderTower ��ũ��Ʈ
 * Tile ������Ʈ ���� Ÿ���� �Ǽ��Ǿ� �ִ��� ���θ� üũ�Ͽ� ������� ��� 
 * �ش� Tile���� (0, 0, -1)��ŭ ������ ��ǥ�� Ÿ���� �����Ѵ�. 
 *      => Ÿ���� Ÿ�Ϻ��� ���� ���������ν� Raycast ������ Ÿ���� �ƴ� Ÿ���� �浹�ϵ��� ��
 */