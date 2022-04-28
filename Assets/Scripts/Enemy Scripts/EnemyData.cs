using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/EnemyData", fileName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float health; // 체력
    public float moveSpeed; // 이동 속도
}


/*
 * File : EnemyData.cs
 * First Update : 2022/04/22 FRI 02:07
 * 라운드마다 생성되는 Enemy의 체력 및 이동속도를 저장할 스크립테이블
 * 체력과 이동속도 데이터를 가진다
 */