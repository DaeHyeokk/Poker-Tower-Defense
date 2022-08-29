using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private static LobbyManager _instance;
    public static LobbyManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LobbyManager>();
                return _instance;
            }

            return _instance;
        }
    }

    [SerializeField]
    private GameObject _difficultySelectObject;

    public void OnClickSingleModeButton()
    {
        _difficultySelectObject.SetActive(true);
    }

    public void OnClickPvpModeButton()
    {

    }

    public void OnClickGameQuitButton()
    {
        Application.Quit();
    }
}
