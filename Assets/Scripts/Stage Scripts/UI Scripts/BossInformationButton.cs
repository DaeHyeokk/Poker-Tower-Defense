using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossInformationButton : MonoBehaviour
{
    [SerializeField]
    private BossInformationUIController.BossType _bossType;
    [SerializeField]
    private int _bossIndex;

    private Button _button;
    private BossInformationUIController _bossInformationUIController;

    private void Awake()
    {
        _bossInformationUIController = GetComponentInParent<BossInformationUIController>();
        _button = GetComponent<Button>();

        _button.onClick.AddListener(SetAndShowBossDetailInfoUI);
    }

    private void SetAndShowBossDetailInfoUI()
    {
        _bossInformationUIController.SetBossDetailInfoUI(_bossType, _bossIndex);
        _bossInformationUIController.ShowBossDetailInfoPanel();
    } 

    /*
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        _bossInformationUIController.SetBossDetailInfoUI(_bossType, _bossIndex);
        _bossInformationUIController.ShowBossDetailInfoPanel();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        _bossInformationUIController.HideBossDetailInfoPanel();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        _bossInformationUIController.HideBossDetailInfoPanel();
    }
    */
}
