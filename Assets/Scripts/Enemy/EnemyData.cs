using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/EnemyData", fileName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float health; // ü��
    public float moveSpeed; // �̵� �ӵ�
}


/*
 * File : EnemyData.cs
 * First Update : 2022/04/22 FRI 02:07
 * ���帶�� �����Ǵ� Enemy�� ü�� �� �̵��ӵ��� ������ ��ũ�����̺�
 * ü�°� �̵��ӵ� �����͸� ������
 */