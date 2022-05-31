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
    private TextFadeEffect _textFadeEffect;
    [SerializeField]
    private float _normalDamageFontSize;
    [SerializeField]
    private Color _normalDamageColor;
    [SerializeField]
    private float _critDamageFontSize;
    [SerializeField]
    private Color _critDamageColor;

    private WaitForSeconds _fadeTime;

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
        _fadeTime = new(_textFadeEffect.fadeTime);
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnPoolCoroutine());
    }

    private IEnumerator ReturnPoolCoroutine()
    {
        yield return _fadeTime;
        UIManager.instance.damageTakenTextPool.ReturnObject(this);
    }
}
