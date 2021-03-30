using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Levels", order = 1)]
public class LevelSO : ScriptableObject {
    public int Level;
    [Header("Speed of the death wall: ")]
    [SerializeField] Transform PlayerSpawningPoint;
    public Transform GetPlayerSpawningPoint => PlayerSpawningPoint;
    public float DeathWallSpeed;
    [Header("The Number Of Position is The Number Of Drones In Game")]
    public Vector3[] DroneSpawnPosition;
    [Header("Time Score: Bronze, Silver, Gold")]
    public float[] TimeToFinish = new float[3];





}