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

    // ���� ������ �����ϴ� �Լ��� ����
    private IStoreController _storeController;
    // ���� �÷����� ���� Ȯ�� ó���� ����
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
            // IAPManager�� ���� �����Ǵ� ���.
            // ���� ����Ǿ �ı����� �ʴ� ������Ʈ�� �����Ѵ�.
            DontDestroyOnLoad(instance);
        }

        InitUnityIAP();
    }

    private void InitUnityIAP()
    {
        // �̹� UnityIAP�� �ν��Ͻ� �ϴµ� �����ߴٸ� �������� �ʴ´�.
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
        Debug.LogError($"����Ƽ IAP �ʱ�ȭ ���� {error}");
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
                errorMessage = "���� �Ұ� ��ǰ";
                break;
            case PurchaseFailureReason.ExistingPurchasePending:
                errorMessage = "���� ���� ������";
                break;
            case PurchaseFailureReason.ProductUnavailable:
                errorMessage = "����� �� ���� ��ǰ";
                break;
            case PurchaseFailureReason.SignatureInvalid:
                errorMessage = "������ �߸���";
                break;
            case PurchaseFailureReason.UserCancelled:
                errorMessage = "���� ���";
                break;
            case PurchaseFailureReason.PaymentDeclined:
                errorMessage = "���� ����";
                break;
            case PurchaseFailureReason.DuplicateTransaction:
                errorMessage = "�ߺ� �ŷ�";
                break;
            default:
                errorMessage = "�� �� ���� ����";
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

        // product�� �����ϰ�, ��� ������ ���¶�� ���� �õ�
        if (product != null && product.availableToPurchase)
            _storeController.InitiatePurchase(product);
    }

    // Apple ����� ������ �� �� ���� �ϴ� �޼���. (���� ����)
    public void RestorePurchase()
    {
        if (!isInitialized) return;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // ���� ���� �õ� ����
            var appleExt = _extensionProvider.GetExtension<IAppleExtensions>();

            appleExt.RestoreTransactions(
                result => Debug.Log($"���� ���� �õ� ���: {result}"));
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
