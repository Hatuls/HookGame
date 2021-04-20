using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class VolumeBoxesSpawner
{
    static Coroutine distanceCheckerCorou;
    float distanceToCheck, xDistance;
    
    Transform playerTransform;
    Queue<Transform[]> BoomBox;
    int lastZIndex;
    public VolumeBoxesSpawner(in Transform PlayerTransform, ref Queue<Transform[]> _BoomBox, ref float xDistance, ref float distanceChecker, ref int lastZIndex)
    {
        InitParams(PlayerTransform, ref _BoomBox, ref xDistance, ref distanceChecker, ref lastZIndex);
    }

    public void InitParams(in Transform PlayerTransform, ref Queue<Transform[]> _BoomBox, ref float xDistance, ref float distanceChecker, ref int lastZIndex)
    {
        playerTransform = PlayerTransform;
        this.xDistance = xDistance;
        BoomBox = _BoomBox;
        this.distanceToCheck = distanceChecker;
        this.lastZIndex = lastZIndex + 1;
        StartCoroutineCheck();
    }

    public void StartCoroutineCheck()
     => distanceCheckerCorou = LevelManager.Instance.StartCoroutine(Checker());

    public void StopCoroutineCheck() {
        if (distanceCheckerCorou != null)
            LevelManager.Instance.StopCoroutine(distanceCheckerCorou);
        
    }

    private Transform GetFurthestVolumeBoxTranform()
    {
        Transform cache = null;

        foreach (var item in BoomBox)
        {
            if (cache == null)
                cache = item[0];

            else if (Mathf.Abs(item[0].position.z - playerTransform.position.z) > distanceToCheck)
                cache = item[0];
        }

        return cache;
    }
    private IEnumerator Checker()
    {
        Transform furthestFromPlayer;
        furthestFromPlayer = GetFurthestVolumeBoxTranform();

        while (true)
        {

            if (Mathf.Abs(furthestFromPlayer.position.z - playerTransform.position.z) < distanceToCheck)
            {
               
                SetNewPosition();
                furthestFromPlayer = GetFurthestVolumeBoxTranform();
            }

            yield return new WaitForSeconds(1f);

        }
    }

    private void SetNewPosition()
    {
        Transform[] cache;
        cache = BoomBox.Dequeue();

        for (int i = 0; i < 2; i++)
            cache[i].position += Vector3.forward * lastZIndex * xDistance;

        BoomBox.Enqueue(cache);
    }

}