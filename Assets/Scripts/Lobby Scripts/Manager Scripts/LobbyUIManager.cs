using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    private static LobbyUIManager s_instance;
    public static LobbyUIManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = FindObjectOfType<LobbyUIManager>();

            return s_instance;
        }
    }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);


    }
}
