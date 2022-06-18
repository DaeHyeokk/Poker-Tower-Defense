using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class FadeText : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _textMeshPro;
    [SerializeField]
    private TextFadeAnimation _textFadeAnimation;

    private WaitForSeconds _fadeDelay;

    public TextMeshPro textMeshPro => _textMeshPro;
    public TextFadeAnimation textFadeAnimation => _textFadeAnimation;
    public WaitForSeconds fadeDelay => _fadeDelay;

    private void Awake()
    {
        _fadeDelay = new(_textFadeAnimation.fadeTime);
    }

    protected IEnumerator FadeWaitCoroutine()
    {
        yield return fadeDelay;
        ReturnPool();
    }

    public abstract void StartAnimation();
    protected abstract void ReturnPool();
}


/*
 * File : FadeText.cs
 * First Update : 2022/06/17 FRI 00:10
 * 인게임에서 화면에 나타났다가 사라지는 텍스트들의 추상클래스.
 */