using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialToggle : MonoBehaviour
{

    [SerializeField] GameObject tutorialGo;
    bool toggle = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (toggle) { tutorialGo.SetActive(false); toggle = false; }
            else { tutorialGo.SetActive(true); toggle = true; }

        }   
    }
}
