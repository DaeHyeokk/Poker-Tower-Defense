using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicText : MonoBehaviour
{
    [SerializeField]
    private Movement2D _movement2D;
    [SerializeField]
    private TextFadeAnimation _textFadeAnimation;
    [SerializeField]
    private TextMeshPro _textMeshPro;

    public float moveSpeed => _movement2D.moveSpeed;
    public float fadeTime => _textFadeAnimation.fadeTime;
    public TextMeshPro textMeshPro => _textMeshPro;

    public void ShowText()
    {

    }

    private IEnumerator ReturnObjectCoroutine()
    {
        yield return null;
    }
}
