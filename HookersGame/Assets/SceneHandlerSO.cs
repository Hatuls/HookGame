
    using UnityEngine;

public enum ScenesName {MainMenuScene=0,Level1Scene=1,Level2Scene=2,Level3Scene=3 }
[CreateAssetMenu(fileName ="SceneHandlerSO", menuName ="ScriptableObjects/Scenes/SceneHandlerSO")]
public  class SceneHandlerSO : ScriptableObject
{
    public static void LoadScene(ScenesName scenes)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)scenes);  
    }
}
