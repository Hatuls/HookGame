using System;
using UnityEngine;

public class PlatformManager : MonoSingleton<PlatformManager> {
    [SerializeField]
    private Platform[] platformsArr;
    [SerializeField] float distanceRadiusCheck ;






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


    public bool CheckEnviroment(Vector3 targetPos) {
        bool isOverLappingObject = false;
      
        for (int i = 0; i < platformsArr.Length; i++)
        {
            
            Vector3 posWithScale = platformsArr[i].transform.position;
            isOverLappingObject |= Vector3.Distance(targetPos, posWithScale)-.5f < distanceRadiusCheck;

                if (isOverLappingObject)
                break;
        }
        
        return isOverLappingObject;
    }
}