
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public override void Init()
    {
        Debug.Log("Hello");
    }
    private void Start()
    {
        Debug.Log(GameManager.Instance);
    }
}
