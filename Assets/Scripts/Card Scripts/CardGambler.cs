using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGambler : MonoBehaviour
{
    public enum GambleType { Tower, Mineral }

    [SerializeField]
    private TowerBuilder _towerBuilder;
    [SerializeField]
    private int[] _mineralGambleAmounts;

    private CardDrawer _cardDrawer;
    private CardUIController _cardUIController;

    // 무엇을 위해(타워짓기, 미네랄뽑기) 카드를 뽑는지를 나타내는 변수
    private GambleType _gambleType;
    private bool _isGambling;

    public GambleType gambleType => _gambleType;
    public int[] mineralGambleAmounts => _mineralGambleAmounts;

    private void Awake()
    {
        _cardDrawer = new CardDrawer();
        _cardUIController = new CardUIController();

        _isGambling = false;
    }

    public void StartGamble(int gambleType)
    {
        // 이미 Gamble을 진행하고 있는 중이라면 수행하지 않는다. (혹시모를 버그 방지)
        if (_isGambling) return;

        // Gamble을 진행하기 위한 100골드를 보유하고 있지 않다면 수행하지 않는다.
        if (GameManager.instance.gold < 100) return;

        // 플레이어의 골드에서 100골드를 차감한다.
        GameManager.instance.DecreaseGold(100);

        if (gambleType == (int)GambleType.Tower)
        {
            // Gambler의 gamble type을 Tower로 변경
            _gambleType = GambleType.Tower;
        }
        else
        {
            // Gambler의 gamble type을 Mineral로 변경
            _gambleType = GambleType.Mineral;
        }

        // Gamble을 진행 중인 상태로 바꾼다.
        _isGambling = true;

        // 플레이어 화면에 오픈된 카드를 모두 뒤집는다.
        _cardUIController.AllReverseCardBackUI();
        // 뽑기 버튼을 화면에서 숨긴다.
        _cardUIController.HideDrawButtonUI();

        // 카드를 뽑는다.
        _cardDrawer.DrawCardAll();

        // 플레이어 화면에 새로 뽑은 카드를 보여준다.
        //StartCoroutine(AllReverseCardFrontUICoroutine());
        AllReverseCardFrontUICoroutine();
    }

    public void ChangeCard(int changeIndex)
    {       
        // 플레이어의 ChangeChance 횟수가 0 이하라면 수행하지 않는다.
        if (GameManager.instance.changeChance <= 0) return;

        // 플레이어의 ChangeChance 횟수를 1 차감한다.
        GameManager.instance.DecreaseChangeChance();

        // 플레이어 화면에 오픈된 카드 중 바꿀 카드를 뒤집는다.
        _cardUIController.ReverseCardBackUI(changeIndex);

        // 카드를 바꾼다.
        _cardDrawer.ChangeCard(changeIndex);

        // 바꾼 카드를 플레이어에게 보여준다.
        //  StartCoroutine(ReverseCardFrountUICoroutine(changeIndex));
        
    }

    private void AllReverseCardFrontUICoroutine()
    {
       // yield return new WaitForSeconds(0.1f);

        for (int index = 0; index < _cardDrawer.drawCards.Length; index++)
        {
            _cardUIController.ReverseCardFrountUI(index, _cardDrawer.drawCards[index]);
            //yield return new WaitForSeconds(0.1f);
        }

        ShowResultUI();
    }
    private IEnumerator ReverseCardFrountUICoroutine(int index)
    {
        yield return new WaitForSeconds(0.1f);

        _cardUIController.ReverseCardFrountUI(index, _cardDrawer.drawCards[index]);

        ShowResultUI();
    }
    private void ShowResultUI()
    {
        _cardUIController.SetHandUI(_cardDrawer.drawHand);

        if (_gambleType == GambleType.Tower)
            _cardUIController.SetTowerPreviewUI((int)_cardDrawer.drawHand);
        else
            _cardUIController.SetMineralPreviewUI(_mineralGambleAmounts[(int)_cardDrawer.drawHand]);

        _cardUIController.ShowGetButtonUI();
    }

    public void GetResult()
    {
        if (gambleType == GambleType.Tower)
            _towerBuilder.BuildTower((int)_cardDrawer.drawHand);
        else
            GameManager.instance.IncreaseMineral(_mineralGambleAmounts[(int)_cardDrawer.drawHand]);

        ResetGambler();
    }

    public void ResetGambler()
    {
        _isGambling = false;
        _cardDrawer.ResetDrawer();

        // 카드를 모두 뒤집는다.
        _cardUIController.AllReverseCardBackUI();
        // 타워를 Get 하는 버튼을 화면에서 숨긴다.
        _cardUIController.HideGetButtonUI();
        // 만들어진 족보를 나타내는 텍스트를 화면에서 숨긴다.
        _cardUIController.HideHandTextUI();
        // 카드 Draw 버튼을 화면에 나타낸다.
        _cardUIController.ShowDrawButtonUI();
    }
}
