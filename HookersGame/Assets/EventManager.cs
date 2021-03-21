using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction Reset;

    public void ResetValues()
    {
        if (Reset != null)
        {
            Reset();
        }
    }



}
