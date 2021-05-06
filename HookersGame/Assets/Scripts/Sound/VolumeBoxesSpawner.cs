using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class VolumeBoxesSpawner
{
    static Coroutine distanceCheckerCorou;
    float distanceToCheck, xDistance;

    Vector3 direction;

    Transform playerTransform;
    Queue<Transform> leftRow;
    Queue<Transform> rightRow;
    int lastZIndex;
    public VolumeBoxesSpawner(in Transform PlayerTransform, ref Queue<Transform> rightRow, ref Queue<Transform> leftRow, ref float xDistance, ref float distanceChecker, ref int lastZIndex ,ref Vector3 direction)
    {

        InitParams(PlayerTransform, ref rightRow, ref leftRow, ref xDistance, ref distanceChecker, ref lastZIndex,ref direction);
    }

    public void InitParams(in Transform PlayerTransform, ref Queue<Transform> rightRow, ref Queue<Transform> leftRow, ref float xDistance, ref float distanceChecker, ref int lastZIndex , ref Vector3 direction)
    {
        playerTransform = PlayerTransform;
        this.xDistance = xDistance;
        this.leftRow = leftRow;
        this.rightRow = rightRow;
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

    private Transform GetFurthestVolumeBoxTranform(ref Queue<Transform> row)
    {
        Transform cache = null;

        foreach (var item in row)
        {
            if (cache == null)
                cache = item;

            else if (Mathf.Abs(item.position.z - playerTransform.position.z) > distanceToCheck)
                cache = item;
        }

        return cache;
    }
    private IEnumerator Checker()
    {
        Transform leftFurthestFromPlayer, rightFurthestFromPlayer ;

        leftFurthestFromPlayer = GetFurthestVolumeBoxTranform(ref leftRow);
        rightFurthestFromPlayer = GetFurthestVolumeBoxTranform(ref rightRow);

        while (true)
        {

            if (Mathf.Abs(leftFurthestFromPlayer.position.z - playerTransform.position.z) < distanceToCheck
                || Mathf.Abs(rightFurthestFromPlayer.position.z - playerTransform.position.z) < distanceToCheck)
            {

                SetNewPosition(ref leftRow);
                SetNewPosition(ref rightRow);

                leftFurthestFromPlayer = GetFurthestVolumeBoxTranform(ref leftRow);
                rightFurthestFromPlayer = GetFurthestVolumeBoxTranform(ref rightRow);
            }

            yield return new WaitForSeconds(1f);

        }
    }

    private void SetNewPosition(ref Queue<Transform> row )
    {
        Transform cache = row.Dequeue();

        cache.position += direction * lastZIndex * xDistance;

        row.Enqueue(cache);
    }

}