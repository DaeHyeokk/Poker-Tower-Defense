using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    private ObjectDetector _objectDetector;

    private void Awake()
    {
        _objectDetector = FindObjectOfType<ObjectDetector>();
    }

    private void OnEnable() => _objectDetector.popupUICount++;
    private void OnDisable() => _objectDetector.popupUICount--;
}
