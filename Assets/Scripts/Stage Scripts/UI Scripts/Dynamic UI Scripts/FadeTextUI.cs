using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class FadeTextUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField]
    private TextUIFadeAnimation _textUIFadeAnimation;

    public TextMeshProUGUI textMeshProUGUI => _textMeshProUGUI;
    public TextUIFadeAnimation textUIFadeAnimation => _textUIFadeAnimation;

    private void Awake()
    {
        _textUIFadeAnimation.onCompletionFadeOut += () => gameObject.SetActive(false);
    }

    public abstract void StartAnimation();

}


/*
 * File : FadeText.cs
 * First Update : 2022/06/17 FRI 00:10
 * 인게임에서 화면에 나타났다가 사라지는 텍스트들의 추상클래스.
 */