using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                return _instance;
            }

            return _instance;
        }
    }

    private int _round;
    private bool _isGameover;

    public int round => _round;
    public bool isGameover => _isGameover;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        _round = 0;
        _isGameover = false;
    }

    public void IncreaseRound()
    {
        _round++;
    }
}

/*
 * File : GameManager.cs
 * First Update : 2022/04/22 FRI 02:30
 * PokerTower Defense 게임의 전체적인 정보를 관리할 GameManager 스크립트
 * 진행중인 게임의 라운드와 Gameover 상태를 갖고 있음
 */
