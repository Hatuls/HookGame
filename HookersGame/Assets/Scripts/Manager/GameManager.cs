public class GameManager : MonoSingleton<GameManager>
{
    [UnityEngine.SerializeField] int FrameRate = 70;
    ISingleton[] ISingletonsArr;
    private void Start()
    {

        ISingletonsArr = new ISingleton[7]
        {
            SoundManager.Instance,
            NoteSpawner.Instance,
            PlayerManager.Instance,
            LevelManager.Instance,
            TunnelManager.Instance,
            EnemyManager.Instance,
            BoxSpawnerManager.Instance
        };

        for (int i = 0; i < ISingletonsArr.Length; i++)
        {
            if (ISingletonsArr[i] != null)
                ISingletonsArr[i].Init();
        }
        UnityEngine.QualitySettings.vSyncCount = 0;
        UnityEngine.Application.targetFrameRate = FrameRate;
    }

    private void Update()
    {
        if (UnityEngine.Application.targetFrameRate != FrameRate)
        {
            UnityEngine.Application.targetFrameRate = FrameRate;
        }
    }
}
