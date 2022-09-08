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

    private void OnEnable() => SoundManager.instance.PlaySFX(SoundFileNameDictionary.popupUIShowSound);

    public void UnlockDifficultyButton(int clearStage)
    {
        switch(clearStage)
        {
            case (int)StageManager.StageDifficulty.Easy:
                _normalButton.interactable = true;
                _normalLockPanelObject.SetActive(false);
                break;

            case (int)StageManager.StageDifficulty.Normal:
                _normalButton.interactable = true;
                _normalLockPanelObject.SetActive(false);
                _hardButton.interactable = true;
                _hardLockPanelObject.SetActive(false);
                break;

            case (int)StageManager.StageDifficulty.Hard:
                _normalButton.interactable = true;
                _normalLockPanelObject.SetActive(false);
                _hardButton.interactable = true;
                _hardLockPanelObject.SetActive(false);
                _hellButton.interactable = true;
                _hellLockPanelObject.SetActive(false);
                break;

            case (int)StageManager.StageDifficulty.Hell:
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
