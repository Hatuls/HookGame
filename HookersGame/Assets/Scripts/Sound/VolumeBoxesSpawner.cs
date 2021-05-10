using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class VolumeBoxesSpawner
{
    static Coroutine distanceCheckerCorou;
    float distanceFromPlayer, xDistance;
    Vector3 direction;
    Transform playerTransform;
    Queue<Transform> leftLine,rightLine;
    int lastZIndex;
    public VolumeBoxesSpawner(in Transform PlayerTransform, ref Queue<Transform> leftLine, ref Queue<Transform> rightLine, ref float xDistance, ref float distanceChecker, ref int lastZIndex , ref Vector3 direction)
    {
        InitParams(PlayerTransform, ref leftLine,ref rightLine, ref xDistance, ref distanceChecker, ref lastZIndex , ref  direction);
    }

    public void InitParams(in Transform PlayerTransform, ref Queue<Transform> leftLine, ref Queue<Transform> rightLine, ref float xDistance, ref float distanceChecker, ref int lastZIndex, ref Vector3 direction)
    { 
        playerTransform = PlayerTransform;
        this.xDistance = xDistance;
        this.leftLine = leftLine;
        this.rightLine = rightLine;
        this.distanceFromPlayer = distanceChecker;
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

            else if (Mathf.Abs(item.position.z - playerTransform.position.z) > distanceFromPlayer)
                cache = item;
        }

        return cache;
    }
    private IEnumerator Checker()
    {
        Transform leftFurthest, rightFurthest;
        leftFurthest = GetFurthestVolumeBoxTranform(ref leftLine);
        rightFurthest = GetFurthestVolumeBoxTranform(ref rightLine);

        while (true)
        {
         
            if (Vector3.Distance((leftFurthest.position + rightFurthest.position) / 2, playerTransform.position) < distanceFromPlayer)
            {
               
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
        if (line == null || line.Count == 0)
            return;

        Transform cache;
        cache = line.Dequeue();

            cache.position += direction * lastZIndex * xDistance;

        line.Enqueue(cache);
    }

}