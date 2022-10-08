using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface ITowerInflictable : IInflictable
{
    public void Inflict(Tower target);
}
