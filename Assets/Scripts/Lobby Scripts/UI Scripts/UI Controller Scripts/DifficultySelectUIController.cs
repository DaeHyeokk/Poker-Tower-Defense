using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectUIController : MonoBehaviour
{
    [SerializeField]
    private Button _normalButton;
    [SerializeField]
    private GameObject _normalLockPanelObject;
    [SerializeField]
    private Button _hardButton;
    [SerializeField]
    private GameObject _hardLockPanelObject;
    [SerializeField]
    private Button _hellButton;
    [SerializeField]
    private GameObject _hellLockPanelObject;

    public void UnlockDifficultyButton(int clearStage)
    {
        Debug.Log(clearStage);
        switch(clearStage)
        {
            case (int)GameManager.StageDifficulty.Easy:
                _normalButton.interactable = true;
                _normalLockPanelObject.SetActive(false);
                break;

            case (int)GameManager.StageDifficulty.Normal:
                _normalButton.interactable = true;
                _normalLockPanelObject.SetActive(false);
                _hardButton.interactable = true;
                _hardLockPanelObject.SetActive(false);
                break;

            case (int)GameManager.StageDifficulty.Hard:
                _normalButton.interactable = true;
                _normalLockPanelObject.SetActive(false);
                _hardButton.interactable = true;
                _hardLockPanelObject.SetActive(false);
                _hellButton.interactable = true;
                _hellLockPanelObject.SetActive(false);
                break;

            case (int)GameManager.StageDifficulty.Hell:
                _normalButton.interactable = true;
                _normalLockPanelObject.SetActive(false);
                _hardButton.interactable = true;
                _hardLockPanelObject.SetActive(false);
                _hellButton.interactable = true;
                _hellLockPanelObject.SetActive(false);
                break;
        }
    }
}
