
public class GameManager : MonoSingleton<GameManager>
{
    IInit[] Singletons;
    private void Start()
    {

        Singletons = new IInit[2]
        {
         EnemyManager.Instance,
         PlatformManager.Instance
        };

        for (int i = 0; i < Singletons.Length; i++)
        {
            Singletons[i].Init();
        }
    }
}
