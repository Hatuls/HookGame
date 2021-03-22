using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI SpeedText;
    [SerializeField] private GameObject WinImage;
    [SerializeField] private Slider HpSlider;


 
    private void Start()
    {
        
    }
    public void TriggerUi(int number)
    {
        
        string String = number + "Km/h";
        SpeedText.text = String;
    }

    public void UpdateHp(int Hp,int MaxHp)
    {
        HpSlider.maxValue = MaxHp;
        HpSlider.minValue = 0;
        HpSlider.value = Hp;
        
    }

    public void ResetUi()
    {
        SpeedText.text = "0";
        WinImage.SetActive(false);



    }
    public void Win()
    {
        Debug.Log("yo");
        WinImage.SetActive(true);
    }
   

}

