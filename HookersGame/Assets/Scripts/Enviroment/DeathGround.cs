
using UnityEngine;

public class DeathGround : MonoBehaviour
{
    bool flag;
    private void Start()
    {
        LevelManager.ResetLevelParams += ResetGround;
    }
    void ResetGround()
        => flag = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && !flag)
        {
            flag = true;
            LevelManager.Instance.ResetLevelValues();
        }
    }
    private void OnDestroy()
    {
        LevelManager.ResetLevelParams -= ResetGround;
    }
}