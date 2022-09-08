using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionReconfirmation: MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _actionReconfirmationText;

    public event Action onYesButton;

    public TextMeshProUGUI actionReconfirmationText => _actionReconfirmationText;

    private void OnDisable()
    {
        onYesButton = null;
    }

    public void OnClickYesButton()
    {
        if(onYesButton != null) 
            onYesButton();
    }
}
