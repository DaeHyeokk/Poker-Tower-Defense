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

    // ������Ʈ�� Ȱ��ȭ �� �� ���� �������� ����ȭ �Ѵ�.
    // Update()���� ����ȭ�ϴ� �Ϳ� ������ ��� ���ҰŸ� �Ͽ��� �Ҹ��� Update()�� ���� ����ȭ �Ǳ� ���� 1������ ��µǴ� ������ ����. 
    private void OnEnable()
    {
        SyncAudioSetting();
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
