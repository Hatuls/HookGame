using System;
using UnityEngine;

public class PlatformManager : MonoSingleton<PlatformManager> {
    [SerializeField]
    private Platform[] platformsArr;







    public delegate void SetTexture();
    public static event SetTexture SetPlatformTexture;
    public override void Init()
    {
        ResetValues();
    }
    public void ResetValues()
    {
        platformsArr = null;
        platformsArr = FindObjectsOfType<Platform>();

        for (int i = 0; i < platformsArr.Length; i++)
            platformsArr[i].SubscribePlatform();
        

        SetPlatformTexture?.Invoke();
    }
}