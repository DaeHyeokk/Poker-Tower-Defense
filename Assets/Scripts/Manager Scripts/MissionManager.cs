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

    private AllKillMission _allKillMission;
    private CutItCloseMission _cutItCloseMission;
    private AtTheGatesOfHellMission _atTheGatesOfHellMission;
    private PlanetDestroyerMission _planetDestroyerMission;

    public AllKillMission allKillMission => _allKillMission;
    public CutItCloseMission cutItCloseMission => _cutItCloseMission;
    public AtTheGatesOfHellMission atTheGatesOfHellMission => _atTheGatesOfHellMission;
    public PlanetDestroyerMission planetDestroyerMission => _planetDestroyerMission;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);    // �ڽ��� �ı�

        _allKillMission = FindObjectOfType<AllKillMission>();
        _cutItCloseMission = FindObjectOfType<CutItCloseMission>();
        _atTheGatesOfHellMission = FindObjectOfType<AtTheGatesOfHellMission>();
        _planetDestroyerMission = FindObjectOfType<PlanetDestroyerMission>();
    }
}
