using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeAnimation : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    private bool _isBlinking;
    private IEnumerator _blinkImageCoroutine;

    public float fadeTime { get; set; }
    public bool isBlinking => _isBlinking;
    public Image image => _image;

    private void Awake()
    {
        _blinkImageCoroutine = BlinkImageCoroutine();
    }

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

        // 알파 값이 0이 될 때 까지 알파 값 감소
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 감소함 => fadeTime초에 걸쳐 서서히 투명해짐.
        while (_image.color.a > 0)
        {
            color = _image.color;
            color.a -= Time.deltaTime / fadeTime;
            _image.color = color;

            yield return null;
        }
    }

    private IEnumerator FadeInImageCoroutine()
    {
        // 알파 값을 0으로 설정.
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 증가함 => fadeTime초에 걸쳐 서서히 뚜렷해짐.
        while (_image.color.a < 1)
        {
            color = _image.color;
            color.a += Time.deltaTime / fadeTime;
            _image.color = color;

            yield return null;
        }
    }

    private IEnumerator BlinkImageCoroutine()
    {
        // 알파 값을 0으로 설정.
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        // 알파 값이 1이 될 때 까지 알파 값 증가
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 증가함 => fadeTime초에 걸쳐 서서히 뚜렷해짐.
        while (_image.color.a < 0.2f)
        {
            color = _image.color;

            // 이미지가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
            if (!_isBlinking)
            {
                color.a = 0f;
                _image.color = color;
                yield break;
            }
            else
            {
                color.a += Time.deltaTime / fadeTime;
                _image.color = color;
            }

            yield return null;
        }

        // 알파 값이 0이 될 때 까지 알파 값 감소
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 감소함 => fadeTime초에 걸쳐 서서히 투명해짐.
        while (_image.color.a > 0f)
        {
            color = _image.color;


            // 이미지가 깜빡이는 상태가 아니라면 이미지를 완전히 투명하게 바꾼 다음 코루틴을 즉시 종료한다.
            if (!_isBlinking)
            {
                color.a = 0f;
                _image.color = color;
                yield break;
            }
            else
            {
                color.a -= Time.deltaTime / fadeTime;
                _image.color = color;
            }

            yield return null;
        }

        // 반복해서 깜빡이도록 하기 위한 코루틴 메소드 재귀호출.
        StartCoroutine(BlinkImageCoroutine());
    }
}
