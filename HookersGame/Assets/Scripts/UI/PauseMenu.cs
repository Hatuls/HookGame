using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PauseMenu : MonoBehaviour, IMenuHandler
{
    [SerializeField] RectTransform[] btns;

   

    IEnumerator GetIntoMenu(System.Action afterAction = null)
    {

        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].gameObject.SetActive(true);


            yield return null;
        }

        afterAction?.Invoke();
    }
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

        gameObject.SetActive(true);     
      
        StartCoroutine(GetIntoMenu(
            () =>
            { 
                Time.timeScale = 0;
                 SceneHandlerSO.MouseShower(true);
            }
            ));
    }
    public void SettingsMenu()
    {
        gameObject.SetActive(false);
        // tunr on Settings Menu
    }




}
