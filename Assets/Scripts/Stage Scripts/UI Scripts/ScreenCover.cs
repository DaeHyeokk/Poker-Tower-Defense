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
        if (UIManager.instance.screenCover != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        _imageFadeAnimation.onCompletionFadeOut += () => gameObject.SetActive(false);
    }

    public void FillTheScreen()
    {
        StartCoroutine(FillTheScreenCoroutine());
    }

    private IEnumerator FillTheScreenCoroutine()
    {
        _image.color = new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
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
        // 이미지가 깜빡이는 상태라면 건너뛴다.
        if (_imageFadeAnimation.isBlinking) return;

        _image.color = color;
        _imageFadeAnimation.lerpSpeed = lerpSpeed;
        _imageFadeAnimation.BlinkImage();
    }

    public void StopBlinkScreen()
    {
        // 이미지가 깜빡이지 않는 상태라면 건너뛴다.
        if (!_imageFadeAnimation.isBlinking) return;

        _imageFadeAnimation.StopBlinkImage();
    }
}
