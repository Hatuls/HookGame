using UnityEngine;

public class WinAreaTrigger : MonoBehaviour
{

    [SerializeField] tutorialToggle _t;
    bool isFlag = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && !isFlag)
            Win();
    }
    void Win()
    {
        isFlag = true;
        UIManager.Instance.OpenWinMenu();
        _t?.gameObject.SetActive(false);
    }

}
