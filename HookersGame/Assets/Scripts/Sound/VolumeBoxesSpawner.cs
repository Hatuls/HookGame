using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class VolumeBoxesSpawner
{
    static Coroutine distanceCheckerCorou;
    float distanceToCheck, xDistance;
    
    Transform playerTransform;
    Queue<Transform> leftLine, rightLine;

    Vector3 direction;
    int lastZIndex;
    public VolumeBoxesSpawner(in Transform PlayerTransform, ref Queue<Transform> leftLine, ref Queue<Transform> rightLine,ref Vector3 direction, ref float xDistance, ref float distanceChecker, ref int lastZIndex)
    {
        InitParams(PlayerTransform, ref leftLine,ref rightLine,ref direction ,  ref xDistance, ref distanceChecker, ref lastZIndex);
    }

    public void InitParams(in Transform PlayerTransform, ref Queue<Transform> leftLine, ref Queue<Transform> rightLine, ref Vector3 direction,ref float xDistance, ref float distanceChecker, ref int lastZIndex)
    {
        playerTransform = PlayerTransform;
        this.xDistance = xDistance;
        this.rightLine = rightLine;
        this.leftLine = leftLine;
        this.distanceToCheck = distanceChecker;
        this.lastZIndex = lastZIndex + 1;
        this.direction = direction;
        StartCoroutineCheck();
    }

    public void StartCoroutineCheck()
     => distanceCheckerCorou = LevelManager.Instance.StartCoroutine(Checker());

    public void StopCoroutineCheck() {
        if (distanceCheckerCorou != null)
            LevelManager.Instance.StopCoroutine(distanceCheckerCorou);
        
    }

    private Transform GetFurthestVolumeBoxTranform(ref Queue<Transform> line)
    {
        Transform cache = null;

        foreach (var item in line)
        {
            if (cache == null)
                cache = item;
          
            if (Vector3.Distance(playerTransform.position, item.position) > distanceToCheck)
                cache = item;
        }

        return cache;
    }
    private IEnumerator Checker()
    {
        Transform leftFurthest,rightFurthest;
        leftFurthest = GetFurthestVolumeBoxTranform(ref leftLine);
        rightFurthest = GetFurthestVolumeBoxTranform(ref rightLine);

        while (true)
        {
            Debug.Log(Vector3.Distance((leftFurthest.position + rightFurthest.position) / 2, playerTransform.position));

            if (Vector3.Distance((leftFurthest.position + rightFurthest.position)/2, playerTransform.position) < distanceToCheck)
            {

                Debug.LogError(Vector3.Distance((leftFurthest.position + rightFurthest.position) / 2, playerTransform.position));
                SetNewPosition(ref leftLine);
                SetNewPosition(ref rightLine);
                leftFurthest = GetFurthestVolumeBoxTranform(ref leftLine);
                rightFurthest = GetFurthestVolumeBoxTranform(ref rightLine);
     
            }

            yield return new WaitForSeconds(1f);

        }
    }

    private void SetNewPosition(ref Queue<Transform> line)
    {
        Transform cache;
        cache = line.Dequeue();
        cache.position += direction * lastZIndex * xDistance;
        line.Enqueue(cache);
    }

}