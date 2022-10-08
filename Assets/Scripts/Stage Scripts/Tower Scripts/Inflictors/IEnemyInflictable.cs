using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface IEnemyInflictable : IInflictable
{
    public void Inflict(Enemy target);
}
