using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    private static LobbyUIManager _instance;
    public static LobbyUIManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LobbyUIManager>();

            return _instance;
        }
    }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);


    }
}
