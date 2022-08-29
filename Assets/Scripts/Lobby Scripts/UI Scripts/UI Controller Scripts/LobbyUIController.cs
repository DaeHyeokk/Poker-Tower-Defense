using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField]
    private DifficultySelectUIController _difficultySelectUIController;

    public void ShowDifficultySelectUI() => _difficultySelectUIController.gameObject.SetActive(true);
}
