using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUIFadeAnimation : MonoBehaviour
{
    public enum DeltaTimeMode { GameTime, RealTime }
    [SerializeField]
    private DeltaTimeMode _deltaTimeMode;
    [SerializeField]
    private float _lerpSpeed;
    [SerializeField]
    private TextMeshProUGUI _text;

    private bool _isBlinking;

    public event Action onCompletionFadeIn;
    public event Action onCompletionFadeOut;

    public float lerpSpeed
    {
        get => _lerpSpeed;
        set => _lerpSpeed = value;
    }
    public bool isBlinking => _isBlinking;

    public void FadeOutText()
    {
        StartCoroutine(FadeOutTextCoroutine());
    }
    public void FadeInText()
    {
        StartCoroutine(FadeInTextCoroutine());
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

    private IEnumerator FadeOutTextCoroutine()
    {
        // ���� ���� 1�� ����.
        Color color = _text.color;
        color.a = 1f;
        _text.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // percent�� 1�� �� �� ���� ����
        while (percent < 1)
        {
            IncreaseCurrentTime(ref currentTime);
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(1f, 0f, percent);

            color = _text.color;
            color.a = lerp;
            _text.color = color;

            yield return null;
        }

        if (onCompletionFadeOut != null)
            onCompletionFadeOut();
    }

    private IEnumerator FadeInTextCoroutine()
    {
        // ���� ���� 0���� ����.
        Color color = _text.color;
        color.a = 0f;
        _text.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        while (percent < 1)
        {
            IncreaseCurrentTime(ref currentTime);
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(0f, 1f, percent);

            color = _text.color;
            color.a = lerp;
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

        float currentTime = 0f;
        float percent = 0f;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        while (percent < 1)
        {
            // �ؽ�Ʈ�� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
            if (!_isBlinking)
            {
                color.a = 0f;
                _text.color = color;
                yield break;
            }

            IncreaseCurrentTime(ref currentTime);
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(0f, 1f, percent);

            color = _text.color;
            color.a = lerp;
            _text.color = color;

            yield return null;
        }

        // ���� ���� 0�� �� �� ���� ���� �� ����
        while (percent > 0)
        {
            // �ؽ�Ʈ�� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
            if (!_isBlinking)
            {
                color.a = 0f;
                _text.color = color;
                yield break;
            }

            IncreaseCurrentTime(ref currentTime);
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(1f, 0f, percent);

            color = _text.color;
            color.a = lerp;
            _text.color = color;

            yield return null;
        }

        // �ݺ��ؼ� �����̵��� �ϱ� ���� �ڷ�ƾ �޼ҵ� ���ȣ��.
        StartCoroutine(BlinkImageCoroutine());
    }

    private void IncreaseCurrentTime(ref float currentTime)
    {
        if (_deltaTimeMode == DeltaTimeMode.GameTime)
            currentTime += Time.deltaTime;
        else
            currentTime += Time.unscaledDeltaTime;
    }
}
