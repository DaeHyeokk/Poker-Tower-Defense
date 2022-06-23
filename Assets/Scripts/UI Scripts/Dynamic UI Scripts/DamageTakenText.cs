using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamageTakenType { Normal, Critical }

public class DamageTakenText : FadeTextObject
{
    [SerializeField]
    private Movement2D _movement2D;
    [SerializeField]
    private float _normalDamageFontSize;
    [SerializeField]
    private float _normalhighlightFontSize;
    [SerializeField]
    private Color _normalDamageColor;
    [SerializeField]
    private float _critDamageFontSize;
    [SerializeField]
    private float _crithighlightFontSize;
    [SerializeField]
    private Color _critDamageColor;
    [SerializeField]
    private float _highlightSpeed;

    private float _highlightFontSize;

    private readonly float _animationStartDelay = 0.2f;
    private readonly WaitForFixedUpdate _waitForFixedUpdate = new();

    public DamageTakenType damageTakenType
    {
        set
        {
            if (value == DamageTakenType.Normal)
            {
                textMeshPro.fontSize = _normalDamageFontSize;
                textMeshPro.color = _normalDamageColor;
                _highlightFontSize = _normalhighlightFontSize;
            }
            else
            {
                textMeshPro.fontSize = _critDamageFontSize;
                textMeshPro.color = _critDamageColor;
                _highlightFontSize = _crithighlightFontSize;
            }
        }
    }

    public override void StartAnimation()
    {
        StartCoroutine(StartAnimationCoroutine());
    }

    private IEnumerator StartAnimationCoroutine()
    {
        _movement2D.Stop();

        float backupFontSize = textMeshPro.fontSize;

        while(textMeshPro.fontSize < _highlightFontSize)
        {
            textMeshPro.fontSize += _highlightSpeed * Time.fixedDeltaTime;

            if (textMeshPro.fontSize > _highlightFontSize)
                textMeshPro.fontSize = _highlightFontSize;

            yield return _waitForFixedUpdate;
        }

        while(textMeshPro.fontSize > backupFontSize)
        {
            textMeshPro.fontSize -= _highlightSpeed * Time.fixedDeltaTime;

            if (textMeshPro.fontSize < backupFontSize)
                textMeshPro.fontSize = backupFontSize;

            yield return _waitForFixedUpdate;
        }

        float animationStartDelay = _animationStartDelay;
        while (animationStartDelay > 0)
        {
            yield return _waitForFixedUpdate;
            animationStartDelay -= Time.fixedDeltaTime;
        }

        _movement2D.Move();
        base.textObjectFadeAnimation.FadeOutText();
    }

    protected override void ReturnPool() => UIManager.instance.damageTakenTextPool.ReturnObject(this);
}

/*
 * File : DamageTakenText.cs
 * First Update : 2022/06/17 FRI 00:10
 * 인게임에서 적이 받는 데미지를 화면에 나타내기 위한 스크립트.
 */