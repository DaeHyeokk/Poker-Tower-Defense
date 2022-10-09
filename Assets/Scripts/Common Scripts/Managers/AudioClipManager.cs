using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    private static AudioClipManager s_instance;
    public static AudioClipManager instance
    {
        get
        {
            if (s_instance == null)
            {
                // 씬에서 StageUIManager 오브젝트를 찾아 할당
                s_instance = FindObjectOfType<AudioClipManager>();
            }

            return s_instance;
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
        {
            Destroy(gameObject);    // 자신을 파괴
            return;
        }

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
            throw new System.Exception("잘못된 이름의 오디오클립 파일을 참조하고 있습니다. 파일 이름을 확인하세요");
        }
    }

    public AudioClip GetSfxAudioClip(string clipName)
    {
        AudioClip audioClip;

        if (_sfxAudioClipDict.TryGetValue(clipName, out audioClip))
            return audioClip;
        else
        {
            throw new System.Exception("잘못된 이름의 오디오클립 파일을 참조하고 있습니다. 파일 이름을 확인하세요");
        }
    }
}
