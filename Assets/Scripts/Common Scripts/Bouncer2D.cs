using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer2D : MonoBehaviour
{
    private enum AnimationState { Bigger, Smaller }

    [SerializeField]
    private float _animSpeed;
    [SerializeField]
    private float _minScale;
    [SerializeField]
    private float _maxScale;

    private float _scale;
    private AnimationState _state;

    private void OnEnable()
    {
        _scale = 1f;
        _state = AnimationState.Bigger;
        this.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        float unscaledDeltaTime = Time.unscaledDeltaTime;
        if (unscaledDeltaTime > 0.3f)
            unscaledDeltaTime = 0.3f;

        if (_state == AnimationState.Bigger)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale += unscaledDeltaTime * _animSpeed;

            if (_scale >= _maxScale)
                _state = AnimationState.Smaller;
        }

        if (_state == AnimationState.Smaller)
        {
            this.transform.localScale = Vector3.one * _scale;
            _scale -= unscaledDeltaTime * _animSpeed;

            if (_scale <= _minScale)
                _state = AnimationState.Bigger;
        }
    }
}
