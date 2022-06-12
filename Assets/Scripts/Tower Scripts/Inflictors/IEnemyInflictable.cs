using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface IEnemyInflictable
{
    public StringBuilder inflictorInfo { get; }
    public void UpdateInflictorInfo();
    public void Inflict(Enemy target);
}
