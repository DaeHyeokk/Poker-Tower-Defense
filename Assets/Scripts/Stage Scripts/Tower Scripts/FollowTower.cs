using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowTower : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _towerRenderer;
    [SerializeField]
    private SpriteRenderer _attackRangeRenderer;
    private Image[] _levelImages;

    private void Awake()
    {
        _levelImages = GetComponentsInChildren<Image>(true);
    }

    public void Setup(Tower fromTower)
    {
        this.transform.position = fromTower.transform.position;

        SetTowerRenderer(fromTower);
        SetAttackRangeRendererScale(fromTower.range);
        SetTowerLevelImage(fromTower.level);
    }

    private void SetAttackRangeRendererScale(float range)
    {
        float attackRangeScale = range * 2 / this.transform.lossyScale.x;
        _attackRangeRenderer.transform.localScale = new Vector3(attackRangeScale, attackRangeScale, 0);
    }

    private void SetTowerRenderer(Tower tower)
    {
        _towerRenderer.sprite = tower.towerRenderer.sprite;
        _towerRenderer.color = tower.towerRenderer.color;
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
