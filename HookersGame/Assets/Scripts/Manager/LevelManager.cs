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
    [SerializeField] Vector3 startingPos;
    [SerializeField] Vector3 directionOfTowersSpawner;
    [SerializeField] Transform buildingHolder;
    [SerializeField] bool toInvert;
    VolumeBoxesSpawner _volumeBoxesHandler;
    int lastXIndex;
    byte indexCounter; int beatSteps;
    System.Collections.Generic.Queue<Transform> leftLine, rightLine;
    public void InitVolumeBoxes()
    {
        beatSteps = 0;
        beatSteps = AmountOfBoxes / 8;

        if (AmountOfBoxes % 8 >= 1)
            beatSteps ++;


        distanceBetweenBoxes += prefab.transform.GetChild(0).localScale.x;

        leftLine = new System.Collections.Generic.Queue<Transform>();
        rightLine = new System.Collections.Generic.Queue<Transform>();

        SpawnBox(ref leftLine);
        SpawnBox(ref rightLine);

        SetBoxesPosition(ref leftLine, startingPos);
        SetBoxesPosition(ref rightLine, startingPos + (-Vector3.left *distanceBetweenLeftAndRight));

        _volumeBoxesHandler = new VolumeBoxesSpawner(PlayerManager.Instance.transform, ref leftLine, ref rightLine, ref directionOfTowersSpawner,  ref distanceBetweenBoxes, ref distanceBetweenPlayerAndBoxes, ref lastXIndex);
    }
    void SetByBeat(ref VolumeBox Cache, ref int _i ) {
        int i = _i - 1;
        Cache._beatSteps = beatSteps;


        if (i!=0 && i% 8 == 0)        
            indexCounter++;

        Cache._onFullBeat = indexCounter;

            Cache._onBeatD8[0] = i % 8;
    }
    private void SpawnBox(ref  System.Collections.Generic.Queue<Transform> line)
    {
        indexCounter = 0;
        Transform building;
        for (int i = 1; i <= AmountOfBoxes; i++)
        {

                 building = Instantiate(prefab, buildingHolder).transform;


                VolumeBox Cache = building.GetComponent<VolumeBox>();

                SetByBeat(ref Cache, ref i);

            
            if (i == AmountOfBoxes - 1)
                lastXIndex = i;

            line.Enqueue(building);
        } 
    }
    public void ResetDistanceChecker()
    {
        if (leftLine != null && leftLine.Count > 0)
        SetBoxesPosition(ref leftLine, startingPos);


        if (rightLine != null && rightLine.Count > 0)
        SetBoxesPosition(ref rightLine , startingPos + (-Vector3.left * distanceBetweenLeftAndRight));

        
        if (_volumeBoxesHandler != null )
        {
            _volumeBoxesHandler.StopCoroutineCheck();
            _volumeBoxesHandler.StartCoroutineCheck();
        }
    }
    private void SetBoxesPosition(ref System.Collections.Generic.Queue<Transform> cache, Vector3 startingPos)
    {
        int counter = 0;
        foreach (var item in cache)
        {

            item.position = startingPos + directionOfTowersSpawner * counter * distanceBetweenBoxes;
              //  + new Vector3(i * distanceBetweenLeftAndRight, 0, 1 * counter * distanceBetweenBoxes);

            counter++;
        }
    }
    #endregion
}
