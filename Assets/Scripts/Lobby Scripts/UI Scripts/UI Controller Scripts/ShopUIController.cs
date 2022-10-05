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

    [SerializeField]
    private BuyResultPanel _buyResultPanel;
    [SerializeField]
    private GameObject[] _completionPanels;
    [SerializeField]
    private Button[] _inAppProductButtons;
    [SerializeField]
    private GameObject _iapNotInitPanelObject;

    private void Awake()
    {
        // ����Ƽ IAP�� ���������� �ʱ�ȭ �� ��쿡�� ����.
        if (IsIAPInit())
        {
            if (IAPManager.instance.HadPurchashed(IAPManager.instance.productAdsRemove))
                DisablePurchashingButton(IAPManager.ProductName.AdsRemove);

            if (IAPManager.instance.HadPurchashed(IAPManager.instance.productExtraGameSpeed))
                DisablePurchashingButton(IAPManager.ProductName.ExtraGameSpeed);

            if (IAPManager.instance.HadPurchashed(IAPManager.instance.productPremiumPass))
                DisablePurchashingButton(IAPManager.ProductName.PremiumPass);
        }
    }

    private void Update()
    {
        // ����Ƽ IAP�� ���������� �ʱ�ȭ �� ��쿡�� ����.
        if (IsIAPInit())
        {
            if (IAPManager.instance.HadPurchashed(IAPManager.instance.productAdsRemove))
                DisablePurchashingButton(IAPManager.ProductName.AdsRemove);

            if (IAPManager.instance.HadPurchashed(IAPManager.instance.productExtraGameSpeed))
                DisablePurchashingButton(IAPManager.ProductName.ExtraGameSpeed);

            if (IAPManager.instance.HadPurchashed(IAPManager.instance.productPremiumPass))
                DisablePurchashingButton(IAPManager.ProductName.PremiumPass);
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

    private void DisablePurchashingButton(IAPManager.ProductName productType)
    {
        _inAppProductButtons[(int)productType].interactable = false;

        // Completion Panel ������Ʈ Ȱ��ȭ.
        _completionPanels[(int)productType].SetActive(true);
    }
}
