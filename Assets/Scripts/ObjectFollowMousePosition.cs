using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowMousePosition : MonoBehaviour
{
    private Camera _mainCamera;
    private IEnumerator _coroutine;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _coroutine = FollowMousePosition();
    }

    public void StartFollowMousePosition()
    {
        StartCoroutine(_coroutine);
    }
    public void StopFollowMousePosition()
    {
        StopCoroutine(_coroutine);
    }

    private IEnumerator FollowMousePosition()
    {
        Vector3 position = new Vector3();

        while (true)
        {
            position.Set(Input.mousePosition.x, Input.mousePosition.y, 0);
            transform.position = _mainCamera.ScreenToWorldPoint(position);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
            yield return null;
        }
    }
}
