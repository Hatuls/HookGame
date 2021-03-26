using UnityEngine;

using System.Collections;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] LevelSO[] LevelsSO;

    int currentLevel;


    public delegate void ClickAction();
    public static event ClickAction ResetLevelParams;


    Transform _playerStartPosition;
    public override void Init()
    {
        ResetLevelValues();
        currentLevel = 0;
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
        PlatformManager.Instance.ResetPlatforms();

        if (_playerStartPosition == null)
            _playerStartPosition = GameObject.FindGameObjectWithTag("StartPoint").GetComponent<Transform>();

        PlayerManager.Instance?.SetStartPoint(_playerStartPosition);
        ResetLevelParams?.Invoke();
    }
   
    public void LoadTheNextLevel()
    { 
        
        
        _playerStartPosition = null;
        PlayerManager.Instance.Win();
        PlatformManager.Instance.ResetPlatforms();
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

}
