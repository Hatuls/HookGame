using UnityEngine;
using System.Collections;

public partial class LevelManager : MonoSingleton<LevelManager>
{
    #region Level Handler
    [SerializeField] AllLevels LevelsSO;

   public int currentLevel { get; set; }

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
        currentLevel = LevelsSO.CurrentLevelSelected;
       
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

        }
    }
    internal float GetLevelDeathWallSpeed()
         => LevelsSO.GetLevel(currentLevel).deathWallSpeed;
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

    
    #endregion
}
