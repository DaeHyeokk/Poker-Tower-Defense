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
        // ����� ������(����, ���Ұ� ����)�� ����ȭ �Ѵ�.
        SyncAudioSetting();
        // ���带 �� ����ߴٸ� ������Ʈ�� �ݳ��Ѵ�.
        if (!_audioSource.isPlaying)
            SoundManager.instance.ReturnAudioSource(_audioSource.clip);
    }

    private void SyncAudioSetting()
    {
        _audioSource.volume = SoundManager.instance.sfxVolume;
        _audioSource.mute = SoundManager.instance.isSfxMuted;
    }
}
