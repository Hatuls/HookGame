using UnityEngine;
public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] ISingletonsArr;
    private void Awake()
    {
        Application.targetFrameRate = 200;
    }
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
    }
}
