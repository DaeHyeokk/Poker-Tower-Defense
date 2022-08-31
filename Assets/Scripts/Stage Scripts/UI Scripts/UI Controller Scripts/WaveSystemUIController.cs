using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSystemUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _waveText;
    [SerializeField]
    private TextMeshProUGUI _minuteText;
    [SerializeField]
    private TextMeshProUGUI _secondText;

    public void SetWaveText(int wave) => _waveText.text = wave.ToString();
    public void SetMinuteText(int minute) => _minuteText.text = minute.ToString();
    public void SetSecondText(int second) => _secondText.text = second.ToString("D2");

}
