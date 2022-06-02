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

    private GambleUIController _gambleUIController;
    private CardDrawer _cardDrawer;

    // ������ ����(Ÿ������, �̳׶��̱�) ī�带 �̴����� ��Ÿ���� ����
    private GambleType _gambleType;
    private bool _isGambling;

    public GambleType gambleType => _gambleType;
    public int[] mineralGambleAmounts => _mineralGambleAmounts;

    private void Awake()
    {
        _gambleUIController = GetComponent<GambleUIController>();
        _cardDrawer = new CardDrawer();

        _isGambling = false;
    }

    public void StartGamble(int gambleType)
    {
        // �̹� Gamble�� �����ϰ� �ִ� ���̶�� �������� �ʴ´�. (Ȥ�ø� ���� ����)
        if (_isGambling) return;

        // Gamble�� �����ϱ� ���� 100��带 �����ϰ� ���� �ʴٸ� �������� �ʴ´�.
        if (GameManager.instance.gold < 100)
        {
            UIManager.instance.ShowSystemMessage("��尡 �����մϴ�.");
            return;
        }

        // �÷��̾��� ��忡�� 100��带 �����Ѵ�.
        GameManager.instance.gold -= 100;

        if (gambleType == (int)GambleType.Tower)
        {
            // Gambler�� gamble type�� Tower�� ����
            _gambleType = GambleType.Tower;
        }
        else
        {
            // Gambler�� gamble type�� Mineral�� ����
            _gambleType = GambleType.Mineral;
        }

        // Gamble�� ���� ���� ���·� �ٲ۴�.
        _isGambling = true;

        // �÷��̾� ȭ�鿡 ���µ� ī�带 ��� �����´�.
        _gambleUIController.AllReverseCardBackUI();
        // �̱� ��ư�� ȭ�鿡�� �����.
        _gambleUIController.HideDrawButtonUI();

        // ī�带 �̴´�.
        _cardDrawer.DrawCardAll();

        // �÷��̾� ȭ�鿡 ���� ���� ī�带 �����ش�.
        //StartCoroutine(AllReverseCardFrontUICoroutine());
        AllReverseCardFrontUICoroutine();
    }

    public void ChangeCard(int changeIndex)
    {
        // �÷��̾��� ChangeChance Ƚ���� 0 ���϶�� �������� �ʴ´�.
        if (GameManager.instance.changeChance <= 0)
        {
            UIManager.instance.ShowSystemMessage("ī�� ��ȯ���� �����մϴ�.");
            return;
        }

        // �÷��̾��� ChangeChance Ƚ���� 1 �����Ѵ�.
        GameManager.instance.changeChance--;

        // �÷��̾� ȭ�鿡 ���µ� ī�� �� �ٲ� ī�带 �����´�.
        _gambleUIController.ReverseCardBackUI(changeIndex);

        // ī�带 �ٲ۴�.
        _cardDrawer.ChangeCard(changeIndex);

        // �ٲ� ī�带 �÷��̾�� �����ش�.
        StartCoroutine(ReverseCardFrountUICoroutine(changeIndex));
        
    }

    private void AllReverseCardFrontUICoroutine()
    {
       // yield return new WaitForSeconds(0.1f);

        for (int index = 0; index < _cardDrawer.drawCards.Length; index++)
        {
            _gambleUIController.ReverseCardFrountUI(index, _cardDrawer.drawCards[index]);
            //yield return new WaitForSeconds(0.1f);
        }

        ShowResultUI();
    }
    private IEnumerator ReverseCardFrountUICoroutine(int index)
    {
        yield return new WaitForSeconds(0.1f);

        _gambleUIController.ReverseCardFrountUI(index, _cardDrawer.drawCards[index]);

        ShowResultUI();
    }
    private void ShowResultUI()
    {
        _gambleUIController.SetHandUI(_cardDrawer.drawHand);

        if (_gambleType == GambleType.Tower)
            _gambleUIController.SetTowerPreviewUI((int)_cardDrawer.drawHand);
        else
            _gambleUIController.SetMineralPreviewUI(_mineralGambleAmounts[(int)_cardDrawer.drawHand]);

        _gambleUIController.ShowGetButtonUI();
    }

    public void GetResult()
    {
        if (gambleType == GambleType.Tower)
            TowerBuilder.instance.BuildTower((int)_cardDrawer.drawHand);
        else
            GameManager.instance.mineral += _mineralGambleAmounts[(int)_cardDrawer.drawHand];

        ResetGambler();
    }

    public void ResetGambler()
    {
        _isGambling = false;
        _cardDrawer.ResetDrawer();

        // ī�带 ��� �����´�.
        _gambleUIController.AllReverseCardBackUI();
        // Ÿ���� Get �ϴ� ��ư�� ȭ�鿡�� �����.
        _gambleUIController.HideGetButtonUI();
        // ������� ������ ��Ÿ���� �ؽ�Ʈ�� ȭ�鿡�� �����.
        _gambleUIController.HideHandTextUI();
        // ī�� Draw ��ư�� ȭ�鿡 ��Ÿ����.
        _gambleUIController.ShowDrawButtonUI();
    }
}
