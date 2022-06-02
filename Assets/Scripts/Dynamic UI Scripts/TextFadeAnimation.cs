using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeAnimation : MonoBehaviour
{
    [SerializeField]
    private float _fadeTime;
    private TextMeshProUGUI _text;
    private Color _color;

    public float fadeTime => _fadeTime;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeTextCoroutine());
    }

    private IEnumerator FadeTextCoroutine()
    {
        // ���� ���� 1�� ����.
        _color = _text.color;
        _color.a = 1f;
        _text.color = _color;

        // ���� ���� 0�� �� �� ���� ���� �� ����
        // Time.deltaTime ���� ���ֱ� ������ fadeTime���� 1��ŭ ������ => fadeTime�ʿ� ���� ������ ��������.
        while (_text.color.a > 0)
        { 
            _color = _text.color;
            _color.a -= Time.deltaTime / _fadeTime;
            _text.color = _color;

            yield return null;
        }
    }
}
