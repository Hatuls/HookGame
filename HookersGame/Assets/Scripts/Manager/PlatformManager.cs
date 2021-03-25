using UnityEngine;
using System.Collections;
public class PlatformManager : MonoSingleton<PlatformManager>
{
    [SerializeField]
    private Platform[] platformsArr;
    private DeathWall deathWall;
    [SerializeField] LayerMask grabableLayer;



    public delegate void ResetTransform();
    public static event ResetTransform ResetPlatformEvent;

    public delegate void SetTexture();
    public static event SetTexture SetPlatformTexture;


    public LayerMask GetGrabableLayer => grabableLayer;
    public override void Init()
    {
        ResetValues();
    }
    public void ResetValues()
    {
        ResetPlatforms();
        platformsArr = null;
        platformsArr = FindObjectsOfType<Platform>();

        for (int i = 0; i < platformsArr.Length; i++)
        {
            platformsArr[i].SubscribePlatform();
        }

        deathWall = FindObjectOfType<DeathWall>();



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

    public void StartChecking()
      => StartCoroutine(CheckDistanceFromVoid(platformsArr));

    public void ResetPlatforms()
    {
        flag = false;
        StopAllCoroutines();
        ResetPlatformEvent?.Invoke();
    }
    bool flag;
    IEnumerator CheckDistanceFromVoid(Platform[] platforms)
    {

        flag = true;
        float timer = 0.3f;
        float distanceFromStarting = 50f;
        Transform deathWallCache = deathWall.transform;
        byte counter = 0;
        while (flag)
        {
            if (counter == platforms.Length - 1)
                break;

            for (int i = 0; i < platforms.Length; i++)
            {
                if (platforms[i] == null)
                    continue;


                if (Mathf.Abs(deathWallCache.position.z - platforms[i].transform.position.z) < distanceFromStarting)
                {
                    counter++;
                    platforms[i].MoveToward(deathWall.transform);
                    platforms[i] = null;
                }
            }



            yield return new WaitForSeconds(timer);
        }
        flag = false;
    }
}
