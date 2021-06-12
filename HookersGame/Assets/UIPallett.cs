using UnityEngine;
using System;


[CreateAssetMenu(fileName = "UIPalette",menuName = "ScriptableObjects/UI/PalettaForUI")]
public class UIPallett : ScriptableObject
{
    [Tooltip("Color List:\n 0 - Orange\n1 - Purple\n2- Sapphir\n3 - dark Purple")]
    [SerializeField] Color[] _colorPaletta;

    [Space(20f)]

    [Tooltip("0 - MainMenuBackground\n" +
        "1 - SettingWindow\n" +
        " 2 - levelBackgroundDefault\n" +
        "3 - UnlockBackgroundFeedback\n" +
        " 4 - LockedBackgroundFeedback\n" +
        " 5 - LockedBackgroundScore\n" +
        " 6 - UnlockBackgroundScore\n" +
        "7 - LockedLevelTextIcon\n" +
        "8 - UnlockedLevelTextIcon")]
    [SerializeField] Sprite[] _getSprite;


    public ref Sprite GetSprite(UISprite uIBackground)
    {
        int index = (int)uIBackground;

        if (index < 0 || index >= _getSprite.Length)
            return ref _getSprite[0];

        return ref _getSprite[index];
    }    
    public ref Color GetColorFrom(UIPallettColor color)
    {
        int index = (int)color;

        if (index < 0 || index >= _colorPaletta.Length)
            return ref _colorPaletta[0];

        return ref _colorPaletta[index];
    }

   
}
public enum UIPallettColor 
{ 
    Orange = 0,
    Purple = 1,
    Sapphir = 2,
    Dark_Purple = 3,
    Grey = 4
}
public enum UISprite
{
    MainMenuBackground = 0,
    SettingWindow = 1,
    levelBackgroundDefault = 2,
    UnlockBackgroundFeedback = 3,
    LockedBackgroundFeedback = 4,
    LockedBackgroundScore = 5,
    UnlockBackgroundScore = 6,
    lockTextScoreImage = 7,
    UnlockTextScoreImage = 8,
    EmptyMainMenuBackground =9
}
