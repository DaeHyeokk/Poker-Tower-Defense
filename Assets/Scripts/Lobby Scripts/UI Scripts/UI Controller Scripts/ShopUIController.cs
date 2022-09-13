using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIController : MonoBehaviour
{
    [System.Serializable]
    private struct BuyResultPanel
    {
        public GameObject panelCanvasObject;
        public TextMeshProUGUI buyResultText;
    }
    [System.Serializable]
    private struct DisablePanel
    {
        public GameObject panelObject;
        public GameObject lockImageObject;
        public TextMeshProUGUI disableTypeText;
    }

    [SerializeField]
    private BuyResultPanel _buyResultPanel;
    [SerializeField]
    private DisablePanel[] _disablePanels;
    [SerializeField]
    private Button[] _inAppProductButtons;
    [SerializeField]
    private GameObject _iapNotInitPanelObject;

    private void Awake()
    {
        // �÷��̾ �α��� ���� ���� ��� ��� �ξ� ��ǰ ��ư�� ��Ȱ��ȭ �Ѵ�.
        if(!Social.localUser.authenticated)
            for (int i = 0; i < _disablePanels.Length; i++)
                DisablePurchashingButton((IAPManager.ProductName)i, false);
        else
        {
            // ����Ƽ IAP�� ���������� �ʱ�ȭ �� ��쿡�� ����.
            if (IsIAPInit())
            {

                // �÷��̾ �α��� �� ��� �÷��̾ ������ �ξ� ��ǰ ��ư�� ��Ȱ��ȭ �Ѵ�.
                if (Social.localUser.authenticated)
                {
                    if (IAPManager.instance.HadPurchashed(IAPManager.instance.productAdsRemove))
                        DisablePurchashingButton(IAPManager.ProductName.AdsRemove, true);

                    if (IAPManager.instance.HadPurchashed(IAPManager.instance.productExtraGameSpeed))
                        DisablePurchashingButton(IAPManager.ProductName.ExtraGameSpeed, true);

                    if (IAPManager.instance.HadPurchashed(IAPManager.instance.productPremiumPass))
                        DisablePurchashingButton(IAPManager.ProductName.PremiumPass, true);
                }
            }
        }
    }

    private void Update()
    {
        // ����Ƽ IAP�� ���������� �ʱ�ȭ �� ��쿡�� ����.
        if (IsIAPInit())
        {

            // �÷��̾ �α��� �� ��� �÷��̾ ������ �ξ� ��ǰ ��ư�� ��Ȱ��ȭ �Ѵ�.
            if (Social.localUser.authenticated)
            {
                if (IAPManager.instance.HadPurchashed(IAPManager.instance.productAdsRemove))
                    DisablePurchashingButton(IAPManager.ProductName.AdsRemove, true);

                if (IAPManager.instance.HadPurchashed(IAPManager.instance.productExtraGameSpeed))
                    DisablePurchashingButton(IAPManager.ProductName.ExtraGameSpeed, true);

                if (IAPManager.instance.HadPurchashed(IAPManager.instance.productPremiumPass))
                    DisablePurchashingButton(IAPManager.ProductName.PremiumPass, true);
            }
        }
    }

    // Unity IAP�� �ʱ�ȭ �Ǿ����� ���ο� ���� ��ǰ ����Ʈ�� �ҷ����� ���̶�� ������ �ǳ��� Ȱ��ȭ �ϰų� ��Ȱ��ȭ �ϴ� �޼���.
    private bool IsIAPInit()
    {
        if (IAPManager.instance.isInitialized)
        {
            _iapNotInitPanelObject.SetActive(false);
            return true;
        }
        else
        {
            _iapNotInitPanelObject.SetActive(true);
            return false;
        }
    }

    public void ShowBuyResultPanel(bool isSuccessed, string contents = null)
    {
        if (isSuccessed)
            _buyResultPanel.buyResultText.text = "���� ����";
        else
            _buyResultPanel.buyResultText.text = "���� ����\n" + contents;

        _buyResultPanel.panelCanvasObject.SetActive(true);
    }

    public void DisablePurchashingButton(IAPManager.ProductName productType, bool isBuying)
    {
        _inAppProductButtons[(int)productType].interactable = false;

        DisablePanel disablePanel = _disablePanels[(int)productType];

        // disable Panel ������Ʈ Ȱ��ȭ.
        disablePanel.panelObject.SetActive(true);

        if (isBuying)
        {
            // Lock Image�� ��Ȱ��ȭ �ϰ� �ؽ�Ʈ�� "���� �Ϸ�", ������ �ʷϻ����� ����.
            disablePanel.lockImageObject.SetActive(false);
            disablePanel.disableTypeText.text = "���� �Ϸ�";
            disablePanel.disableTypeText.color = Color.green;
        }
        else
        {
            // Lock Image�� Ȱ��ȭ �ϰ� �ؽ�Ʈ�� "�α��� �� ���� ����", ������ ���������� ����.
            disablePanel.lockImageObject.SetActive(true);
            disablePanel.disableTypeText.text = "�α��� �� ���� ����";
            disablePanel.disableTypeText.color = Color.red;
        }
    }
}
