using UnityEngine;
public class CheatMenu : MonoSingleton<CheatMenu> {

    [SerializeField] bool CanLose;
    [SerializeField] bool toSpawnDrones;

    public bool GetCanLoseCondition => CanLose;
    public bool GetToLoseCondition => toSpawnDrones;
}