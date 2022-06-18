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

    public void FadeInScreen(Color color, float fadeTime)
    {
        _image.color = color;
        _imageFadeAnimation.fadeTime = fadeTime;
        _imageFadeAnimation.FadeOutImage();
    }

    public void FadeOutScreen(Color color, float fadeTime)
    {
        _image.color = color;
        _imageFadeAnimation.fadeTime = fadeTime;
        _imageFadeAnimation.FadeInImage();
    }

    public void BlinkScreen(Color color, float fadeTime)
    {
        // �̹����� �����̴� ���¶�� �ǳʶڴ�.
        if (_imageFadeAnimation.isBlinking) return;

        _image.color = color;
        _imageFadeAnimation.fadeTime = fadeTime;
        _imageFadeAnimation.BlinkImage();
    }

    public void StopBlinkScreen()
    {
        // �̹����� �������� �ʴ� ���¶�� �ǳʶڴ�.
        if (!_imageFadeAnimation.isBlinking) return;

        _imageFadeAnimation.StopBlinkImage();
    }
}
