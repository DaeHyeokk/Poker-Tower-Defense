using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeAnimation : MonoBehaviour
{
    public enum DeltaTimeMode { GameTime, RealTime }
    [SerializeField]
    private DeltaTimeMode _deltaTimeMode;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private float _lerpSpeed;

    private bool _isBlinking;

    public event Action onCompletionFadeIn;
    public event Action onCompletionFadeOut;

    public float lerpSpeed
    {
        set => _lerpSpeed = value;
    }

    public bool isBlinking => _isBlinking;

    public void FadeOutImage()
    {
        StartCoroutine(FadeOutImageCoroutine());
    }
    public void FadeInImage()
    {
        StartCoroutine(FadeInImageCoroutine());
    }
    public void BlinkImage()
    {
        _isBlinking = true;

        StartCoroutine(BlinkImageCoroutine());
    }

    public void StopBlinkImage()
    {
        _isBlinking = false;
    }

    private IEnumerator FadeOutImageCoroutine()
    {
        // ���� ���� 1�� ����.
        Color color = _image.color;
        color.a = 1f;
        _image.color = color;

        float currentTime = 0f;
        float percent = 0f;
        // ���� ���� 0�� �� �� ���� ���� �� ����
        while (percent < 1)
        {
            if (_deltaTimeMode == DeltaTimeMode.GameTime)
                currentTime += Time.deltaTime;
            else
                currentTime += Time.unscaledDeltaTime;

            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(1f, 0f, percent);

            color = _image.color;
            color.a = lerp;
            _image.color = color;

            yield return null;
        }

        if (onCompletionFadeOut != null)
            onCompletionFadeOut();
    }

    private IEnumerator FadeInImageCoroutine()
    {
        // ���� ���� 0���� ����.
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        while (percent < 1)
        {
            if (_deltaTimeMode == DeltaTimeMode.GameTime)
                currentTime += Time.deltaTime;
            else
                currentTime += Time.unscaledDeltaTime;

            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(0f, 1f, percent);

            color = _image.color;
            color.a = lerp;
            _image.color = color;

            yield return null;
        }

        if (onCompletionFadeIn != null)
            onCompletionFadeIn();
    }

    private IEnumerator BlinkImageCoroutine()
    {
        // ���� ���� 0���� ����.
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        while (percent < 1)
        {
            // �̹����� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
            if (!_isBlinking)
            {
                color.a = 0f;
                _image.color = color;
                yield break;
            }

            if (_deltaTimeMode == DeltaTimeMode.GameTime)
                currentTime += Time.deltaTime;
            else
                currentTime += Time.unscaledDeltaTime;

            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(0f, 1f, percent);

            color = _image.color;
            color.a = lerp;
            _image.color = color;

            yield return null;
        }

        // ���� ���� 0�� �� �� ���� ���� �� ����
        while (percent > 0)
        {
            // �̹����� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
            if (!_isBlinking)
            {
                color.a = 0f;
                _image.color = color;
                yield break;
            }

            if (_deltaTimeMode == DeltaTimeMode.GameTime)
                currentTime += Time.deltaTime;
            else
                currentTime += Time.unscaledDeltaTime;

            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(1f, 0f, percent);

            color = _image.color;
            color.a = lerp;
            _image.color = color;

            yield return null;
        }

        // �ݺ��ؼ� �����̵��� �ϱ� ���� �ڷ�ƾ �޼ҵ� ���ȣ��.
        StartCoroutine(BlinkImageCoroutine());
    }
}
