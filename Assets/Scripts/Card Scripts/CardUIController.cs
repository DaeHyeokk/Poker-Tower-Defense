using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIController : MonoBehaviour
{
    private CardDrawer _cardDrawer;

    private void Awake()
    {
        _cardDrawer = GetComponent<CardDrawer>();
    }

    public void AllEnableCardUI()
    {
        UIManager.instance.DisableHandText();
        StartCoroutine(AllEnableCardUICoroutine());
    }

    private IEnumerator AllEnableCardUICoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        for (int index = 0; index < _cardDrawer.drawCards.Length; index++)
        {
            UIManager.instance.EnableCard(index, _cardDrawer.drawCards[index]);
            UIManager.instance.EnableChangeButton(index);
            yield return new WaitForSeconds(0.15f);
        }

        SetHandUI();
        UIManager.instance.EnableGetButton();
        // 플레이어의 화면에 카드를 모두 표시하고난 이후에 플레이어는 타워를 지을 수 있는 상태가 된다.
        _cardDrawer.ReadyBuildTower();
        //
        _cardDrawer.ReadyDrawCard();
    }

    public void EnableCardUI(int index)
    {
      //  UIManager.instance.DisableHandText();
        StartCoroutine(EnableCardUICoroutine(index));
    }

    private IEnumerator EnableCardUICoroutine(int index)
    {
        yield return new WaitForSeconds(0.15f);

        UIManager.instance.EnableCard(index, _cardDrawer.drawCards[index]);
        UIManager.instance.EnableChangeButton(index);
        SetHandUI();
    }
    public void AllDisableCardUI()
    {
        for (int index = 0; index < GameManager.instance.pokerCount; index++)
        {
            DisableCardUI(index);
            DisableChangeButton(index);
        }
    }

    public void DisableCardUI(int index) => UIManager.instance.DisableCard(index);

    public void EnableChangeButton(int index) => UIManager.instance.EnableChangeButton(index);

    public void DisableChangeButton(int index) => UIManager.instance.DisableChangeButton(index);

    public void SetHandUI()
    {
        UIManager.instance.SetHandText(_cardDrawer.drawHand.ToString());
        UIManager.instance.EnableHandText();
        UIManager.instance.SetTowerPreviewImage((int)_cardDrawer.drawHand);
        UIManager.instance.EnableTowerPreviewImage();
    }

    public void DisableHandTextUI() => UIManager.instance.DisableHandText();

    public void DisableDrawButtonUI() => UIManager.instance.DisableDrawButton();

    public void EnableDrawButtonUI() => UIManager.instance.EnableDrawButton();

    public void DisableGetButtonUI()
    {
        UIManager.instance.DisableGetButton();
        UIManager.instance.DisableTowerPreviewImage();
    }

    public void EnableGetButtonUI() => UIManager.instance.EnableGetButton();
}


/*
 * File : CardUIController.cs
 * First Update : 2022/04/27 WED 11:10
 * 인게임에서 카드와 관련된 UI를 제어하는 스크립트.
 * 전체 UI의 변경은 UIManager에서 수행하기 때문에
 * UIManager에 정의된 메서드들을 호출하여 UI를 제어한다.
 */