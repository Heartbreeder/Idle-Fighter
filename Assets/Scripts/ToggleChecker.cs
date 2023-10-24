using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleChecker : MonoBehaviour
{ 

    [System.Serializable]
    public class OnCliclkEvent : UnityEvent { }

    public OnCliclkEvent OnIsOn, OnIsOff;


    public void ToggleOnOff()
    {
        if (GetComponent<UnityEngine.UI.Toggle>().isOn)
        {
            OnIsOn.Invoke();
        }
        else
        {
            OnIsOff.Invoke();
        }
    }
}

