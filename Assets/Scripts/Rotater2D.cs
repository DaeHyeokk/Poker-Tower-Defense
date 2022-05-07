using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater2D : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _targetSprite;


    public void NaturalRotate()
    {
        _targetSprite.transform.Rotate(Vector3.forward * Time.deltaTime * 50f);
    }

    public void Rotate(Vector3 direction)
    {
        if (direction == Vector3.right)
            _targetSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Vector3.up)
            _targetSprite.transform.rotation = Quaternion.Euler(0, 0, 90f);
        else if (direction == Vector3.left)
            _targetSprite.transform.rotation = Quaternion.Euler(0, 0, 180f);
        else
            _targetSprite.transform.rotation = Quaternion.Euler(0, 0, 270f);
    }
}
