using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private WaveSystem _waveSystem;

    private void Awake()
    {
        // Ʃ�丮���� Ȱ��ȭ �Ǹ� ���̺��� ������ �����.
        _waveSystem.isWaveStopped = true;
        
    }

}
