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
 * Ÿ�Ͽ� Ÿ���� �Ǽ��Ǿ� �ִ��� ���θ� Ȯ���ϱ� ���� Tile ��ũ��Ʈ
 * bool Ÿ�� ���� isBuildTower�� ���� Ÿ���� ���� ������ �ִ��� Ȯ���� �� �ִ�.
 */