using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAudioSource : MonoBehaviour
{
    private AudioSource _audioSource;

    public AudioSource audioSource => _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
    }

    // 오브젝트가 활성화 될 때 사운드 설정값을 동기화 한다.
    // Update()에서 동기화하는 것에 의존할 경우 음소거를 하여도 소리가 Update()에 의해 동기화 되기 전에 1프레임 출력되는 문제가 있음. 
    private void OnEnable()
    {
        SyncAudioSetting();
    }

    private void Update()
    {
        // 오디오 설정값(볼륨, 음소거 여부)을 동기화 한다.
        SyncAudioSetting();
        // 사운드를 다 재생했다면 오브젝트를 반납한다.
        if (!_audioSource.isPlaying)
            SoundManager.instance.ReturnAudioSource(_audioSource.clip);
    }

    private void SyncAudioSetting()
    {
        _audioSource.volume = SoundManager.instance.sfxVolume;
        _audioSource.mute = SoundManager.instance.isSfxMuted;
    }
}
