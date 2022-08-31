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
            // SoundManager�� ���� �����Ǵ� ���.
            // ���� ����Ǿ �ı����� �ʴ� ������Ʈ�� �����Ѵ�.
            DontDestroyOnLoad(instance);
            // ���õ�ũ�� ����� PlayerPrefs �����͸� ������ ����ȭ�Ѵ�.
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
        // ���� ���� ���� �ε��ϴ� ���� ���� �ٸ� ���� ��쿡�� ���ҽ� ������ �ε��Ѵ�.
        if (instance._currentScene != scene)
        {
            instance.AddListenerAllButtonAndToggle();
            instance._currentScene = scene;
        }

        // ������ SoundSettingUIController ������Ʈ�� ã��, ���� ������ ��� ���� �������� ����ȭ�Ѵ�.
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
        // �ε��� ���� �����ϴ� ��� ��ư ������Ʈ�� �����´���,
        // ��ư Ŭ�� �̺�Ʈ �߻� �� ȿ������ ����ϵ��� �����ʿ� �߰��Ѵ�.
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button button in buttons)
            button.onClick.AddListener(PlayButtonClickSound);

        // �ε��� ���� �����ϴ� ��� ��� ������Ʈ�� �����´���,
        // ��� ���� �̺�Ʈ �߻� �� ȿ������ ����ϵ��� �����ʿ� �߰��Ѵ�.
        Toggle[] toggles = FindObjectsOfType<Toggle>(true);
        foreach (Toggle toggle in toggles)
            toggle.onValueChanged.AddListener(PlayButtonToggleSound);
    }

    private void LoadPlayerPrefsSoundData()
    {
        // PlayerPrefs���� Ű�� ���� ���θ� Ȯ���ϰ� Ű�� �����ϴ� ��� ����� ������, Ű�� �������� �ʴ� ��� �⺻������ �������ش�.
        bgmVolume = PlayerPrefs.HasKey("BgmVolume") ? PlayerPrefs.GetFloat("BgmVolume") : 1f;
        sfxVolume = PlayerPrefs.HasKey("SfxVolume") ? PlayerPrefs.GetFloat("SfxVolume") : 1f;

        // BGM, SFX ���Ұ� ������ �⺻���� False�̴�.
        // ���� Ű�� �������� ���� ��� GetString()�� ��ȯ���� ���ڿ��̶�� Ư¡�� �̿��� "True"�� ���Ͽ� ���� �������ִ� ������ �ۼ��Ͽ���.
        isBgmMuted = PlayerPrefs.GetString("IsBgmMuted") == "True" ? true : false;
        isSfxMuted = PlayerPrefs.GetString("IsSfxMuted") == "True" ? true : false;
    }

    // ���� �ݰų� �ٸ������� ��ȯ�ϰų� ����� �� ȣ��.
    private void OnApplicationPause(bool pause)
    {
        // ���� �����ϴ� ��� ���� ���� �������� ��ũ�� �����Ѵ�.
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

        // ���� �÷��� ���̴� BGM�� �����Ѵ�.
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

        // ������ ������� 10�� �̻� ��ø ������̶�� ���� ���� �÷����� ���带 �����Ѵ�.
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
            throw new System.Exception("���󺸴� ȣ��Ƚ���� �����ϴ�. �ڵ带 �����ϼ���");
        }
    }
}
