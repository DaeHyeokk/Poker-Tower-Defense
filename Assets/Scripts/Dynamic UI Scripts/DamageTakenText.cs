using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamageTakenType { Normal, Critical }

public class DamageTakenText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _damageTakenText;
    [SerializeField]
    private TextFadeAnimation _textFadeAnimation;
    [SerializeField]
    private float _normalDamageFontSize;
    [SerializeField]
    private Color _normalDamageColor;
    [SerializeField]
    private float _critDamageFontSize;
    [SerializeField]
    private Color _critDamageColor;

    private WaitForSeconds _fadeDelay;

    public TextMeshProUGUI damageTakenText => _damageTakenText;
    public DamageTakenType damageTakenType
    {
        set
        {
            if (value == DamageTakenType.Normal)
            {
                _damageTakenText.fontSize = _normalDamageFontSize;
                _damageTakenText.color = _normalDamageColor;
            }
            else
            {
                _damageTakenText.fontSize = _critDamageFontSize;
                _damageTakenText.color = _critDamageColor;
            }
        }
    }

    private void Awake()
    {
        _fadeDelay = new(_textFadeAnimation.fadeTime);
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnPoolCoroutine());
    }

    private IEnumerator ReturnPoolCoroutine()
    {
        yield return _fadeDelay;
        UIManager.instance.damageTakenTextPool.ReturnObject(this);
    }
}
