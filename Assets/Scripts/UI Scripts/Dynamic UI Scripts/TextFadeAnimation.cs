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
        // 알파 값을 1로 설정.
        _color = _text.color;
        _color.a = 1f;
        _text.color = _color;

        // 알파 값이 0이 될 때 까지 알파 값 감소
        // Time.deltaTime 값을 빼주기 때문에 fadeTime마다 1만큼 감소함 => fadeTime초에 걸쳐 서서히 투명해짐.
        while (_text.color.a > 0)
        { 
            _color = _text.color;
            _color.a -= Time.deltaTime / _fadeTime;
            _text.color = _color;

            yield return null;
        }
    }
}
