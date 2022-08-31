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
            // SoundManager�� ���� �����Ǵ� ���.
            // ���� ����Ǿ �ı����� �ʴ� ������Ʈ�� �����Ѵ�.
            DontDestroyOnLoad(instance);
            // ���õ�ũ�� ����� PlayerPrefs �����͸� ������ ����ȭ�Ѵ�.
            LoadPlayerPrefsTowerData();
        }
    }

    private void LoadPlayerPrefsTowerData()
    {
        // ���õ�ũ�� ����� Ÿ���� ������ ų���� �ε��Ͽ� Dictionary�� ��Ƶд�.
        for(int i=0; i<Tower.towerTypeNames.Length; i++)
        {
            string towerName = Tower.towerTypeNames[i];
            int level = PlayerPrefs.GetInt(towerName + "����");
            int killCount = PlayerPrefs.GetInt(towerName + "ų��");

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

            PlayerPrefs.SetInt(towerName + "����", level);
            PlayerPrefs.SetInt(towerName + "ų��", killCount);
        }

        // �߿��� �������̹Ƿ� Application Pause �̺�Ʈ�� ȣ��� �� �������� �ʰ� ���ŵ� �� �ٷ� �����Ѵ�.
        PlayerPrefs.Save();
    }
}
