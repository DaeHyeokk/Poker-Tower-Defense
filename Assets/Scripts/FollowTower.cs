using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowTower : MonoBehaviour
{
    private SpriteRenderer _towerRenderer;
    private SpriteRenderer _attackRangeRenderer;
    private Image[] _levelImages;

    private ObjectFollowMousePosition _movement;

    private void Awake()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _towerRenderer = spriteRenderers[0];
        _attackRangeRenderer = spriteRenderers[1];

        _levelImages = GetComponentsInChildren<Image>(true);
        _movement = GetComponent<ObjectFollowMousePosition>();
    }

    public void Setup(Tower fromTower)
    {
        SetAttackRangeRendererScale(fromTower.range);
        SetTowerRendererColor(fromTower.color);
        SetTowerLevelImage(fromTower.level);
    }
    public void StartFollowMousePosition()
    {
        _movement.StartFollowMousePosition();
    }

    public void StopFollowMousePosition()
    {
        _movement.StopFollowMousePosition();
    }

    private void SetAttackRangeRendererScale(float range)
    {
        float attackRangeScale = range * 2 / this.transform.lossyScale.x;
        _attackRangeRenderer.transform.localScale = new Vector3(attackRangeScale, attackRangeScale, 0);
    }

    private void SetTowerRendererColor(Color color)
    {
        _towerRenderer.color = color;
    }

    private void SetTowerLevelImage(int level)
    {
        for (int i = 0; i < level; i++)
            _levelImages[i].gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _levelImages.Length; i++)
            _levelImages[i].gameObject.SetActive(false);
    }
}
