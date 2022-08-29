using System;
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


    public event Action onShowMissionDetailUI;
    public event Action onHideMissionDetailUI;

    private void ShowMissionDetailUI()
    {
        if (onShowMissionDetailUI != null)
            onShowMissionDetailUI();

        gameObject.SetActive(true);

        SoundManager.instance.PlaySFX("Mission UI Show Sound");
    }

    private void HideMissionDetailUI()
    {
        if (onHideMissionDetailUI != null)
            onHideMissionDetailUI();

        gameObject.SetActive(false);

        SoundManager.instance.PlaySFX("Mission UI Hide Sound");
    }

    public void OnClickMissionButton()
    {
        if (!gameObject.activeSelf)
            ShowMissionDetailUI();
        else
            HideMissionDetailUI();
    }

    public void OnClickCloseButton()
    {
        HideMissionDetailUI();
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
