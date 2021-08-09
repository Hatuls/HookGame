using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public static WinMenu Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu()
    {
     
        gameObject.SetActive(true);
        Time.timeScale = 0;
        SceneHandlerSO.MouseShower(true);
    }

    public void ReturnToMainMenu()
    {
        SceneHandlerSO.LoadScene(ScenesName.MainMenuScene);
        Time.timeScale = 1;
    }
    public void MoveToNextMenu()
    {
        LevelManager.Instance.FinishLevel();
        Time.timeScale = 1;
    }

}