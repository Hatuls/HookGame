using UnityEngine;

public class PlatformManager : MonoSingleton<PlatformManager> 
{
    [SerializeField]
    private Platform[] platformsArr;
    
    [SerializeField] LayerMask grabableLayer;





    public delegate void SetTexture();
    public static event SetTexture SetPlatformTexture;


    public LayerMask GetGrabableLayer => grabableLayer;
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


    public Transform GetClosestObjectTransform(Vector3 targetPos)
    {
        Transform closestObject = null;

        if (platformsArr != null && platformsArr.Length >= 1)
        {

            closestObject = platformsArr[0].transform;
            for (int i = 0; i < platformsArr.Length; i++)
            {
                if (targetPos.z > platformsArr[i].transform.position.z)
                    continue;

                if (Vector3.Distance(closestObject.position, targetPos) > Vector3.Distance(platformsArr[i].transform.position, targetPos))
                    closestObject = platformsArr[i].transform;
            }
        }
        return closestObject;
    }

 
}
