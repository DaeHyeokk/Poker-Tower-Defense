using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class FieldBossEnemy : FieldEnemy
{
    public event Action onMissingAction;
    public abstract void OnMissing();
}
