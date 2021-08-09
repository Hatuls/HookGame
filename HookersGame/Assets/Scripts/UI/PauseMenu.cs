using UnityEngine;

public class PauseMenu : MonoBehaviour, IMenuHandler
{
    public void Init(ref UIPallett uIPallett)
    {
        gameObject.SetActive(true);
    }

    public void OnEnd()
    {
        gameObject.SetActive(false);
    }

    public void ExitGame()
        => Application.Quit();

    public void ReturnToMainMenu()
        => SceneHandlerSO.LoadScene(ScenesName.MainMenuScene);


    public void Resume()
    {
       SceneHandlerSO.MouseShower(false);
        gameObject.SetActive(false);
    }

    public void SettingsMenu()
    {
        gameObject.SetActive(false);
        // tunr on Settings Menu
    }
}
