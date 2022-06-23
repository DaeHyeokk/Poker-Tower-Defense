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

    private readonly float _fadeOutDelay = 0.5f;
    private readonly WaitForFixedUpdate _waitForFixedUpdate = new();

    private void OnEnable()
    {
        if (_waveSystem.isBossWave)
            _waveWarningText.gameObject.SetActive(true);
        else
            _waveWarningText.gameObject.SetActive(false);

        _waveStartText.text = "웨이브 " + _waveSystem.wave.ToString();

        StartCoroutine(AlphaLerfCoroutine(0f, 1f));
    }

    private IEnumerator AlphaLerfCoroutine(float start, float end)
    {
        float fixedUnscaledDeltaTime;
        float currentTime = 0f;
        float percent = 0f;

        while (percent < 1)
        {
            if (GameManager.instance.isPausing)
            {
                yield return GameResumeWaitCoroutine();
                // 게임이 일시정지 됐다가 다시 재개 되면 FixedUpdate 텀이 길기 때문에 Time.fixedUnscaledDeltaTime 가 매우 큰 값을 가지고 있게됨.
                // 따라서 한 타임 넘겨줌으로써 Time.fixedUnscaledDeltaTime 을 정상값으로 되돌림.
                yield return _waitForFixedUpdate;
            }

            fixedUnscaledDeltaTime = Time.fixedUnscaledDeltaTime;
            currentTime += fixedUnscaledDeltaTime;
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

            yield return _waitForFixedUpdate;
        }

        float fadeOutDelay = _fadeOutDelay;
        while(fadeOutDelay > 0)
        {
            if (GameManager.instance.isPausing)
            {
                yield return GameResumeWaitCoroutine();
                // 게임이 일시정지 됐다가 다시 재개 되면 FixedUpdate 텀이 길기 때문에 Time.fixedUnscaledDeltaTime 가 매우 큰 값을 가지고 있게됨.
                // 따라서 한 타임 넘겨줌으로써 Time.fixedUnscaledDeltaTime 을 정상값으로 되돌림.
                yield return _waitForFixedUpdate;
            }

            yield return _waitForFixedUpdate;

            fixedUnscaledDeltaTime = Time.fixedUnscaledDeltaTime;
            fadeOutDelay -= fixedUnscaledDeltaTime;
        }
        

        while (percent > 0)
        {
            if (GameManager.instance.isPausing)
            {
                yield return GameResumeWaitCoroutine();
                // 게임이 일시정지 됐다가 다시 재개 되면 FixedUpdate 텀이 길기 때문에 Time.fixedUnscaledDeltaTime 가 매우 큰 값을 가지고 있게됨.
                // 따라서 한 타임 넘겨줌으로써 Time.fixedUnscaledDeltaTime 을 정상값으로 되돌림.
                yield return _waitForFixedUpdate;
            }

            fixedUnscaledDeltaTime = Time.fixedUnscaledDeltaTime;
            currentTime -= Time.fixedUnscaledDeltaTime;
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

            yield return _waitForFixedUpdate;
        }

        gameObject.SetActive(false);
    }

    private IEnumerator GameResumeWaitCoroutine()
    {
        while (GameManager.instance.isPausing)
            yield return _waitForFixedUpdate;
    }
}
