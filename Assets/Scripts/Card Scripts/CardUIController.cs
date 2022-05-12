
public class CardUIController
{
    public void ReverseCardFrountUI(int index, Card card)
    {
        UIManager.instance.ReverseCardFront(index, card);
        UIManager.instance.ShowChangeButton(index);
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

    public void SetHandUI(PokerHand drawHand)
    {
        UIManager.instance.SetHandText(drawHand.ToString());
        UIManager.instance.ShowHandText();
    }

    public void SetTowerPreviewUI(int towerIndex)
    {
        UIManager.instance.SetTowerPreviewImage(towerIndex);
        UIManager.instance.ShowTowerPreviewImage();
    }

    public void SetMineralPreviewUI(int mineralAmount)
    {
        UIManager.instance.SetMineralGetText(mineralAmount);
        UIManager.instance.ShowMineralGetText();
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
        UIManager.instance.HideMineralGetText();
    }
    public void ShowGetButtonUI() => UIManager.instance.ShowGetButton();
}


/*
 * File : CardUIController.cs
 * First Update : 2022/04/27 WED 11:10
 * 인게임에서 카드와 관련된 UI를 제어하는 스크립트.
 * 전체 UI의 변경은 UIManager에서 수행하기 때문에
 * UIManager에 정의된 메서드들을 호출하여 UI를 제어한다.
 */