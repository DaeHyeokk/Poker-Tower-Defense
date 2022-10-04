using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private WaveSystem _waveSystem;

    private void Awake()
    {
        // 튜토리얼이 활성화 되면 웨이브의 진행을 멈춘다.
        _waveSystem.isWaveStopped = true;
        
    }

}
