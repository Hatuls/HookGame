using UnityEngine.Events;
    using UnityEngine;

public enum ScenesName {MainMenuScene=0,Level1Scene=1,Level2Scene=2,Level3Scene=3, Level4Scene = 4, Level5Scene = 5 , Level6Scene = 6 };
[CreateAssetMenu(fileName ="SceneHandlerSO", menuName ="ScriptableObjects/Scenes/SceneHandlerSO")]
public  class SceneHandlerSO : ScriptableObject
{
    
    public static int CurrentLevel => UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    public static int HighestLevel;

    public static void LoadScene(ScenesName scenes)
    {
        int sceneIndex = (int)scenes;

        if (sceneIndex > HighestLevel)
            return;

        MouseShower(scenes == ScenesName.MainMenuScene);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);

    }


    public static bool LevelCompleted()
    {
        if (CurrentLevel >= HighestLevel)
        {
         HighestLevel++;
            return true;
        }
        return false;
    }


    public static void MouseShower(bool toUnLockAndRevealMouse)
    {
        if (toUnLockAndRevealMouse)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
