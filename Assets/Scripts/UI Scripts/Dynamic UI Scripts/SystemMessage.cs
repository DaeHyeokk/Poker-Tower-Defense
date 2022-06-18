using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemMessage : FadeText
{
    [SerializeField]
    private Movement2D _movement2D;

    private WaitForSeconds _animationStartDelay = new(0.03f);

    public override void StartAnimation()
    {
        StartCoroutine(AnimationStartDelayCoroutine());
    }

    private IEnumerator AnimationStartDelayCoroutine()
    {
        _movement2D.Stop();
        base.textFadeAnimation.FadeStop();
        
        yield return _animationStartDelay;

        _movement2D.Move();
        base.textFadeAnimation.FadeStart();

        StartCoroutine(base.FadeWaitCoroutine());
    }

    protected override void ReturnPool()
    {
        UIManager.instance.systemMessagePool.ReturnObject(this);
    }
}

/*
 * File : SystemMessage.cs
 * First Update : 2022/06/17 FRI 00:10
 * �ΰ��ӿ��� �÷��̾�� �ȳ� ������ �����ֱ� ���� ��ũ��Ʈ.
 */