using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
                return m_instance;
            }
            else
                return m_instance;
        }
    }

    private int round = 0;
    public bool isGameover { get; private set; }
    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        isGameover = false;
    }

    public void IncreaseRound()
    {
        round++;
    }
    public int GetRound()
    {
        return round;
    }
}

/*
 * File : GameManager.cs
 * First Update : 2022/04/22 FRI 02:30
 * PokerTower Defense 게임의 전체적인 정보를 관리할 GameManager 스크립트
 * 진행중인 게임의 라운드와 Gameover 상태를 갖고 있음
 */
