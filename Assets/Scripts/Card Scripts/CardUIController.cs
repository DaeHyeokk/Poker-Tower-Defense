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
        // �÷��̾��� ȭ�鿡 ī�带 ��� ǥ���ϰ� ���Ŀ� �÷��̾�� Ÿ���� ���� �� �ִ� ���°� �ȴ�.
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
 * �ΰ��ӿ��� ī��� ���õ� UI�� �����ϴ� ��ũ��Ʈ.
 * ��ü UI�� ������ UIManager���� �����ϱ� ������
 * UIManager�� ���ǵ� �޼������ ȣ���Ͽ� UI�� �����Ѵ�.
 */