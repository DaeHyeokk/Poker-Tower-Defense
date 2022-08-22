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
        SyncAudioSetting();
        _audioSource.loop = false;
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
