using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _text;

    public event Action onCompletionFadeIn;
    public event Action onCompletionFadeOut;

    private bool _isBlinking;

    public float fadeTime { get; set; }
    public bool isBlinking => _isBlinking;


    public void FadeOutText()
    {
        StartCoroutine(FadeOutImageCoroutine());
    }
    public void FadeInText()
    {
        StartCoroutine(FadeInImageCoroutine());
    }
    public void BlinkText()
    {
        _isBlinking = true;

        StartCoroutine(BlinkImageCoroutine());
    }

    public void StopBlinkText()
    {
        _isBlinking = false;
    }

    private IEnumerator FadeOutImageCoroutine()
    {
        // ���� ���� 1�� ����.
        Color color = _text.color;
        color.a = 1f;
        _text.color = color;

        // ���� ���� 0�� �� �� ���� ���� �� ����
        while (_text.color.a > 0)
        {
            color = _text.color;
            color.a -= Time.unscaledDeltaTime / fadeTime;
            _text.color = color;

            yield return null;
        }

        if (onCompletionFadeOut != null)
            onCompletionFadeOut();
    }

    private IEnumerator FadeInImageCoroutine()
    {
        // ���� ���� 0���� ����.
        Color color = _text.color;
        color.a = 0f;
        _text.color = color;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        while (_text.color.a < 1)
        {
            color = _text.color;
            color.a += Time.unscaledDeltaTime / fadeTime;
            _text.color = color;

            yield return null;
        }

        if (onCompletionFadeIn != null)
            onCompletionFadeIn();
    }

    private IEnumerator BlinkImageCoroutine()
    {
        // ���� ���� 0���� ����.
        Color color = _text.color;
        color.a = 0f;
        _text.color = color;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        while (_text.color.a < 0.2f)
        {
            color = _text.color;

            // �ؽ�Ʈ�� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
            if (!_isBlinking)
            {
                color.a = 0f;
                _text.color = color;
                yield break;
            }
            else
            {
                color.a += Time.unscaledDeltaTime / fadeTime;
                _text.color = color;
            }

            yield return null;
        }

        // ���� ���� 0�� �� �� ���� ���� �� ����
        while (_text.color.a > 0f)
        {
            color = _text.color;


            // �ؽ�Ʈ�� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
            if (!_isBlinking)
            {
                color.a = 0f;
                _text.color = color;
                yield break;
            }
            else
            {
                color.a -= Time.unscaledDeltaTime / fadeTime;
                _text.color = color;
            }

            yield return null;
        }

        // �ݺ��ؼ� �����̵��� �ϱ� ���� �ڷ�ƾ �޼ҵ� ���ȣ��.
        StartCoroutine(BlinkImageCoroutine());
    }
}
