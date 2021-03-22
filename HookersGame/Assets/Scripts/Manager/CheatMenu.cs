using UnityEngine;
public class CheatMenu : MonoSingleton<CheatMenu> {

    [SerializeField] bool CanLose;
    public bool GetCanLoseCondition => CanLose;

    [SerializeField] bool toSpawnDrones;
    public bool GetToLoseCondition => toSpawnDrones;


}