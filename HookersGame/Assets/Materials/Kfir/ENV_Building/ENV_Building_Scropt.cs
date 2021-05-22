using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENV_Building_Scropt : MonoBehaviour
{
    Material ENV_Building;

    // Start is called before the first frame update
    void Start()
    {
        ENV_Building = GetComponent<MeshRenderer>().material;
        SetFloats();
    }
    public void SetFloats()
    {
        SetMatFloat("ForeGround_Speed", -1f, 1f);
        SetMatFloat("ForeGround_lines_Scale_X_", -2f, 2f);
        SetMatFloat("ForeGround_Dots_Speed", -1f, 1f);
        SetMatFloat("BackGround_Dots_Scale_X_", -2f, 2f);
        SetMatFloat("BackGround_lines_Speed", -1f, 1f);
        SetMatFloat("BackGround_Lines_Scale_X_", -1f, 2f);
        SetMatFloat("Background_Dots_Speed", -1f, 1f);
        SetMatFloat("BackGround_Dots_Scale_X__1", -2f, 2f);

    }
    public void SetMatFloat(string name, float min, float max) 
    {
        ENV_Building.SetFloat(name, Random.Range(min, max));
    }
    

}
