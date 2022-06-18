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
        // ���� ���� 1�� ����.
        Color color = _image.color;
        color.a = 1f;
        _image.color = color;

        // ���� ���� 0�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ ��������.
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
        // ���� ���� 0���� ����.
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ �ѷ�����.
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
        // ���� ���� 0���� ����.
        Color color = _image.color;
        color.a = 0f;
        _image.color = color;

        // ���� ���� 1�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ �ѷ�����.
        while (_image.color.a < 0.2f)
        {
            color = _image.color;

            // �̹����� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
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

        // ���� ���� 0�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ ��������.
        while (_image.color.a > 0f)
        {
            color = _image.color;


            // �̹����� �����̴� ���°� �ƴ϶�� �̹����� ������ �����ϰ� �ٲ� ���� �ڷ�ƾ�� ��� �����Ѵ�.
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

        // �ݺ��ؼ� �����̵��� �ϱ� ���� �ڷ�ƾ �޼ҵ� ���ȣ��.
        StartCoroutine(BlinkImageCoroutine());
    }
}
