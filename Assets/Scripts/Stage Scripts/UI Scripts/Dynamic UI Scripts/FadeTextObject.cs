using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class FadeTextObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _textMeshPro;
    [SerializeField]
    private TextObjectFadeAnimation _textObjectFadeAnimation;

    public TextMeshPro textMeshPro => _textMeshPro;
    public TextObjectFadeAnimation textObjectFadeAnimation => _textObjectFadeAnimation;

    private void Awake()
    {
        _textObjectFadeAnimation.onCompletionFadeOut += ReturnPool;
    }

    public abstract void StartAnimation();

    protected abstract void ReturnPool();
}


/*
 * File : FadeText.cs
 * First Update : 2022/06/17 FRI 00:10
 * �ΰ��ӿ��� ȭ�鿡 ��Ÿ���ٰ� ������� �ؽ�Ʈ���� �߻�Ŭ����.
 */