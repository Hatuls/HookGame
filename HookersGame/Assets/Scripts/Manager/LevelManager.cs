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
    [SerializeField] Transform buildingHolder;
    [SerializeField] bool toInvert;
    [SerializeField] Vector3 leftStartPos;
    [SerializeField] Vector3 spawningDirecton;
    VolumeBoxesSpawner _volumeBoxesHandler;
    int lastXIndex;
    byte indexCounter; 
    int beatSteps;
    System.Collections.Generic.Queue<Transform> leftRow , rightRow;
    public void InitVolumeBoxes()
    {
        beatSteps = 0;
        beatSteps = AmountOfBoxes / 8;
        if (AmountOfBoxes % 8 >= 1)
            beatSteps ++;
             indexCounter = 0;
        leftRow = new System.Collections.Generic.Queue<Transform>();
        rightRow = new System.Collections.Generic.Queue<Transform>();

        SpawnBox(ref leftRow);
        indexCounter = 0;
        SpawnBox(ref rightRow);

        SetBoxesPosition(ref leftRow, leftStartPos);
   
        SetBoxesPosition(ref rightRow, leftStartPos +(-Vector3.left *distanceBetweenLeftAndRight));

        _volumeBoxesHandler = new VolumeBoxesSpawner(PlayerManager.Instance.transform, ref rightRow, ref leftRow, ref distanceBetweenBoxes, ref distanceBetweenPlayerAndBoxes, ref lastXIndex, ref spawningDirecton);
    }
    void SetByBeat(ref VolumeBox Cache, ref int i) {

     
        Cache._beatSteps = beatSteps;


        if (i-1!=0 && i-1 % 8 == 0)        
            indexCounter++;

            Cache._onFullBeat = indexCounter;

            Cache._onBeatD8[0] = i-1 % 8;
    }
    private void SpawnBox(ref  System.Collections.Generic.Queue<Transform> row)
    {
     //   bool invert = true;
        distanceBetweenBoxes += prefab.transform.GetChild(0).localScale.x;
        
        for (int i = 1; i <= AmountOfBoxes; i++)
        {
                  Transform transformChache = Instantiate(prefab, buildingHolder).transform;
           
                  VolumeBox Cache = transformChache.GetComponent<VolumeBox>();

                 SetByBeat(ref Cache, ref i);
              
                  row.Enqueue(transformChache);
        } 
    }
    public void ResetDistanceChecker()
    {
        if (leftRow != null && leftRow.Count > 0)
        {
            SetBoxesPosition(ref leftRow, leftStartPos);
            SetBoxesPosition(ref rightRow, leftStartPos + (Vector3.left * distanceBetweenLeftAndRight));
        }

        if (_volumeBoxesHandler != null )
        {
            _volumeBoxesHandler.StopCoroutineCheck();
            _volumeBoxesHandler.StartCoroutineCheck();
        } 
    }
    private void SetBoxesPosition(ref System.Collections.Generic.Queue<Transform> rowCache,Vector3 startPos)
    {
        if (rowCache == null || rowCache.Count == 0)
            return;

        int counter =0;

        foreach (var tower in rowCache)
        {
    
                tower.position = startPos + (spawningDirecton * counter * distanceBetweenBoxes);
            //  + new Vector3(i * distanceBetweenLeftAndRight, 0, 1 * counter * distanceBetweenBoxes);
            counter++;
        }
    }
    #endregion
}
