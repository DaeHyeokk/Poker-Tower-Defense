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
    private Tower _collocationTower;

    public Tower collocationTower
    {
        get => _collocationTower;
        set
        {
            _collocationTower = value;

            if (_collocationTower == null)
                _spriteRenderer.sprite = _emptySprite;
            else
                _spriteRenderer.sprite = _nonEmptySprite;
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        collocationTower = null;
    }
}
