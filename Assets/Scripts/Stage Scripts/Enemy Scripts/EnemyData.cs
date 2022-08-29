using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable/EnemyData", fileName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Sprite sprite;   // 적 스프라이트 이미지
    public float health;    // 체력
    public float moveSpeed; // 이동 속도
}


/*
 * File : EnemyData.cs
 * First Update : 2022/04/22 FRI 02:07
 * 라운드마다 생성되는 Enemy의 체력 및 이동속도를 저장할 스크립테이블
 * 체력과 이동속도 데이터를 가진다
 * 
 * Update : 2022/05/10 TUE 20:51
 * Enemy 종류마다 다른 스프라이트 이미지를 갖게 하기 위한 스프라이트 이미지 변수 추가.
 * 
 * Update : 2022/05/12 WED 18:45
 * 스프라이트 변수 삭제 => EnemySpawner에 할당하여 EnemySpawner에서 Enemy를 소환할 때 스프라이트를 변경하도록 변경.
 */