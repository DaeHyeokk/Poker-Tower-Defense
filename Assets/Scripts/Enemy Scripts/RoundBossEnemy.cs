using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBossEnemy : FieldEnemy
{
    public override void OnMissing()
    {
        // ���� ������ ������ ��� ���ӿ��� �й��Ѵ�.
        GameManager.instance.DefeatGame();

        ReturnObject();
    }

    protected override void ReturnObject()
    {
        this.gameObject.SetActive(false);
    }
}
