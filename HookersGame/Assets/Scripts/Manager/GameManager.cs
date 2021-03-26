
public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] ISingletonsArr;
    private void Start()
    {

        ISingletonsArr = new ISingleton[5]
        {
            SoundManager.Instance,
            EnemyManager.Instance,
            PlatformManager.Instance,
            LevelManager.Instance,
            PlayerManager.Instance
        };

        for (int i = 0; i < ISingletonsArr.Length; i++)
        {
            if (ISingletonsArr[i] != null)
                ISingletonsArr[i].Init();


        }
    }
}
