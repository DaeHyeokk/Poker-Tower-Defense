using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IAPManager s_instance;
    public static IAPManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = FindObjectOfType<IAPManager>();

            return s_instance;
        }
    }

    public enum ProductName { AdsRemove, ExtraGameSpeed, PremiumPass }

    public readonly string productAdsRemove = "Ads Remove"; // UnConsumeable
    public readonly string productExtraGameSpeed = "Extra Game Speed"; // UnConsumeable
    public readonly string productPremiumPass = "Premium Pass"; // UnConsumeable

    public readonly string androidAdsRemoveId = "com.devdduck.pokertowerdefense.ads_remove";
    public readonly string androidExtraGameSpeedId = "com.devdduck.pokertowerdefense.extra_game_speed";
    public readonly string androidPremiumPass = "com.devdduck.pokertowerdefense.premium_pass";

    // 구매 과정을 제어하는 함수를 제공
    private IStoreController _storeController;
    // 여러 플랫폼을 위한 확장 처리를 제공
    private IExtensionProvider _extensionProvider;

    public event Action _OnPurchashingSuccessed;
    public event Action<string> _OnPurchashingFailed;

    public event Action OnPurchashingSuccessed
    {
        add
        {
            if (_OnPurchashingSuccessed != null)
                _OnPurchashingSuccessed = null;
            _OnPurchashingSuccessed += value;
        }

        remove
        {
            _OnPurchashingSuccessed = null;
        }
    }

    public event Action<string> OnPurchashingFailed
    {
        add
        {
            if (_OnPurchashingFailed != null)
                _OnPurchashingFailed = null;
            _OnPurchashingFailed += value;
        }

        remove
        {
            _OnPurchashingFailed = null;
        }
    }

    public bool isInitialized => _storeController != null && _extensionProvider != null;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            // IAPManager가 최초 생성되는 경우.
            // 씬이 종료되어도 파괴되지 않는 오브젝트로 설정한다.
            DontDestroyOnLoad(instance);
        }

        InitUnityIAP();
    }

    private void InitUnityIAP()
    {
        // 이미 UnityIAP를 인스턴스 하는데 성공했다면 수행하지 않는다.
        if (isInitialized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(
            productAdsRemove, ProductType.NonConsumable, new IDs()
            {
                { androidAdsRemoveId, GooglePlay.Name }
            }
            );

        builder.AddProduct(
            productExtraGameSpeed, ProductType.NonConsumable, new IDs()
            {
                { androidExtraGameSpeedId, GooglePlay.Name }
            }
            );

        builder.AddProduct(
            productPremiumPass, ProductType.NonConsumable, new IDs()
            {
                { androidPremiumPass, GooglePlay.Name }
            }
            );

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        _storeController = controller;
        _extensionProvider = extension;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"유니티 IAP 초기화 실패 {error}");
    }

    public void ReInitialized()
    {
        _storeController = null;
        _extensionProvider = null;
        InitUnityIAP();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string purchaseProductId = args.purchasedProduct.definition.id;

        if(_OnPurchashingSuccessed != null)
            _OnPurchashingSuccessed();

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        string errorMessage;

        switch (reason)
        {
            case PurchaseFailureReason.PurchasingUnavailable:
                errorMessage = "구매 불가 상품";
                break;
            case PurchaseFailureReason.ExistingPurchasePending:
                errorMessage = "기존 구매 보류중";
                break;
            case PurchaseFailureReason.ProductUnavailable:
                errorMessage = "사용할 수 없는 상품";
                break;
            case PurchaseFailureReason.SignatureInvalid:
                errorMessage = "서명이 잘못됨";
                break;
            case PurchaseFailureReason.UserCancelled:
                errorMessage = "구매 취소";
                break;
            case PurchaseFailureReason.PaymentDeclined:
                errorMessage = "결제 거절";
                break;
            case PurchaseFailureReason.DuplicateTransaction:
                errorMessage = "중복 거래";
                break;
            default:
                errorMessage = "알 수 없는 에러";
                break;
        }


        if(_OnPurchashingFailed != null)
        {
            _OnPurchashingFailed(errorMessage);
        }
    }

    public void Purchase(string productId)
    {
        if (!isInitialized) return;

        Product product = _storeController.products.WithID(productId);

        // product가 존재하고, 사용 가능한 상태라면 구매 시도
        if (product != null && product.availableToPurchase)
            _storeController.InitiatePurchase(product);
    }

    // Apple 스토어 구현할 때 꼭 들어가야 하는 메서드. (구매 복구)
    public void RestorePurchase()
    {
        if (!isInitialized) return;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // 구매 복구 시도 로직
            var appleExt = _extensionProvider.GetExtension<IAppleExtensions>();

            appleExt.RestoreTransactions(
                result => Debug.Log($"구매 복구 시도 결과: {result}"));
        }
    }

    public bool HadPurchashed(string productId)
    {
        if (!isInitialized) return false;

        Product product = _storeController.products.WithID(productId);

        if (product != null)
            return product.hasReceipt;
        else
            return false;
    }
}
