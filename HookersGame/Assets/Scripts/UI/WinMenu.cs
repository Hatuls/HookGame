using UnityEngine;
using TMPro;

public class WinMenu : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI mark;



    public void OpenMenu()
    {

   
        gameObject.SetActive(true);
        time.text = string.Format("{0:0.00}", LevelManager.Instance.LevelTimer);
        //string.Concat((t / 60) + " : " + (Mathf.Round(LevelManager.Instance.LevelTimer % 60));
        mark.text = AssessmentHander.GetAssessment(LevelManager.Instance.LevelTimer);
        Time.timeScale = 0;
        SceneHandlerSO.MouseShower(true);
    }

 

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneHandlerSO.LoadScene(ScenesName.MainMenuScene);
        UIManager.Instance.ResetMenu();
    }
    public void MoveToNextMenu()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        UIManager.Instance.InnerContainer(false);
        LevelManager.Instance.FinishLevel();
    }

}


public static class AssessmentHander
{
    public static string GetAssessment(float time)
    {
        float t = time / 60;
        if (t < 3)
            return "It's a blast!";
        else if (t < 6)
            return "Impressive!";
        else if (t < 8)
            return "Well Done!";
        else if (t < 10)
            return "Okay...";
        else
            return "Rei Segev would do it better than you";


    }



}