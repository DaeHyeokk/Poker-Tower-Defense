using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIController : MonoBehaviour
{
    [SerializeField]
    private Button _collectMissionButton;
    [SerializeField]
    private Button _killMissionButton;
    [SerializeField]
    private ScrollRect _collectMissionScrollView;
    [SerializeField]
    private ScrollRect _killMissionScrollView;

    public void OnClickMissionButton()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickCollectMissionButton()
    {
        _collectMissionButton.interactable = false;
        _collectMissionScrollView.gameObject.SetActive(true);

        _killMissionButton.interactable = true;
        _killMissionScrollView.gameObject.SetActive(false);
    }

    public void OnClickKillMissionButton()
    {
        _killMissionButton.interactable = false;
        _killMissionScrollView.gameObject.SetActive(true);

        _collectMissionButton.interactable = true;
        _collectMissionScrollView.gameObject.SetActive(false);
    }
}
