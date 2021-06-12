
using UnityEngine;

public class MainMenuUI : MonoBehaviour, IMenuHandler
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
