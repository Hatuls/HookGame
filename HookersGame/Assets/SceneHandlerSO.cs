
    using UnityEngine;

public enum ScenesName {MainMenuScene=0,Level1Scene=1,Level2Scene=2,Level3Scene=3 };
[CreateAssetMenu(fileName ="SceneHandlerSO", menuName ="ScriptableObjects/Scenes/SceneHandlerSO")]
public  class SceneHandlerSO : ScriptableObject
{
    public static int CurrentLevel;
    public static int HighestLevel;
    public static void LoadScene(ScenesName scenes)
    {
        int sceneIndex = (int)scenes;
        if (sceneIndex > HighestLevel)
            return;

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);  
    }

    public static void LevelCompleted()
    {
        CurrentLevel++;
        HighestLevel++;
    }
}
