using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDetailInfoUIAnimation : MonoBehaviour
{
    [SerializeField]
    private float _animSpeed;
    private float _scale;
    private bool _isBigger = false;
    private bool _isSmaller = false;

    public event Action onCompletionBigger;
    public event Action onCompletionSmaller;

    public void StartBiggerAnimation()
    {
        this.transform.localScale = Vector3.zero;
        _scale = 0f;
        StartCoroutine(BiggerAnimationCoroutine());
    }

    private IEnumerator BiggerAnimationCoroutine()
    {
        _isBigger = true;

        while (_scale < 1.2f)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale += Time.unscaledDeltaTime * _animSpeed;

            yield return null;
        }

        _scale = 1f;

        while (_scale >= 1f)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale -= Time.unscaledDeltaTime * _animSpeed;

            yield return null;
        }

        this.transform.localScale = Vector3.one;

        if (onCompletionBigger != null)
            onCompletionBigger();

        _isBigger = false;
    }

    public void StartSmallerAnimation()
    {
        // 만약 팝업UI가 점점 커지거나 점점 작아지는 연산을 실행중이라면 건너뛴다.
        if (_isBigger || _isSmaller)
            return;

        _scale = 1f;
        StartCoroutine(SmallerAnimationCoroutine());
    }
    private IEnumerator SmallerAnimationCoroutine()
    {
        _isSmaller = true;

        while (_scale <= 1.2f)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale += Time.unscaledDeltaTime * _animSpeed;

            yield return null;
        }

        _scale = 1.2f;

        while (_scale >= 0f)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale -= Time.unscaledDeltaTime * _animSpeed;

            yield return null;
        }

        this.transform.localScale = Vector3.zero;

        if (onCompletionSmaller != null)
            onCompletionSmaller();

        _isSmaller = false;
    }
}
