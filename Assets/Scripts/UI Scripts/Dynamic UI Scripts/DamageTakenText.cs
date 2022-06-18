using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamageTakenType { Normal, Critical }

public class DamageTakenText : FadeText
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
    private WaitForSeconds _animationStartDelay = new(0.2f);

    public DamageTakenType damageTakenType
    {
        set
        {
            if (value == DamageTakenType.Normal)
            {
                base.textMeshPro.fontSize = _normalDamageFontSize;
                base.textMeshPro.color = _normalDamageColor;
                _highlightFontSize = _normalhighlightFontSize;
            }
            else
            {
                base.textMeshPro.fontSize = _critDamageFontSize;
                base.textMeshPro.color = _critDamageColor;
                _highlightFontSize = _crithighlightFontSize;
            }
        }
    }

    public override void StartAnimation()
    {
        StartCoroutine(HighlightEffectCoroutine());
    }

    private IEnumerator HighlightEffectCoroutine()
    {
        _movement2D.Stop();

        float backupFontSize = base.textMeshPro.fontSize;

        while(base.textMeshPro.fontSize < _highlightFontSize)
        {
            base.textMeshPro.fontSize += _highlightSpeed * Time.deltaTime;

            if (base.textMeshPro.fontSize > _highlightFontSize)
                base.textMeshPro.fontSize = _highlightFontSize;

            yield return null;
        }

        while(base.textMeshPro.fontSize > backupFontSize)
        {
            base.textMeshPro.fontSize -= _highlightSpeed * Time.deltaTime;

            if (base.textMeshPro.fontSize < backupFontSize)
                base.textMeshPro.fontSize = backupFontSize;

            yield return null;
        }

        yield return _animationStartDelay;

        _movement2D.Move();
        base.textFadeAnimation.FadeOutText();
    }

    protected override void ReturnPool() => UIManager.instance.damageTakenTextPool.ReturnObject(this);
}

/*
 * File : DamageTakenText.cs
 * First Update : 2022/06/17 FRI 00:10
 * �ΰ��ӿ��� ���� �޴� �������� ȭ�鿡 ��Ÿ���� ���� ��ũ��Ʈ.
 */