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
        // 유니티 IAP가 성공적으로 초기화 된 경우에만 수행.
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
        // 유니티 IAP가 성공적으로 초기화 된 경우에만 수행.
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

    // Unity IAP가 초기화 되었는지 여부에 따라 상품 리스트를 불러오는 중이라는 내용의 판넬을 활성화 하거나 비활성화 하는 메서드.
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
            _buyResultPanel.buyResultText.text = "구매 성공";
        else
            _buyResultPanel.buyResultText.text = "구매 실패\n" + contents;

        _buyResultPanel.panelCanvasObject.SetActive(true);
    }

    private void DisablePurchashingButton(IAPManager.ProductName productType)
    {
        _inAppProductButtons[(int)productType].interactable = false;

        // Completion Panel 오브젝트 활성화.
        _completionPanels[(int)productType].SetActive(true);
    }
}
