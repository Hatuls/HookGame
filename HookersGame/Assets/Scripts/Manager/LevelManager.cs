using UnityEngine;
using System.Collections;

public partial class LevelManager : MonoSingleton<LevelManager>
{
    #region Level Handler
    [SerializeField] LevelSO[] LevelsSO;

    int currentLevel;

    Transform _playerStartPosition;

    Coroutine CheckPlatformVoidDistance;

    public delegate void LevelEvents();
    public static event LevelEvents ResetLevelParams;
    public static event LevelEvents ResetPlatformEvent;


    public Transform GetStartPointTransform
    {
        get {

            if (_playerStartPosition == null)
                _playerStartPosition = GameObject.FindGameObjectWithTag("StartPoint").GetComponent<Transform>();


            return _playerStartPosition;
        }
    }
    public override void Init()
    {
        ResetLevelValues();
        currentLevel = 0;
        InitVolumeBoxes();
        ResetLevelParams += ResetDistanceChecker;
    }
    internal float GetLevelDeathWallSpeed()
        => LevelsSO[currentLevel].deathWallSpeed;
    public void ResetLevelValues()
    {
        // Player:
        // Reset Player Position + rotation
        // Reset player Cooldowns
        // reset physics and forces


        // Level:
        // Reset DeathWall - V
        // Reset Timer For Level - X
        // Reset Enemy Spawner + Reset Enemy Params - X
        // Reset Destroyable Objects - X

        // UI 
        // Reset Ui Elements
        Time.timeScale = 1f;
        AssignLevelObject();
        StopVoidPlatformCoroutine();
        ResetLevelParams?.Invoke();
        ResetPlatformEvent?.Invoke();
    }

    private void AssignLevelObject()
    {
        if (platformsArr == null || platformsArr.Length == 0)
        {
            platformsArr = FindObjectsOfType<Platform>();

            for (int i = 0; i < platformsArr.Length; i++)
                platformsArr[i].SubscribePlatform();
        }

        if (deathWall == null)
            deathWall = FindObjectOfType<DeathWall>();
    }
    private void StopVoidPlatformCoroutine()
    {
        flag = false;
        if (CheckPlatformVoidDistance != null)
            StopCoroutine(CheckPlatformVoidDistance);

    }
    public void LoadTheNextLevel()
    {
        deathWall = null;
        platformsArr = null;
        _playerStartPosition = null;


        StopVoidPlatformCoroutine();

        StartCoroutine(WinningCountDown());

        // maybe show success
        //currentLevel++; 

        // ResetLevelParams();
    }
    IEnumerator WinningCountDown()
    {
        Time.timeScale = 0.1f;

        yield return new WaitForSeconds(0.5f);
        ResetLevelValues();
    }
    #endregion



    #region Boom Boxes on the side

    [Header("Spawning Side Boom Box :")]
    [SerializeField] int AmountOfBoxes;
    [SerializeField] float distanceBetweenBoxes;
    [SerializeField] float distanceBetweenLeftAndRight;
    [SerializeField] float distanceBetweenPlayerAndBoxes;
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3 leftStartPos;
    [SerializeField] Transform buildingHolder;
    [SerializeField] bool toInvert;
    VolumeBoxesSpawner _volumeBoxesHandler;
    int lastXIndex;
    byte indexCounter; int beatSteps;
    System.Collections.Generic.Queue<Transform[]> boomBox;
    public void InitVolumeBoxes()
    {
        beatSteps = 0;
        beatSteps = AmountOfBoxes / 8;
        if (AmountOfBoxes % 8 >= 1)
            beatSteps ++;
             indexCounter = 0;
        boomBox = new System.Collections.Generic.Queue<Transform[]>();
        SpawnBox(ref boomBox);
        SetBoxesPosition(ref boomBox);

        _volumeBoxesHandler = new VolumeBoxesSpawner(PlayerManager.Instance.transform, ref boomBox, ref distanceBetweenBoxes, ref distanceBetweenPlayerAndBoxes, ref lastXIndex);
    }
    void SetByBeat(ref VolumeBox Cache, ref int _i, ref int isLeft) {
        int i = _i - 1;
        Cache._beatSteps = beatSteps;


        if (i!=0 && isLeft  ==0 &&i% 8 == 0)        
            indexCounter++;
        Cache._onFullBeat = indexCounter;

            Cache._onBeatD8[0] = i % 8;
    }
    private void SpawnBox(ref  System.Collections.Generic.Queue<Transform[]> boomBox)
    {
        bool invert = true;
        distanceBetweenBoxes += prefab.transform.GetChild(0).localScale.x;
        
        for (int i = 1; i <= AmountOfBoxes; i++)
        {
            Transform[] transformChache = new Transform[2];

            for (int x = 0; x < 2; x++)
            {
                var leftBuilding = Instantiate(prefab, buildingHolder);
                
                transformChache[x] = leftBuilding.transform;
                VolumeBox Cache = leftBuilding.GetComponent<VolumeBox>();

                SetByBeat(ref Cache, ref i, ref x);

                if (toInvert == false)
                {
                   Cache.GetSetBand = i % 8;
                }

                else
                {
                    if (invert)
                    {
                        Cache.GetSetBand = i % 8;
                        if (i % 8 == 0)
                            invert = false;
                    }
                    else
                    {
                        Cache.GetSetBand = 8 - (i % 8);
                        if (8 - (i % 8) == 1)
                            invert = true;
                    }

                }
            }
            if (i == AmountOfBoxes - 1)
                lastXIndex = i;

            boomBox.Enqueue(transformChache);
        } 
    }
    public void ResetDistanceChecker()
    {
        if (boomBox != null && boomBox.Count > 0)
        SetBoxesPosition(ref boomBox);
        
        if (_volumeBoxesHandler != null )
        {
            _volumeBoxesHandler.StopCoroutineCheck();
            _volumeBoxesHandler.StartCoroutineCheck();
        }
    }
    private void SetBoxesPosition(ref System.Collections.Generic.Queue<Transform[]> cache)
    {
        int counter = 0;
        foreach (var item in cache)
        {
            for (int i = 0; i < item.Length; i++)
                item[i].position = leftStartPos + (-Vector3.left * counter * distanceBetweenBoxes);
            //    + new Vector3(i * distanceBetweenLeftAndRight, 0, 1 * counter * distanceBetweenBoxes);

            counter++;
        }
    }
    #endregion
}
