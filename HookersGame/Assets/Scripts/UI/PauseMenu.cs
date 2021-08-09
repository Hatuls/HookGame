using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PauseMenu : MonoBehaviour, IMenuHandler
{
    [SerializeField] RectTransform[] btns;

   

    IEnumerator GetOutMenu(System.Action afterAction = null)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].gameObject.SetActive(false);
            yield return null;
        }
        afterAction?.Invoke();
  
    }


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
    {

        Time.timeScale = 1;
        SceneHandlerSO.LoadScene(ScenesName.MainMenuScene);
        UIManager.Instance.ResetMenu();


    }


    public void Resume()
    {
      
        StartCoroutine(GetOutMenu(
            () => {
            SceneHandlerSO.MouseShower(false);
                Time.timeScale = 1;
            gameObject.SetActive(false);
        }
        ));
    }
    public void OpenPauseMenu()
    {

       this.gameObject.SetActive(true);

        for (int i = 0; i < btns.Length; i++)
        {

        btns[i].gameObject.SetActive(true);
        }
        Time.timeScale = 0;
                 SceneHandlerSO.MouseShower(true);
       
    }
    public void SettingsMenu()
    {
        gameObject.SetActive(false);
        // tunr on Settings Menu
    }
}
