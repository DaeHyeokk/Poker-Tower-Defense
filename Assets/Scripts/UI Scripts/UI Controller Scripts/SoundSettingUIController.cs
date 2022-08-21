using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettingUIController : MonoBehaviour
{
    [SerializeField]
    private Image _bgmMuteButtonBackGround;
    [SerializeField]
    private TextMeshProUGUI _bgmMuteText;
    [SerializeField]
    private Slider _bgmVolumeSlider;
    [SerializeField]
    private Image _sfxMuteButtonBackGround;
    [SerializeField]
    private TextMeshProUGUI _sfxMuteText;
    [SerializeField]
    private Slider _sfxVolumeSlider;

    private void Awake()
    {
        SyncSoundSettingUI();
    }

    private void SyncSoundSettingUI()
    {
        SetBGMMuteButton();
        SetSFXMuteButton();

        _bgmVolumeSlider.value = SoundManager.instance.bgmAudioSource.volume;
        _sfxVolumeSlider.value = SoundManager.instance.sfxAudioSource.volume;
    }

    public void OnClickBGMMuteButton()
    {
        SoundManager.instance.bgmAudioSource.mute = !(SoundManager.instance.bgmAudioSource.mute);
        SetBGMMuteButton();
    }

    public void OnClickSFXMuteButton()
    {
        SoundManager.instance.sfxAudioSource.mute = !(SoundManager.instance.sfxAudioSource.mute);
        SetSFXMuteButton();
    }

    public void OnValueChangeBGMVolumeSlider()
    {
        SoundManager.instance.bgmAudioSource.volume = _bgmVolumeSlider.value;
    }

    public void OnValueChangeSFXVolumeSlider()
    {
        SoundManager.instance.sfxAudioSource.volume = _sfxVolumeSlider.value;
    }

    private void SetBGMMuteButton()
    {
        if (SoundManager.instance.bgmAudioSource.mute)
        {
            Color color = _bgmMuteButtonBackGround.color;
            color = Color.gray;
            _bgmMuteButtonBackGround.color = color;
            _bgmMuteText.text = "OFF";
        }
        else
        {
            Color color = _bgmMuteButtonBackGround.color;
            color = Color.red;
            _bgmMuteButtonBackGround.color = color;
            _bgmMuteText.text = "ON";
        }
    }

    private void SetSFXMuteButton()
    {
        if (SoundManager.instance.sfxAudioSource.mute)
        {
            Color color = _sfxMuteButtonBackGround.color;
            color = Color.gray;
            _sfxMuteButtonBackGround.color = color;
            _sfxMuteText.text = "OFF";
        }
        else
        {
            Color color = _sfxMuteButtonBackGround.color;
            color = Color.blue;
            _sfxMuteButtonBackGround.color = color;
            _sfxMuteText.text = "ON";
        }
    }
}
