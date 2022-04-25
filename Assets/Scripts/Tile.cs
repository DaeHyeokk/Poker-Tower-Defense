using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isBuildTower { get; set; }

    private void Awake()
    {
        isBuildTower = false;
    }
}


/*
 * File : Tile.cs
 * First Update : 2022/04/25 MON 10:50
 * 타일에 타워가 건설되어 있는지 여부를 확인하기 위한 Tile 스크립트
 * bool 타입 변수 isBuildTower를 통해 타워가 현재 지어져 있는지 확인할 수 있다.
 */