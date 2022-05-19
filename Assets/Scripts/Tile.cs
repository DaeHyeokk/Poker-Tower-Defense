using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Sprite _emptySprite;
    [SerializeField]
    private Sprite _nonEmptySprite;

    private SpriteRenderer _spriteRenderer;
    private bool _isEmpty;

    public bool isEmpty => _isEmpty;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isEmpty = true;
    }

    public void ToggleIsEmpty()
    {
        if(_isEmpty)
        {
            _isEmpty = !_isEmpty;
            _spriteRenderer.sprite = _nonEmptySprite;
        }
        else
        {
            _isEmpty = !_isEmpty;
            _spriteRenderer.sprite = _emptySprite;
        }
    }
}
