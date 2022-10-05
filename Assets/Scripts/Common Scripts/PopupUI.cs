using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    private void OnEnable() => SoundManager.instance.PlaySFX(SoundFileNameDictionary.popupUIShowSound);
    private void OnDisable() => SoundManager.instance.PlaySFX(SoundFileNameDictionary.popupUIHideSound);
}
