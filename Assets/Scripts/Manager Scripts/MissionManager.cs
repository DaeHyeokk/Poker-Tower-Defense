using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private static MissionManager _instance;
    public static MissionManager instance
    {
        get
        {
            if (_instance == null)
            {
                // ������ UIManager ������Ʈ�� ã�� �Ҵ�
                _instance = FindObjectOfType<MissionManager>();
            }

            return _instance;
        }
    }

    private Mission _allKillMission;

    public Mission allKillMission => _allKillMission;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);    // �ڽ��� �ı�

        _allKillMission = FindObjectOfType<AllKillMission>();
    }
}
