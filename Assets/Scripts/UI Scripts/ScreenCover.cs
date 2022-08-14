using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCover : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private ImageFadeAnimation _imageFadeAnimation;

    private void Awake()
    {
        _imageFadeAnimation.onCompletionFadeOut += () => gameObject.SetActive(false);
    }

    public void FadeOut(Color color, float lerpSpeed)
    {
        _image.color = color;
        _imageFadeAnimation.lerpSpeed = lerpSpeed;
        _imageFadeAnimation.FadeOutImage();
    }

    public void FadeIn(Color color, float lerpSpeed)
    {
        _image.color = color;
        _imageFadeAnimation.lerpSpeed = lerpSpeed;
        _imageFadeAnimation.FadeInImage();
    }

    public void BlinkScreen(Color color, float lerpSpeed)
    {
        // �̹����� �����̴� ���¶�� �ǳʶڴ�.
        if (_imageFadeAnimation.isBlinking) return;

        _image.color = color;
        _imageFadeAnimation.lerpSpeed = lerpSpeed;
        _imageFadeAnimation.BlinkImage();
    }

    public void StopBlinkScreen()
    {
        // �̹����� �������� �ʴ� ���¶�� �ǳʶڴ�.
        if (!_imageFadeAnimation.isBlinking) return;

        _imageFadeAnimation.StopBlinkImage();
    }
}