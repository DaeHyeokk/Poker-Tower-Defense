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

    public void AllReverseCardFrontUI()
    {
        UIManager.instance.HideHandText();
        StartCoroutine(AllReverseCardFrontUICoroutine());
    }

    private IEnumerator AllReverseCardFrontUICoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        for (int index = 0; index < _cardDrawer.drawCards.Length; index++)
        {
            UIManager.instance.ReverseCardFront(index, _cardDrawer.drawCards[index]);
            UIManager.instance.ShowChangeButton(index);
            yield return new WaitForSeconds(0.1f);
        }

        SetHandUI();
        UIManager.instance.ShowGetButton();
        // �÷��̾��� ȭ�鿡 ī�带 ��� ǥ���ϰ� ���Ŀ� �÷��̾�� Ÿ���� ���� �� �ִ� ���°� �ȴ�.
        _cardDrawer.ReadyBuildTower();
    }

    public void ReverseCardFrountUI(int index)
    {
        StartCoroutine(ReverseCardFrountUICoroutine(index));
    }

    private IEnumerator ReverseCardFrountUICoroutine(int index)
    {
        yield return new WaitForSeconds(0.1f);

        UIManager.instance.ReverseCardFront(index, _cardDrawer.drawCards[index]);
        UIManager.instance.ShowChangeButton(index);
        SetHandUI();
    }
    public void AllReverseCardBackUI()
    {
        for (int index = 0; index < GameManager.instance.pokerCount; index++)
        {
            ReverseCardBackUI(index);
            HideChangeButton(index);
        }
    }

    public void ReverseCardBackUI(int index) => UIManager.instance.ReverseCardBack(index);

    public void ShowChangeButton(int index) => UIManager.instance.ShowChangeButton(index);

    public void HideChangeButton(int index) => UIManager.instance.HideChangeButton(index);

    public void SetHandUI()
    {
        UIManager.instance.SetHandText(_cardDrawer.drawHand.ToString());
        UIManager.instance.ShowHandText();
        UIManager.instance.SetTowerPreviewImage((int)_cardDrawer.drawHand);
        UIManager.instance.ShowTowerPreviewImage();
    }

    public void HideHandTextUI() => UIManager.instance.HideHandText();

    public void HideDrawButtonUI()
    {
        UIManager.instance.HideTowerGambleButton();
        UIManager.instance.HideMineralGambleButton();
    }
    public void ShowDrawButtonUI()
    {
        UIManager.instance.ShowTowerGambleButton();
        UIManager.instance.ShowMineralGambleButton();
    }
    public void HideGetButtonUI()
    {
        UIManager.instance.HideGetButton();
        UIManager.instance.HideTowerPreviewImage();
    }

    public void ShowGetButtonUI() => UIManager.instance.ShowGetButton();
}


/*
 * File : CardUIController.cs
 * First Update : 2022/04/27 WED 11:10
 * �ΰ��ӿ��� ī��� ���õ� UI�� �����ϴ� ��ũ��Ʈ.
 * ��ü UI�� ������ UIManager���� �����ϱ� ������
 * UIManager�� ���ǵ� �޼������ ȣ���Ͽ� UI�� �����Ѵ�.
 */