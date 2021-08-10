using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AllLevels", menuName = "ScriptableObjects/Level/AllLevels")] 
public class AllLevels : ScriptableObject
{
  [SerializeField]  LevelSO[] _levels;
    [SerializeField] int maxLevelUnlocked = 0;
    [SerializeField] int levelSelected;
    public int CurrentLevelSelected { get => levelSelected; set => levelSelected = value; }
    public int GetMaxLevelUnlocked => maxLevelUnlocked;
    public LevelSO GetLevel(int index)
    {
        if (index < 0 || index >= _levels.Length)
            return null;
        return _levels[index];
    }

    internal void RegisterTime(float levelTimer)
    {
        LevelSO s = GetCurrentLevel();

        if (s.timeFinished <= 1f || s.timeFinished > levelTimer)
            GetCurrentLevel().timeFinished = levelTimer;



    }

    public void RaiseLevel() {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex-1 == maxLevelUnlocked)
        {
        maxLevelUnlocked++;
        }

        if (SceneHandlerSO.HighestLevel < maxLevelUnlocked)
        SceneHandlerSO.HighestLevel = maxLevelUnlocked-1;    
    }

    public LevelSO GetCurrentLevel() => GetLevel(levelSelected);
}