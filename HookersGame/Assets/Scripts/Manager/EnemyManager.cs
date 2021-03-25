
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{

    [SerializeField] Vector2 MinPoint, MaxPoint;
    [SerializeField] float DistanceFromPlayer, RotationSpeed, DroneSpeed;
    public static float GetDistanceFromPlayer => Instance.DistanceFromPlayer;
    public static float GetRotationSpeed => Instance.RotationSpeed;
    public static float GetDroneSpeed => Instance.DroneSpeed;


    internal Vector2 GetRandomPosition()
    => new Vector2(UnityEngine.Random.Range(MinPoint.x, MaxPoint.x), UnityEngine.Random.Range(MinPoint.y, MaxPoint.y));
}
