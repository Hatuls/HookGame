using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAreaTrigger : MonoBehaviour
{
    bool isFlag;
    private void Start()
    {
        isFlag = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && !isFlag)
            Win();

    }
    void Win() { }
}
