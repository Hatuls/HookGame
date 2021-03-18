using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI AmmoText;
    [SerializeField] private Slider HpSlider;
    [SerializeField] private Slider ManaSlider;
    [SerializeField] private Slider ExpSlider;

    private void Start()
    {
        
    }
    public void UpdateAmmoText(int inMagazine,int Total)
    {
        string ammoString = inMagazine + "/" + Total;
        AmmoText.text = ammoString;
    }
    public void UpdateHp(int Hp,int MaxHp)
    {
        HpSlider.maxValue = MaxHp;
        HpSlider.minValue = 0;
        HpSlider.value = Hp;
        
    }
    public void UpdateMana(int Mana, int maxMana)
    {
        ManaSlider.maxValue = maxMana;
        ManaSlider.minValue = 0;
        ManaSlider.value = Mana;
    }
    public void UpdateExp(float currentExp, float MaxExp)
    {
        ExpSlider.maxValue = MaxExp;
        ExpSlider.minValue = 0;
        ExpSlider.value = currentExp;
    }

}

