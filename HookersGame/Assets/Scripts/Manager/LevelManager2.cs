using UnityEngine;
using System.Collections;


public  partial class LevelManager 
{

    public Platform[] platformsArr;
   public LayerMask grabableLayer;
    bool flag;
    private DeathWall deathWall;

    




    public DeathWall GetDeathWall => deathWall;
    public LayerMask GetGrabableLayer => grabableLayer;




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
    => CheckPlatformVoidDistance = StartCoroutine(CheckDistanceFromVoid(platformsArr));

   
    IEnumerator CheckDistanceFromVoid( Platform[] platforms)
    {
        if (platforms == null || platforms.Length == 0)
            yield break;

        float timer = 0.3f;
        float distanceFromStarting = 50f;
        Transform deathWallCache = deathWall.transform;
        byte[] platformIndex = new byte[platforms.Length];
        flag = true;
        
        while (flag)
        {
            for (byte i = 1; i <= platforms.Length; i++)
            {
                if (!flag)
                   yield break;

                if ( i == platformIndex[i-1]|| platforms[i-1] == null)
                    continue;

                if (Mathf.Abs(deathWallCache.position.z - platforms[i-1].transform.position.z) < distanceFromStarting)
                {
                    platforms[i-1].SuckTowards(deathWall.transform);
                    platformIndex[i-1] = i;
                }
            }
            yield return new WaitForSeconds(timer);

        }
        flag = false;

    }
}
