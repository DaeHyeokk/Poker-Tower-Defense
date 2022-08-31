using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SoundManager>();

            return _instance;
        }
    }

    [SerializeField]
    private AudioSource _bgmAudioSource;
    [SerializeField]
    private AudioSource _commonSfxAudioSource;
    [SerializeField]
    private GameObject _sfxAudioSourcePrefab;
    private ObjectPool<SFXAudioSource> _sfxAudioSourcePool;

    [Header("Common Audio Clips")]
    [SerializeField]
    private AudioClip _buttonClickAudioClip;
    [SerializeField]
    private AudioClip _buttonToggleAudioClip;
    [SerializeField]
    private AudioClip _errorAudioClip;

    private Scene _currentScene;

    private Dictionary<AudioClip, Queue<SFXAudioSource>> _sfxPlayingAudioClipDict = new();

    private float _bgmVolume;
    private float _sfxVolume;
    private bool _isBgmMuted;
    private bool _isSfxMuted;
    
    public float bgmVolume
    {
        get => _bgmVolume;
        set
        {
            _bgmVolume = value;
            _bgmAudioSource.volume = value;
        }
    }
    public float sfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            _commonSfxAudioSource.volume = value;
        }
    }

    public bool isBgmMuted
    {
        get => _isBgmMuted;
        set
        {
            _isBgmMuted = value;
            _bgmAudioSource.mute = value;
        }
    }

    public bool isSfxMuted
    {
        get => _isSfxMuted;
        set
        {
            _isSfxMuted = value;
            _commonSfxAudioSource.mute = value;
        }
    }
    

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        else
        {
            // SoundManager가 최초 생성되는 경우.
            // 씬이 종료되어도 파괴되지 않는 오브젝트로 설정한다.
            DontDestroyOnLoad(instance);
            // 로컬디스크에 저장된 PlayerPrefs 데이터를 가져와 동기화한다.
            LoadPlayerPrefsSoundData();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        // 원래 씬과 새로 로드하는 씬이 서로 다른 씬일 경우에만 리소스 파일을 로딩한다.
        if (instance._currentScene != scene)
        {
            instance.AddListenerAllButtonAndToggle();
            instance._currentScene = scene;
        }

        // 씬에서 SoundSettingUIController 오브젝트를 찾고, 씬에 존재할 경우 사운드 설정값을 동기화한다.
        SoundSettingUIController soundSettingUIController = FindObjectOfType<SoundSettingUIController>(true);
        if (soundSettingUIController != null)
            soundSettingUIController.SyncSoundSettingUI();

        if(_sfxAudioSourcePool != null) 
            _sfxAudioSourcePool.ClearObjectPool();
        _sfxAudioSourcePool = new(_sfxAudioSourcePrefab, 10);

        if (_sfxPlayingAudioClipDict.Count > 0)
            _sfxPlayingAudioClipDict.Clear();

        instance.PlayBGM(SoundFileNameDictionary.mainBGM);
    }

    private void AddListenerAllButtonAndToggle()
    {
        // 로딩된 씬에 존재하는 모든 버튼 컴포넌트를 가져온다음,
        // 버튼 클릭 이벤트 발생 시 효과음을 재생하도록 리스너에 추가한다.
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button button in buttons)
            button.onClick.AddListener(PlayButtonClickSound);

        // 로딩된 씬에 존재하는 모든 토글 컴포넌트를 가져온다음,
        // 토글 변경 이벤트 발생 시 효과음을 재생하도록 리스너에 추가한다.
        Toggle[] toggles = FindObjectsOfType<Toggle>(true);
        foreach (Toggle toggle in toggles)
            toggle.onValueChanged.AddListener(PlayButtonToggleSound);
    }

    private void LoadPlayerPrefsSoundData()
    {
        // PlayerPrefs에서 키의 존재 여부를 확인하고 키가 존재하는 경우 저장된 값으로, 키가 존재하지 않는 경우 기본값으로 대입해준다.
        bgmVolume = PlayerPrefs.HasKey("BgmVolume") ? PlayerPrefs.GetFloat("BgmVolume") : 1f;
        sfxVolume = PlayerPrefs.HasKey("SfxVolume") ? PlayerPrefs.GetFloat("SfxVolume") : 1f;

        // BGM, SFX 음소거 여부의 기본값은 False이다.
        // 따라서 키가 존재하지 않을 경우 GetString()의 반환값이 빈문자열이라는 특징을 이용해 "True"와 비교하여 값을 대입해주는 로직을 작성하였다.
        isBgmMuted = PlayerPrefs.GetString("IsBgmMuted") == "True" ? true : false;
        isSfxMuted = PlayerPrefs.GetString("IsSfxMuted") == "True" ? true : false;
    }

    // 앱을 닫거나 다른앱으로 전환하거나 종료될 때 호출.
    private void OnApplicationPause(bool pause)
    {
        // 앱을 중지하는 경우 현재 사운드 설정값을 디스크에 저장한다.
        if (pause)
            SoundManager.instance.SavePlayerPrefsSoundData();
    }

    private void SavePlayerPrefsSoundData()
    {
        PlayerPrefs.SetFloat("BgmVolume", _bgmAudioSource.volume);
        PlayerPrefs.SetFloat("SfxVolume", _commonSfxAudioSource.volume);
        PlayerPrefs.SetString("IsBgmMuted", _bgmAudioSource.mute.ToString());
        PlayerPrefs.SetString("IsSfxMuted", _commonSfxAudioSource.mute.ToString());

        PlayerPrefs.Save();
    }

    public void PlayBGM(string audioClipName)
    {
        AudioClip audioClip = AudioClipManager.instance.GetBgmAudioClip(audioClipName);

        // 전에 플레이 중이던 BGM을 정지한다.
        _bgmAudioSource.Stop();

        _bgmAudioSource.clip = audioClip;

        _bgmAudioSource.Play();
    }

    public void PauseBGM()
    {
        if(_bgmAudioSource.isPlaying)
            _bgmAudioSource.Pause();
    }

    public void ResumeBGM()
    {
        if (!_bgmAudioSource.isPlaying)
            _bgmAudioSource.Play();
    }

    public void PlaySFX(string audioClipName)
    {
        AudioClip audioClip = AudioClipManager.instance.GetSfxAudioClip(audioClipName);

        if (!_sfxPlayingAudioClipDict.ContainsKey(audioClip))
            _sfxPlayingAudioClipDict.Add(audioClip, new Queue<SFXAudioSource>());

        // 동일한 오디오가 10개 이상 중첩 재생중이라면 가장 먼저 플레이한 사운드를 중지한다.
        if (_sfxPlayingAudioClipDict[audioClip].Count >= 10)
            ReturnAudioSource(audioClip);

        SFXAudioSource sfxAudioSource = _sfxAudioSourcePool.GetObject();
        _sfxPlayingAudioClipDict[audioClip].Enqueue(sfxAudioSource);
        sfxAudioSource.audioSource.clip = audioClip;
        sfxAudioSource.audioSource.Play();
    }

    public void PlayButtonClickSound()
    {
        _commonSfxAudioSource.clip = _buttonClickAudioClip;
        _commonSfxAudioSource.Play();
    }

    public void PlayButtonToggleSound(bool value)
    {
        _commonSfxAudioSource.clip = _buttonToggleAudioClip;
        _commonSfxAudioSource.Play();
    }

    public void PlayErrorSound()
    {
        _commonSfxAudioSource.PlayOneShot(_errorAudioClip);
    }

    public void ReturnAudioSource(AudioClip audioClip)
    {
        SFXAudioSource sfxAudioSource;
        if (_sfxPlayingAudioClipDict[audioClip].TryDequeue(out sfxAudioSource))
        {
            sfxAudioSource.audioSource.Stop();
            _sfxAudioSourcePool.ReturnObject(sfxAudioSource);
        }
        else
        {
            throw new System.Exception("예상보다 호출횟수가 많습니다. 코드를 점검하세요");
        }
    }
}
