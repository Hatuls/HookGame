
public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] ISingletonsArr;
    private void Start()
    {

        ISingletonsArr = new ISingleton[7]
        {
            SoundManager.Instance,
            NoteSpawner.Instance,
            EnemyManager.Instance,
            PlayerManager.Instance,
            LevelManager.Instance,
            TunnelManager.Instance,
            BoxSpawnerManager.Instance
        };

        for (int i = 0; i < ISingletonsArr.Length; i++)
        {
            if (ISingletonsArr[i] != null)
                ISingletonsArr[i].Init();
        }
    }
}
