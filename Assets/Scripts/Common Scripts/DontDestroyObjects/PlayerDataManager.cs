using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager _instance;
    public static PlayerDataManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayerDataManager>();

            return _instance;
        }
    }

    private readonly Dictionary<string, KeyValuePair<int, int>> _towerDataDict = new();
    public Dictionary<string, KeyValuePair<int, int>> towerDataDict => _towerDataDict;
    
    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        else
        {
            // SoundManager가 최초 생성되는 경우.
            // 씬이 종료되어도 파괴되지 않는 오브젝트로 설정한다.
            DontDestroyOnLoad(instance);
            // 로컬디스크에 저장된 PlayerPrefs 데이터를 가져와 동기화한다.
            LoadPlayerPrefsTowerData();
        }
    }

    private void LoadPlayerPrefsTowerData()
    {
        // 로컬디스크에 저장된 타워의 레벨과 킬수를 로드하여 Dictionary에 담아둔다.
        for(int i=0; i<Tower.towerTypeNames.Length; i++)
        {
            string towerName = Tower.towerTypeNames[i];
            int level = PlayerPrefs.GetInt(towerName + "레벨");
            int killCount = PlayerPrefs.GetInt(towerName + "킬수");

            if (level == 0) level = 1;

            _towerDataDict.Add(Tower.towerTypeNames[i], new KeyValuePair<int, int>(level, killCount));
        }
    }

    public void SavePlayerPrefsTowerData()
    {
        for(int i=0; i<Tower.towerTypeNames.Length; i++)
        {
            string towerName = Tower.towerTypeNames[i];
            int level = _towerDataDict[towerName].Key;
            int killCount = _towerDataDict[towerName].Value;

            PlayerPrefs.SetInt(towerName + "레벨", level);
            PlayerPrefs.SetInt(towerName + "킬수", killCount);
        }

        // 중요한 데이터이므로 Application Pause 이벤트가 호출될 때 저장하지 않고 갱신될 때 바로 저장한다.
        PlayerPrefs.Save();
    }
}
