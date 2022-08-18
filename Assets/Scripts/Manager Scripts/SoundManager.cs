using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    [SerializeField]
    private AudioSource _bgmAudioSource;
    [SerializeField]
    private AudioSource _sfxAudioSource;

    private float _bgmVolume = 1f;
    private float _sfxVolume = 1f;
    private bool _isBGMMuted;
    private bool _isSFXMuted;

    private Dictionary<string, AudioClip> _bgmAudioClipDict = new();
    private Dictionary<string, AudioClip> _sfxAudioClipDict = new();
    private Dictionary<string, int> _sfxAudioCountDict = new();

    private readonly WaitForSeconds _pointOneSeconds = new(0.1f);

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    public void LoadSceneSoundResource(string sceneName)
    {
        AudioClip[] audioClips;
        _bgmAudioClipDict.Clear();
        _sfxAudioClipDict.Clear();
        _sfxAudioCountDict.Clear();

        switch(sceneName)
        {
            case "SingleModeScene":
                audioClips = Resources.LoadAll<AudioClip>("Sounds/Stage/BGM");
                foreach(AudioClip audioClip in audioClips)
                    _bgmAudioClipDict.Add(audioClip.name, audioClip);

                audioClips = Resources.LoadAll<AudioClip>("Sounds/Stage/SFX");
                foreach(AudioClip audioClip in audioClips)
                {
                    _sfxAudioClipDict.Add(audioClip.name, audioClip);
                    _sfxAudioCountDict.Add(audioClip.name, 0);
                }
                break;

            case "LobbyScene":
                audioClips = Resources.LoadAll<AudioClip>("Sounds/Lobby/BGM");
                foreach(AudioClip audioClip in audioClips)
                    _bgmAudioClipDict.Add(audioClip.name, audioClip);

                audioClips = Resources.LoadAll<AudioClip>("Sounds/Lobby/SFX");
                foreach(AudioClip audioClip in audioClips)
                {
                    _sfxAudioClipDict.Add(audioClip.name, audioClip);
                    _sfxAudioCountDict.Add(audioClip.name, 0);
                }
                break;
        }
    }

    public void PlayBGM(string audioFileName)
    {
        AudioClip audioClip;

        if (!_bgmAudioClipDict.TryGetValue(audioFileName, out audioClip))
        {
            Debug.Log("할당되지 않은 오디오 파일을 참조하려 합니다.");
        }
        else
        {
            // 현재 BGM을 재생중이었다면 정지한다.
            if (_bgmAudioSource.isPlaying)
            {
                _bgmAudioSource.Stop();
            }
            
            if (_isBGMMuted)
                _bgmAudioSource.volume = 0f;
            else
                _bgmAudioSource.volume = _bgmVolume;

            _bgmAudioSource.clip = audioClip;
            _bgmAudioSource.Play();
        }
    }

    public void PlaySFX(string audioFileName)
    {
        AudioClip audioClip;

        if (!_sfxAudioClipDict.TryGetValue(audioFileName, out audioClip))
        {
            Debug.Log("할당되지 않은 오디오 파일을 참조하려 합니다.");
        }
        else
        {
            // 동일한 오디오가 5개 이상 중첩 재생중이라면 재생하지 않는다.
            if (_sfxAudioCountDict[audioFileName] >= 5)
                return;

            _sfxAudioCountDict[audioFileName]++;

            if (_isSFXMuted)
                _sfxAudioSource.PlayOneShot(audioClip, 0f);
            else
                _sfxAudioSource.PlayOneShot(audioClip, _sfxVolume);

            // 오디오 출력이 종료되면 카운트를 감소시키기 위한 코루틴 메소드.
            StartCoroutine(DecreaseAudioCountCoroutine(audioFileName, audioClip.length));
        }
    }

    private IEnumerator DecreaseAudioCountCoroutine(string audioFileName, float delay)
    {
        while(delay > 0f)
        {
            yield return _pointOneSeconds;
            delay -= 0.1f;
        }

        _sfxAudioCountDict[audioFileName]--;
    }

    public void ToggleIsBGMMuted()
    {
        _isBGMMuted = !_isBGMMuted;
    }

    public void ToggleIsSFXMuted()
    {
        _isSFXMuted = !_isSFXMuted;
    }

    public void SetBGMVolume(float volume)
    {
        _bgmVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = volume;
    }
}
