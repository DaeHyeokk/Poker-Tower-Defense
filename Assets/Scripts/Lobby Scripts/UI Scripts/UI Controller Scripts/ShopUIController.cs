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
        // 플레이어가 로그인 하지 않은 경우 모든 인앱 상품 버튼을 비활성화 한다.
        if(!Social.localUser.authenticated)
            for (int i = 0; i < _disablePanels.Length; i++)
                DisablePurchashingButton((IAPManager.ProductName)i, false);
        else
        {
            // 유니티 IAP가 성공적으로 초기화 된 경우에만 수행.
            if (IsIAPInit())
            {

                // 플레이어가 로그인 한 경우 플레이어가 구매한 인앱 상품 버튼을 비활성화 한다.
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
        // 유니티 IAP가 성공적으로 초기화 된 경우에만 수행.
        if (IsIAPInit())
        {

            // 플레이어가 로그인 한 경우 플레이어가 구매한 인앱 상품 버튼을 비활성화 한다.
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

    public void DisablePurchashingButton(IAPManager.ProductName productType, bool isBuying)
    {
        _inAppProductButtons[(int)productType].interactable = false;

        DisablePanel disablePanel = _disablePanels[(int)productType];

        // disable Panel 오브젝트 활성화.
        disablePanel.panelObject.SetActive(true);

        if (isBuying)
        {
            // Lock Image를 비활성화 하고 텍스트를 "구매 완료", 색깔은 초록색으로 설정.
            disablePanel.lockImageObject.SetActive(false);
            disablePanel.disableTypeText.text = "구매 완료";
            disablePanel.disableTypeText.color = Color.green;
        }
        else
        {
            // Lock Image를 활성화 하고 텍스트를 "로그인 후 구매 가능", 색깔은 빨간색으로 설정.
            disablePanel.lockImageObject.SetActive(true);
            disablePanel.disableTypeText.text = "로그인 후 구매 가능";
            disablePanel.disableTypeText.color = Color.red;
        }
    }
}
