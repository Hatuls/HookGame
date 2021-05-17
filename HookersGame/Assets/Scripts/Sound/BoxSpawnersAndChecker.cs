using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[System.Serializable]
public class BoxSpawnersAndChecker
{
    static Coroutine distanceCheckerCorou;

    Transform playerTransform;
    Queue<Transform> line;
    int lastZIndex;



    [SerializeField] float distanceBetweenPlayerAndBoxes, distanceBetweenBoxes;
    [SerializeField] int AmountOfBoxes;
    [SerializeField] Vector3 direction;
    [SerializeField] GameObject prefab;
    public Vector3 startingPos;

    [SerializeField] bool toInvert, toBeAffectedByBeat;

    byte indexCounter; int beatSteps;


    public void InitParams()
    {

        InitVolumeBoxes();

        playerTransform = PlayerManager.Instance.transform;

        lastZIndex++;

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

            if (Vector3.Distance(playerTransform.position, item.position) > distanceBetweenPlayerAndBoxes)
                cache = item;
        }

        return cache;
    }
    private IEnumerator Checker()
    {
        Transform furthest;
        furthest = GetFurthestVolumeBoxTranform(ref line);


        while (true)
        {

            if (Vector3.Distance(furthest.position, playerTransform.position) < distanceBetweenPlayerAndBoxes)
            {

                SetNewPosition(ref line);
                furthest = GetFurthestVolumeBoxTranform(ref line);

            }

            yield return new WaitForSeconds(1f);

        }
    }

    private void SetNewPosition(ref Queue<Transform> line)
    {
        Transform cache;
        cache = line.Dequeue();
        cache.position += direction * lastZIndex * distanceBetweenBoxes;
        line.Enqueue(cache);
    }

    public void InitVolumeBoxes()
    {
        if (toBeAffectedByBeat)
        {
            beatSteps = 0;
            beatSteps = AmountOfBoxes / 8;

            if (AmountOfBoxes % 8 >= 1)
                beatSteps++;
        }


        distanceBetweenBoxes += prefab.transform.GetChild(0).localScale.x;


          line = new Queue<Transform>();


        SpawnBox(ref line);
        SetBoxesPosition(ref line, startingPos);



    }
    void SetByBeat(ref VolumeBox Cache, ref int _i)
    {
        int i = _i - 1;
        Cache._beatSteps = beatSteps;


        if (i != 0 && i % 8 == 0)
            indexCounter++;

        Cache._onFullBeat = indexCounter;

        Cache._onBeatD8[0] = i % 8;
    }
    private void SpawnBox(ref Queue<Transform> line)
    {
        indexCounter = 0;
        Transform building;
    
        for (int i = 1; i <= AmountOfBoxes; i++)
        {

             building = BoxSpawnerManager.Instance.InstantiatePrefab(ref prefab).transform;
            building.SetParent(BoxSpawnerManager.Instance.GetContainer);
            if (toBeAffectedByBeat)
            {
                VolumeBox Cache = building.GetComponent<VolumeBox>();

                SetByBeat(ref Cache, ref i);
            }

            if (i == AmountOfBoxes - 1)
                lastZIndex = i;

            line.Enqueue(building);
        }
    }
    public void ResetDistanceChecker()
    {

        SetBoxesPosition(ref line, startingPos);


        StopCoroutineCheck();
        StartCoroutineCheck();


    }
    private void SetBoxesPosition(ref Queue<Transform> cache, Vector3 startingPos)
    {
        int counter = 0;
        foreach (var item in cache)
        {
            item.position = startingPos + direction * counter * distanceBetweenBoxes;

            counter++;
        }
    }

    
}