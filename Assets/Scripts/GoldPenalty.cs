using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldPenalty : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _remainWaveText;
    private int _remainWave;

    public int remainWave
    {
        get => _remainWave;
        set
        {
            if (value > 0)
            {
                if (!this.gameObject.activeSelf)
                    this.gameObject.SetActive(true);
            }
            else
            {
                if (this.gameObject.activeSelf)
                    this.gameObject.SetActive(false);
            }

            _remainWave = value;
            _remainWaveText.text = value.ToString();
        }
    }
}
