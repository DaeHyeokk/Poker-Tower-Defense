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
        // 알파 값을 1로 설정.
        Color color = _image.color;
        color.a = 1f;
        _image.color = color;

        float currentTime = 0f;
        float percent = 0f;
        // 알파 값이 0이 될 때 까지 알파 값 감소
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
        // 알파 값을 0으로 설정.
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // 알파 값이 1이 될 때 까지 알파 값 증가
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
        // 알파 값을 0으로 설정.
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        float currentTime = 0f;
        float percent = 0f;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        while (percent < 1)
        {
            // 이미지가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
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

        // 알파 값이 0이 될 때 까지 알파 값 감소
        while (percent > 0)
        {
            // 이미지가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
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

        // 반복해서 깜빡이도록 하기 위한 코루틴 메소드 재귀호출.
        StartCoroutine(BlinkImageCoroutine());
    }
}
