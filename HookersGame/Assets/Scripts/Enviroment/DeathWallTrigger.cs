using UnityEngine;

public class DeathWallTrigger : MonoBehaviour {

 

    bool flag;
    private void Start()
    {
        LevelManager.ResetLevelParams += ResetTrigger;
    }
   private void ResetTrigger()
          => flag = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && !flag)
        {
            flag = true;
            Debug.Log("Death Wall Starting to Chase The Player");
            LevelManager.Instance.GetDeathWall.SetStartDeathWall = true;
            LevelManager.Instance.StartChecking();
        }
    }


    private void OnDestroy()
    {
        LevelManager.ResetLevelParams -= ResetTrigger;
    }
}