using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIAnimation : MonoBehaviour
{
    [SerializeField]
    private float _animSpeed;
    private float _scale;

    private readonly WaitForFixedUpdate _waitForFixedUpdate = new();

    public event Action onCompletionBigger;
    public event Action onCompletionSmaller;
  
    public void StartBiggerAnimation()
    {
        _scale = 0f;
        StartCoroutine(BiggerAnimationCoroutine());
    }

    private IEnumerator BiggerAnimationCoroutine()
    {
        while(_scale < 1.2f)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale += Time.fixedUnscaledDeltaTime * _animSpeed;

            yield return _waitForFixedUpdate;
        }

        while (_scale >= 1f)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale -= Time.fixedUnscaledDeltaTime * _animSpeed;

            yield return _waitForFixedUpdate;
        }

        this.transform.localScale = Vector3.one;

        if (onCompletionBigger != null)
            onCompletionBigger();
    }

    public void StartSmallerAnimation()
    {
        _scale = 1f;
        StartCoroutine(SmallerAnimationCoroutine());
    }
    private IEnumerator SmallerAnimationCoroutine()
    {
        while (_scale <= 1.2f)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale += Time.fixedUnscaledDeltaTime * _animSpeed;

            yield return _waitForFixedUpdate;
        }

        while (_scale >= 0f)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale -= Time.fixedUnscaledDeltaTime * _animSpeed;

            yield return _waitForFixedUpdate;
        }

        this.transform.localScale = Vector3.zero;

        if (onCompletionSmaller != null)
            onCompletionSmaller();
    }
}
