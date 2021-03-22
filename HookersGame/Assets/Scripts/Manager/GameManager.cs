
public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] Singletons;
    private void Start()
    {

        Singletons = new ISingleton[5]
        {
            EventManager.Instance,
            EnemyManager.Instance,
            PlatformManager.Instance,
            PlayerManager.Instance,
            LevelManager.Instance
        };

        for (int i = 0; i < Singletons.Length; i++)
        {
            Singletons[i].Init();
        }
    }
}
