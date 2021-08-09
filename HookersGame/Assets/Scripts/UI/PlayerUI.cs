using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro speedText;
    [SerializeField] private Image SpeedoMeter;
    [SerializeField] private GameObject WinImage;
    [SerializeField] private Slider HpSlider;
    [SerializeField] private Image InnerCourser;


 
    private void Start()
    {
        
    }
    public void TriggerUi(float number)
    {

        string String = (int)number +" ";

        speedText.text = String;

        SpeedoMeter.fillAmount = number/300;
    }

    public void SetCourserColor(bool turnOn)
    {
        if (turnOn)
        {
            InnerCourser.color = Color.green;
        }
        else
        {
            InnerCourser.color = Color.red;

        }
    }

    public void UpdateHp(int Hp,int MaxHp)
    {
        
        HpSlider.maxValue = MaxHp;
        HpSlider.minValue = 0;
        HpSlider.value = Hp;
        
    }

    public void ResetUi()
    {
        speedText.text = "0";
        WinImage.SetActive(false);



    }
    public void Win()
    {
        Debug.Log("yo");
        WinImage.SetActive(true);
    }
   

}

