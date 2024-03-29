using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveStartMessage : MonoBehaviour
{
    [SerializeField]
    private WaveSystem _waveSystem;
    [SerializeField]
    private Image _backgroundImage;
    [SerializeField]
    private TextMeshProUGUI _waveStartText;
    [SerializeField]
    private TextMeshProUGUI _waveWarningText;
    [SerializeField]
    private float _lerpSpeed;

    private readonly float _fadeOutDelay = 0.65f;

    private void OnEnable()
    {
        if (_waveSystem.isBossWave)
            _waveWarningText.gameObject.SetActive(true);
        else
            _waveWarningText.gameObject.SetActive(false);

        _waveStartText.text = "웨이브 " + _waveSystem.wave.ToString();

        StartCoroutine(AlphaLerpCoroutine(0f, 1f));
    }

    private IEnumerator AlphaLerpCoroutine(float start, float end)
    {
        float currentTime = 0f;
        float percent = 0f;

        while (percent < 1f)
        {
            currentTime += Time.unscaledDeltaTime;
            percent = currentTime * _lerpSpeed;
            float lerp = Mathf.Lerp(start, end, percent);

            _backgroundImage.transform.localScale = new Vector3(1f, lerp, 1f);
            Color color;

            color = _waveStartText.color;
            color.a = lerp;
            _waveStartText.color = color;

            color = _waveWarningText.color;
            color.a = lerp;
            _waveWarningText.color = color;

            yield return null;
        }

        if (start < end)
        {
            float fadeOutDelay = _fadeOutDelay;
            while (fadeOutDelay > 0)
            {
                fadeOutDelay -= Time.unscaledDeltaTime;

                yield return null;
            }
            // 크기와 알파값이 감소하도록 재귀호출.
            StartCoroutine(AlphaLerpCoroutine(end, start));
        }
        else
            gameObject.SetActive(false);
    }
}
