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
                // 씬에서 UIManager 오브젝트를 찾아 할당
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
            Destroy(gameObject);    // 자신을 파괴

        _allKillMission = FindObjectOfType<AllKillMission>();
    }
}
