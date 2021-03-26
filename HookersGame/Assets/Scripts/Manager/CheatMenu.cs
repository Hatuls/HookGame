using UnityEngine;
public class CheatMenu : MonoSingleton<CheatMenu> {

    [SerializeField] bool CanLose;
    [SerializeField] bool toSpawnDrones;
    [SerializeField] bool UnLimitedCompressor;
    public bool GetCanLoseCondition => CanLose;
    public bool GetToLoseCondition => toSpawnDrones;
    public bool GetUnLimitedCompressor => UnLimitedCompressor;
}