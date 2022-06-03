using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GambleUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardDrawCanvas;
    [SerializeField]
    private Image[] _cardImages;
    [SerializeField]
    private Button[] _changeButtons;
    [SerializeField]
    private TextMeshProUGUI _handText;
    [SerializeField]
    private Sprite _cardBackSprite;
    // Index�� ���ϴ� ��������Ʈ�� �����ϱ� ������ �迭�� ����ϴ� ���� �߿�!
    [SerializeField]
    private Sprite[] _cardSprites;

    [Header("Gamble Button UI Canvas")]
    [SerializeField]
    private Button _towerGambleButton;
    [SerializeField]
    private Button _mineralGambleButton;
    [SerializeField]
    private Button _getButton;
    [SerializeField]
    private Image _towerPreviewImage;
    [SerializeField]
    private Sprite[] _towerSprites;
    [SerializeField]
    private TextMeshProUGUI _mineralGetText;

    public void ReverseCardFrountUI(int index, Card card)
    {
        _cardImages[index].sprite = _cardSprites[card.index];
        _changeButtons[index].gameObject.SetActive(true);
    }

    public void AllReverseCardBackUI()
    {
        for (int index = 0; index < GameManager.instance.pokerCount; index++)
        {
            ReverseCardBackUI(index);
            HideChangeButton(index);
        }
    }

    public void ReverseCardBackUI(int index) => _cardImages[index].sprite = _cardBackSprite;

    public void ShowChangeButton(int index) => _changeButtons[index].gameObject.SetActive(true);

    public void HideChangeButton(int index) => _changeButtons[index].gameObject.SetActive(false);

    public void SetHandUI(PokerHand drawHand)
    {
        _handText.text = drawHand.ToString();
        _handText.gameObject.SetActive(true);
    }

    public void HideHandTextUI() => _handText.gameObject.SetActive(false);

    public void HideGambleButtonUI()
    {
        _towerGambleButton.gameObject.SetActive(false);
        _mineralGambleButton.gameObject.SetActive(false);
    }

    public void ShowGambleButtonUI()
    {
        _towerGambleButton.gameObject.SetActive(true);
        _mineralGambleButton.gameObject.SetActive(true);
    }

    public void EnableGambleButtonUI()
    {
        _towerGambleButton.interactable = true;
        _mineralGambleButton.interactable = true; ;

    }

    public void DisableGambleButtonUI()
    {
        _towerGambleButton.interactable = false;
        _mineralGambleButton.interactable = false;
    }
    public void SetTowerPreviewUI(int towerIndex)
    {
        _towerPreviewImage.sprite = _towerSprites[towerIndex];
        _towerPreviewImage.gameObject.SetActive(true);
    }

    public void SetMineralPreviewUI(int mineralAmount)
    {
        _mineralGetText.text = '+' + mineralAmount.ToString() + 'M';
        _mineralGetText.gameObject.SetActive(true);
    }
    public void HideGetButtonUI()
    {
        _getButton.gameObject.SetActive(false);
        _towerPreviewImage.gameObject.SetActive(false);
        _mineralGetText.gameObject.SetActive(false);
    }

    public void ShowGetButtonUI() => _getButton.gameObject.SetActive(true);
}


/*
 * File : GambleUIController.cs
 * First Update : 2022/04/27 WED 11:10
 * �ΰ��ӿ��� ī��� ���õ� UI�� �����ϴ� ��ũ��Ʈ.
 * ��ü UI�� ������ UIManager���� �����ϱ� ������
 * UIManager�� ���ǵ� �޼������ ȣ���Ͽ� UI�� �����Ѵ�.
 * 
 * Update : 2022/06/02 THU 20:10
 * UIManager�� ���ǵ� �޼��带 ȣ���Ͽ� UI�� �����ϴ� ����� �ƴ� ���� UI ��Ҹ� �����ؼ� �����ϴ� ������� ����.
 */