using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _loadingText;
    [SerializeField]
    private string _loadingTypeString;

    private readonly WaitForSeconds _textChangeDelay = new(0.7f);

    public string loadingTypeString
    {
        get => _loadingTypeString;
        set
        {
            _loadingTypeString = value;
            _loadingText.text = value;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ChangeTextCoroutine());
    }

    private IEnumerator ChangeTextCoroutine()
    {
        while(true)
        {
            _loadingText.text = _loadingTypeString;
            yield return _textChangeDelay;

            _loadingText.text = _loadingTypeString + ".";
            yield return _textChangeDelay;

            _loadingText.text = _loadingTypeString + ". .";
            yield return _textChangeDelay;

            _loadingText.text = _loadingTypeString + ". . .";
            yield return _textChangeDelay;
        }
    }
}
