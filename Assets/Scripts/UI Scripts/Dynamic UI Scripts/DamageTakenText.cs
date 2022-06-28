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

    private readonly WaitForSeconds _waitAnimationStartDelay = new(0.2f);

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
            textMeshPro.fontSize += _highlightSpeed * Time.deltaTime;

            if (textMeshPro.fontSize > _highlightFontSize)
                textMeshPro.fontSize = _highlightFontSize;

            yield return null;
        }

        while(textMeshPro.fontSize > backupFontSize)
        {
            textMeshPro.fontSize -= _highlightSpeed * Time.deltaTime;

            if (textMeshPro.fontSize < backupFontSize)
                textMeshPro.fontSize = backupFontSize;

            yield return null;
        }

        yield return _waitAnimationStartDelay;

        _movement2D.Move();
        base.textObjectFadeAnimation.FadeOutText();
    }

    protected override void ReturnPool() => UIManager.instance.damageTakenTextPool.ReturnObject(this);
}

/*
 * File : DamageTakenText.cs
 * First Update : 2022/06/17 FRI 00:10
 * �ΰ��ӿ��� ���� �޴� �������� ȭ�鿡 ��Ÿ���� ���� ��ũ��Ʈ.
 */