using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 ObjectPool 오브젝트를 찾아 할당
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    [Header("Used by CardDrawer Component")]
    [SerializeField]
    private Image[] _cardImages;
    [SerializeField]
    private Image _towerPreviewImage;
    [SerializeField]
    private Button[] _changeButtons;
    [SerializeField]
    private TextMeshProUGUI _handText;
    [SerializeField]
    private Button _drawButton;
    [SerializeField]
    private Button _getButton;
    [SerializeField]
    private Sprite _cardBackSprite;
    // Index로 원하는 스프라이트에 접근하기 때문에 배열에 등록하는 순서 중요!
    [SerializeField]
    private Sprite[] _cardSprites;
    [SerializeField]
    private Sprite[] _towerSprites;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);    // 자신을 파괴
            return;
        }
    }

    public void EnableCard(int index, Card card) => _cardImages[index].sprite = _cardSprites[card.index];

    public void DisableCard(int index) => _cardImages[index].sprite = _cardBackSprite;

    public void EnableChangeButton(int index) => _changeButtons[index].gameObject.SetActive(true);

    public void DisableChangeButton(int index) => _changeButtons[index].gameObject.SetActive(false);
    
    public void DisableHandText() => _handText.gameObject.SetActive(false);

    public void EnableHandText() => _handText.gameObject.SetActive(true);

    public void SetHandText(string handString) => _handText.text = handString;

    public void SetTowerPreviewImage(int index) => _towerPreviewImage.sprite = _towerSprites[index];

    public void DisableTowerPreviewImage() => _towerPreviewImage.gameObject.SetActive(false);

    public void EnableTowerPreviewImage() => _towerPreviewImage.gameObject.SetActive(true);

    public void DisableDrawButton() => _drawButton.gameObject.SetActive(false);

    public void EnableDrawButton() => _drawButton.gameObject.SetActive(true);

    public void DisableGetButton() => _getButton.gameObject.SetActive(false);

    public void EnableGetButton() => _getButton.gameObject.SetActive(true);
}


/*
 * File : UIManager.cs
 * First Update : 2022/04/27 WED 23:10
 * 게임에서 사용되는 모든 UI의 변경을 담당한다.
 */