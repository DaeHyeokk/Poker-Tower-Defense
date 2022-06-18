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
        // 알파 값을 1로 설정.
        _tempColor = _image.color;
        _tempColor.a = 1f;
        _image.color = _tempColor;

        // 알파 값이 0이 될 때 까지 알파 값 감소
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 감소함 => fadeTime초에 걸쳐 서서히 투명해짐.
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
        // 알파 값을 0으로 설정.
        _tempColor = _image.color;
        _tempColor.a = 0f;
        _image.color = _tempColor;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 증가함 => fadeTime초에 걸쳐 서서히 뚜렷해짐.
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
        // 알파 값을 0으로 설정.
        _tempColor = _image.color;
        _tempColor.a = 0f;
        _image.color = _tempColor;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 증가함 => fadeTime초에 걸쳐 서서히 뚜렷해짐.
        while (_image.color.a < 0.2f)
        {
            _tempColor = _image.color;

            // 이미지가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
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

        // 알파 값이 0이 될 때 까지 알파 값 감소
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 감소함 => fadeTime초에 걸쳐 서서히 투명해짐.
        while (_image.color.a > 0f)
        {
            _tempColor = _image.color;


            // 이미지가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
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

        // 반복해서 깜빡이도록 하기 위한 코루틴 메소드 재귀호출.
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
