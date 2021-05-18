using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
public class BoxSpawnerManager : MonoSingleton<BoxSpawnerManager>
{
    [Header("Spawning Side Boom Box :")]

    [SerializeField] Transform buildingHolder;
    [SerializeField] bool toDrawGizomos = true;

    [SerializeField] BoxSpawnersAndChecker[] _volumeBoxesHandler;



    public Transform GetContainer => buildingHolder;
    // byte indexCounter; int beatSteps;


    public override void Init()
    {
        if (_volumeBoxesHandler != null && _volumeBoxesHandler.Length > 0)
        {
            for (int i = 0; i < _volumeBoxesHandler.Length; i++)
            {
                _volumeBoxesHandler[i].InitParams();

                LevelManager.ResetLevelParams += _volumeBoxesHandler[i].ResetDistanceChecker;
            }
        }
    }
    public GameObject InstantiatePrefab(ref GameObject prefab)
    {
       return Instantiate(prefab).gameObject;
    }

    public void OnDisable()
    {
        if (_volumeBoxesHandler != null && _volumeBoxesHandler.Length > 0)
        {
            for (int i = 0; i < _volumeBoxesHandler.Length; i++)
                LevelManager.ResetLevelParams -= _volumeBoxesHandler[i].ResetDistanceChecker;
        }
    }


    public void OnDrawGizmos()
    {
        if (toDrawGizomos)
        {
            if (_volumeBoxesHandler != null && _volumeBoxesHandler.Length > 0)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < _volumeBoxesHandler.Length; i++)
                {
                    if (_volumeBoxesHandler[i] != null)
                    {

                    Gizmos.DrawCube(_volumeBoxesHandler[i].startingPos, Vector3.one);
                    }
                }
            }
        }
    }
}
