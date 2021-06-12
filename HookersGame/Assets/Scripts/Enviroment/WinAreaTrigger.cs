using UnityEngine;

public class WinAreaTrigger : MonoBehaviour
{
    bool isFlag = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && !isFlag)
            Win();
    }
    void Win()
    {
        isFlag = true;
        SceneHandlerSO.LevelCompleted();
        SceneHandlerSO.LoadScene((ScenesName)SceneHandlerSO.CurrentLevel);
    }

}
