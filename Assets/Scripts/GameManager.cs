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
    private int _finalRound;
    private int _life;
    private int _gold;
    private int _mineral;
    private int _changeChance;
    private int _pokerCount;
    private bool _isGameover;

    public int round => _round;
    public int life => _life;
    public int gold => _gold;
    public int mineral => _mineral;
    public int changeChance => _changeChance;
    public int pokerCount => _pokerCount;
    public bool isGameover => _isGameover;
    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        _round = 0;
        _finalRound = 40;
        _life = 100;
        _gold = 400;
        _mineral = 0;
        _changeChance = 0;
        _pokerCount = 7;
        _isGameover = false;
    }

    public void IncreaseRound() => _round++;

    public void DecreaseLife(int decreaseCount)
    {
        _life -= decreaseCount;

        if (_life <= 0)
            EndGame();
    }
    private void EndGame()
    {
        _isGameover = true;
    }

    public void IncreaseGold(int increaseCount) => _gold += increaseCount;
    public void DecreaseGold(int decreaseCount) => _gold -= decreaseCount;

    public void IncreaseMineral(int increaseCount) => _mineral += increaseCount;
    public void DecreaseMineral(int decreaseCount) => _mineral -= decreaseCount;

    public void IncreaseChangeChance(int increaseCount) => _changeChance += increaseCount;
    public void DecreaseChangeChance() => _changeChance--;

    public bool IsFinalRound() { return _round == _finalRound; }
}

/*
 * File : GameManager.cs
 * First Update : 2022/04/22 FRI 02:30
 * PokerTower Defense 게임의 전체적인 정보를 관리할 GameManager 스크립트
 * 진행중인 게임의 라운드와 Gameover 상태를 갖고 있음
 * 
 * Update : 2022/05/10 THU 06:30
 * 진행중인 게임의 남은 라이프, 골드와 미네랄, 카드 변경권 횟수를 관리하는 로직 추가.
 * 현재 라운드가 마지막 라운드인지 확인하는 로직 추가.
 */
