using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemMessage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _messageText;
    [SerializeField]
    private TextFadeEffect _textFadeEffect;

    private WaitForSeconds _fadeDelay;
    
    public TextMeshProUGUI messageText => _messageText;

    private void Awake()
    {
        _fadeDelay = new WaitForSeconds(_textFadeEffect.fadeTime);
    }
    
    private void OnEnable()
    {
        StartCoroutine(ReturnPoolCoroutine());
    }

    private IEnumerator ReturnPoolCoroutine()
    {
        yield return _fadeDelay;
        UIManager.instance.systemMessagePool.ReturnObject(this);
    }
}
