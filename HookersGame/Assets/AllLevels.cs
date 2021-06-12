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
    public void RaiseLevel() => maxLevelUnlocked++;

    public LevelSO GetCurrentLevel() => GetLevel(levelSelected);
}