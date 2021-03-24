using UnityEngine;

using System.Collections;

public class LevelManager : MonoSingleton<LevelManager> 
{
    [SerializeField] LevelSO[] LevelsSO;
    
    int currentLevel;
    public delegate void ClickAction();
    public static event ClickAction ResetLevelParams;


    
    public override void Init()
    {
        ResetLevelValues();
        currentLevel = 0;
    }
    internal float GetLevelDeathWallSpeed()
        => LevelsSO[currentLevel].DeathWallSpeed;

    public void LoadTheNextLevel() {
        PlayerManager.Instance.Win();
        StartCoroutine(WinningCountDown());

        // maybe show success
        //currentLevel++;
       // ResetLevelParams();
    }
    IEnumerator WinningCountDown() {
        Time.timeScale = 0.1f;

        yield return new WaitForSeconds(0.5f);
        ResetLevelValues();
    }
    private int GetCurrentLevel() => currentLevel;
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
        
        PlayerManager.Instance?.SetStartPoint(LevelsSO[GetCurrentLevel()].GetPlayerSpawningPoint);
        ResetLevelParams?.Invoke();
    }



}
