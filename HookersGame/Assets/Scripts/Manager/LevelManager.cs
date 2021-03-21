using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager> 
{
    [SerializeField] LevelSO[] LevelsSO;
    
    int currentLevel;
    public delegate void ClickAction();
    public static event ClickAction ResetLevelParams;

    
    public override void Init()
    {
        currentLevel = 1;
    }
    internal float GetLevelDeathWallSpeed()
        => LevelsSO[currentLevel].DeathWallSpeed;
   
    public static void ResetLevelValues()
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
        
        ResetLevelParams?.Invoke();

    }



}
