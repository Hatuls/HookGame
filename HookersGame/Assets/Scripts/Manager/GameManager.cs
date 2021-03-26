
public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] ISingletonsArr;
    private void Start()
    {

        ISingletonsArr = new ISingleton[4]
        {
            SoundManager.Instance,
            EnemyManager.Instance,
            PlayerManager.Instance,
            LevelManager.Instance
        };

        for (int i = 0; i < ISingletonsArr.Length; i++)
        {
            if (ISingletonsArr[i] != null)
                ISingletonsArr[i].Init();
        }
    }
}
