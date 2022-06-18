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
        // 이미지가 깜빡이는 상태라면 건너뛴다.
        if (_imageFadeAnimation.isBlinking) return;

        _image.color = color;
        _imageFadeAnimation.fadeTime = fadeTime;
        _imageFadeAnimation.BlinkImage();
    }

    public void StopBlinkScreen()
    {
        // 이미지가 깜빡이지 않는 상태라면 건너뛴다.
        if (!_imageFadeAnimation.isBlinking) return;

        _imageFadeAnimation.StopBlinkImage();
    }
}
