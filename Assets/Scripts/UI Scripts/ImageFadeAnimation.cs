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

    private bool _isBlinking;
    private Color _tempColor;

    public event Action onCompletionFadeIn;
    public event Action onCompletionFadeOut;

    public float fadeTime { get; set; }
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
        _tempColor = _image.color;
        _tempColor.a = 1f;
        _image.color = _tempColor;

        // ���� ���� 0�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ ��������.
        while (_image.color.a > 0)
        {
            _tempColor = _image.color;
            DecreaseColorAlpha();

            yield return null;
        }

        if (onCompletionFadeOut != null)
            onCompletionFadeOut();
    }

    private IEnumerator FadeInImageCoroutine()
    {
        // ���� ���� 0���� ����.
        _tempColor = _image.color;
        _tempColor.a = 0f;
        _image.color = _tempColor;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ �ѷ�����.
        while (_image.color.a < 1)
        {
            _tempColor = _image.color;
            IncreaseColorAlpha();

            yield return null;
        }

        if (onCompletionFadeIn != null)
            onCompletionFadeIn();
    }

    private IEnumerator BlinkImageCoroutine()
    {
        // ���� ���� 0���� ����.
        _tempColor = _image.color;
        _tempColor.a = 0f;
        _image.color = _tempColor;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ �ѷ�����.
        while (_image.color.a < 0.2f)
        {
            _tempColor = _image.color;

            // �̹����� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
            if (!_isBlinking)
            {
                _tempColor.a = 0f;
                _image.color = _tempColor;
                yield break;
            }
            else
                IncreaseColorAlpha();

            yield return null;
        }

        // ���� ���� 0�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ ��������.
        while (_image.color.a > 0f)
        {
            _tempColor = _image.color;


            // �̹����� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
            if (!_isBlinking)
            {
                _tempColor.a = 0f;
                _image.color = _tempColor;
                yield break;
            }
            else
                DecreaseColorAlpha();

            yield return null;
        }

        // �ݺ��ؼ� �����̵��� �ϱ� ���� �ڷ�ƾ �޼ҵ� ���ȣ��.
        StartCoroutine(BlinkImageCoroutine());
    }

    private void IncreaseColorAlpha()
    {
        if (_deltaTimeMode == DeltaTimeMode.GameTime)
            _tempColor.a += Time.deltaTime / fadeTime;
        else
            _tempColor.a += Time.unscaledDeltaTime / fadeTime;

        _image.color = _tempColor;
    }

    private void DecreaseColorAlpha()
    {
        if (_deltaTimeMode == DeltaTimeMode.GameTime)
            _tempColor.a -= Time.deltaTime / fadeTime;
        else
            _tempColor.a -= Time.unscaledDeltaTime / fadeTime;

        _image.color = _tempColor;
    }
}
