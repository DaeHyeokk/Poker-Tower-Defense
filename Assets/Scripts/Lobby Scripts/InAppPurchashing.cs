using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppPurchashing : MonoBehaviour
{
    public void Reward()
    {
        UIManager.instance.actionReconfirmation.gameObject.SetActive(true);
    }

    public void Failed()
    {
        
    }
}
