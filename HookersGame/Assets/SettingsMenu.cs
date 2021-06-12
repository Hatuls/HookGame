
using UnityEngine;

public class SettingsMenu : MonoBehaviour, IMenuHandler
{
    public void Init(ref UIPallett uIPallett)
    {
        gameObject.SetActive(true);
    }

    public void OnEnd()
    {
        gameObject.SetActive(false);
    }
}
