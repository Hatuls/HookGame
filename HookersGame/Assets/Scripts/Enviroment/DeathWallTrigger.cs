using UnityEngine;

public class DeathWallTrigger : MonoBehaviour {

    [SerializeField] DeathWall deathWall;

    bool flag;
    private void Start()
    {
        LevelManager.ResetLevelParams += ReseTrigger;
    }
    void ReseTrigger()
          => flag = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && !flag)
        {
            flag = true;
            Debug.Log("Death Wall Starting to Chase The Player");
            deathWall.Instance.SetStartDeathWall = true;
            PlatformManager.Instance.StartChecking();
        }
    }


    private void OnDestroy()
    {
        LevelManager.ResetLevelParams -= ReseTrigger;
    }
}