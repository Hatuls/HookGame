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
        setmaterialfloat("ForeGround_Lines_Speed", 0.1f, 1f);
        setmaterialfloat("ForeGround_lines_Scale_X", 1f, 2f);
        setmaterialfloat("ForeGround_Dots_Speed", 0.1f, 1f);
        setmaterialfloat("ForeGround_Dots_Scale_X", 1f, 2f);
        setmaterialfloat("BackGround_lines_Speed", 0.1f, 1f);
        setmaterialfloat("BackGround_Lines_Scale_X", 1f, 2f);
        setmaterialfloat("Background_Dots_Speed", 0.1f, 1f);
        setmaterialfloat("BackGround_Dots_Scale_X", 1f, 2f);
        
    }
    public void setmaterialfloat(string name, float min, float max) 
    {
        ENV_Building.SetFloat(name, Random.Range(min, max));
    }
   
}
