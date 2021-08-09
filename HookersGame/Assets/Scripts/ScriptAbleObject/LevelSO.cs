using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Levels", order = 1)]
public class LevelSO : ScriptableObject
{

    public int level;
    [Header("Speed of the death wall: ")]
    public float deathWallSpeed;
    [Header("The Number Of Position is The Number Of Drones In Game")]
    public Vector3[] droneSpawnPosition;
    [Header("Time Score: Bronze, Silver, Gold")]
    public float[] timeToFinish = new float[3];



    public Sprite LevelImage;
    public float timeFinished;

}
