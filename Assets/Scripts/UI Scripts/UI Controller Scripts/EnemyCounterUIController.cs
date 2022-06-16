using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounterUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _enemyCountText;
    [SerializeField]
    private TextMeshProUGUI _maxEnemyCountText;
    [SerializeField]
    private TextMeshProUGUI _slashText;

    public void Awake()
    {
        _enemyCountText.text = "0";
    }

    public void SetEnemyCountText(int count)
    {
        if (count <= 60)
            _enemyCountText.color = _maxEnemyCountText.color = _slashText.color = Color.white;
        else
            _enemyCountText.color = _maxEnemyCountText.color = _slashText.color = Color.red;

        _enemyCountText.text = count.ToString();
    }
}