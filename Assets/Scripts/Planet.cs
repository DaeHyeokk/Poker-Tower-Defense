using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [SerializeField]
    private Rotater2D _rotater2D;

    // Update is called once per frame
    void Update()
    {
        _rotater2D.NaturalRotate();
    }
}
