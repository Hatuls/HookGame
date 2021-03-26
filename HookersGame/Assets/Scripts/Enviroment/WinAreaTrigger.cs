﻿using UnityEngine;

public class WinAreaTrigger : MonoBehaviour
{
    bool isFlag = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && !isFlag)
            Win();
    }
    void Win() { isFlag = true;   LevelManager.Instance.LoadTheNextLevel(); }

}