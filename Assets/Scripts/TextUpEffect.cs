using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUpEffect : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    private void OnEnable()
    {
        StartCoroutine(MoveUpTextCoroutine());
    }

    private IEnumerator MoveUpTextCoroutine()
    {
        transform.position = Vector3.zero;

        while (true)
        {
            transform.Translate(new Vector3(0f, _moveSpeed * Time.deltaTime, 0f));

            yield return null;
        }
    }
}
