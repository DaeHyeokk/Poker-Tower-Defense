using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeAnimation : MonoBehaviour
{
    [SerializeField]
    private float _fadeTime;
    [SerializeField]
    private TextMeshPro _text;
    private bool _isStop;

    public float fadeTime => _fadeTime;

    private void OnEnable()
    {
        StartCoroutine(FadeTextCoroutine());
    }

    private IEnumerator FadeTextCoroutine()
    {
        // ���� ���� 1�� ����.
        Color color = _text.color;
        color.a = 1f;
        _text.color = color;

        // ���� ���� 0�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ ��������.
        while (_text.color.a > 0)
        {
            if (!_isStop)
            {
                color = _text.color;
                color.a -= Time.deltaTime / _fadeTime;
                _text.color = color;
            }

            yield return null;
        }
    }

    public void FadeStart() => _isStop = false;

    public void FadeStop() => _isStop = true;
}
