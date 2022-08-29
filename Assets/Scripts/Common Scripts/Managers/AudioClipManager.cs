using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    private static AudioClipManager _instance;
    public static AudioClipManager instance
    {
        get
        {
            if (_instance == null)
            {
                // ������ StageUIManager ������Ʈ�� ã�� �Ҵ�
                _instance = FindObjectOfType<AudioClipManager>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private AudioClip[] _bgmAudioClips;
    [SerializeField]
    private AudioClip[] _sfxAudioClips;

    private Dictionary<string, AudioClip> _bgmAudioClipDict = new();
    private Dictionary<string, AudioClip> _sfxAudioClipDict = new();

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);    // �ڽ��� �ı�

        foreach (AudioClip bgmAudioClip in _bgmAudioClips)
            _bgmAudioClipDict.Add(bgmAudioClip.name, bgmAudioClip);

        foreach (AudioClip sfxAudioClip in _sfxAudioClips)
            _sfxAudioClipDict.Add(sfxAudioClip.name, sfxAudioClip);
    }

    public AudioClip GetBgmAudioClip(string clipName)
    {
        AudioClip audioClip;

        if (_bgmAudioClipDict.TryGetValue(clipName, out audioClip))
            return audioClip;
        else
        {
            throw new System.Exception("�߸��� �̸��� �����Ŭ�� ������ �����ϰ� �ֽ��ϴ�. ���� �̸��� Ȯ���ϼ���");
        }
    }

    public AudioClip GetSfxAudioClip(string clipName)
    {
        AudioClip audioClip;

        if (_sfxAudioClipDict.TryGetValue(clipName, out audioClip))
            return audioClip;
        else
        {
            throw new System.Exception("�߸��� �̸��� �����Ŭ�� ������ �����ϰ� �ֽ��ϴ�. ���� �̸��� Ȯ���ϼ���");
        }
    }
}
