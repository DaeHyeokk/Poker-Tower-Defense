using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private ShopUIController _shopUIController;

    private void Awake()
    {
        IAPManager.instance.OnPurchashingSuccessed += () => _shopUIController.ShowBuyResultPanel(true);
        IAPManager.instance.OnPurchashingFailed += (errorMessage) => _shopUIController.ShowBuyResultPanel(false, errorMessage);
    }

    public void OnClickAdsRemoveBuyBtn() => IAPManager.instance.Purchase(IAPManager.instance.productAdsRemove);

    public void OnClickExtraGameSpeedBuyBtn() => IAPManager.instance.Purchase(IAPManager.instance.productExtraGameSpeed);

    public void OnClickPremiumPassBuyBtn() => IAPManager.instance.Purchase(IAPManager.instance.productPremiumPass);
}
