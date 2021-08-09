using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour, IMenuHandler
{
    public void Init(ref UIPallett uIPallett)
    {
        gameObject.SetActive(true);
    }

    public void OnEnd()
    {
        gameObject.SetActive(false);
    }
}
