using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax;
using System;
public class TriggerChecker : MonoBehaviour
{
    public Action<bool> trigger;

    public void OnFixedTriggerEnter()
    {
        trigger.Invoke(true);
    }

    public void OnFixedTriggerExit()
    {
        trigger.Invoke(false);
    }
}
