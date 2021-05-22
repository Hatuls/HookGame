using System;
using UnityEngine;

public enum Movement { Stay,Up,Down,Left,Right,UpLeft, UpRight, DownLeft,DownRight};
public class TunnelManager : MonoSingleton<TunnelManager>
{
    [Header("Grid Settings")]
    [SerializeField] GameObject[] tunnelGridPoint;
    [SerializeField] bool hideGridGizmoInScene;

  

    [SerializeField] float distanceBetweenPoints;
    [SerializeField] float distancebetweenDroneAndPlayer;
    [SerializeField] TunnelEntranceTrigger TunnelCollider;

  
 
    int[] directionGridIndex;
    static int[] playerPos = new int[3] {0, 1, 1 };
    static int[] enemyPos = new int[3] {1, 1, 1 };
    Vector3[,,] tunnelPoint = new Vector3[2, 3, 3];
    // 0 -> Player
    // 1 -> Drone
    public override void Init()
    {
        LevelManager.ResetLevelParams += ResetTunnel;
        InitGridPosition();
    }

    internal void EnterTunnelStage()
    {
        PlayerManager.Instance.SetCurrentStage(Stage.Tunnel);
    }
    internal void ExitTunnelStage()
    {
        PlayerManager.Instance.SetCurrentStage(Stage.City);
    }


    void InitGridPosition()
    {
        tunnelGridPoint = null;
        if (tunnelGridPoint == null)
            tunnelGridPoint = GameObject.FindGameObjectsWithTag("TunnelGridPoint");


        int x = tunnelPoint.GetLength(1),
            y = tunnelPoint.GetLength(2),
            z = tunnelPoint.GetLength(0);

        for (int t = 0; t < tunnelGridPoint.Length; t++)
        {
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    for (int k = 0; k < z; k++)
                        tunnelPoint[k, i, j] = tunnelGridPoint[t].transform.position + new Vector3(i * distanceBetweenPoints, j * distanceBetweenPoints, k * distancebetweenDroneAndPlayer);
        }


        ResetTunnel();
    }

    void ResetTunnel() {
        for (int i = 1; i < playerPos.Length; i++)
        {
           playerPos[i] = i/i;
            enemyPos[i] = i/i;
        }
     
      
    }




    // ask ron if he prefer bool or vector
    public Vector3 MoveOnGrid(bool isPlayer,Movement direction)
    {
        int[] posOnGrid = isPlayer ? playerPos : enemyPos;
        if (direction != Movement.Stay)
        {
            int[] dir = MovementEnumToIntArr(direction);
            if (dir == null)
                return Vector3.zero;
            // dir[0] -> x
            // dir[1] -> y

            // if it out of index array
            if (((dir[0] + posOnGrid[1]) >= 0 && (dir[0] + posOnGrid[1]) < tunnelPoint.GetLength(2))
             && (dir[1] + posOnGrid[2]) >= 0 && (dir[1] + posOnGrid[2]) < tunnelPoint.GetLength(1))
            {
                posOnGrid[1] += dir[0];
                posOnGrid[2] += dir[1];
            }
        }

        return tunnelPoint[posOnGrid[0], posOnGrid[1], posOnGrid[2]];
    }
    public Movement GetRandomMovement()
    {
        int randomInt = UnityEngine.Random.Range(0,9);

        switch (randomInt)
        {
            
            case 1:
                return Movement.DownLeft;
            case 2:
                return Movement.DownRight;
            case 3:
                return Movement.Left;
            case 4:
                return Movement.Right;
            case 5:
                return Movement.Up;
            case 6:
                return Movement.Down;
            case 7:
                return Movement.UpLeft;
            case 8:
                return Movement.UpRight;
             case 0:
             default:   
                return Movement.Stay;
        }

    }

    int[] MovementEnumToIntArr(Movement direction)
    {
        if (directionGridIndex == null)
            directionGridIndex = new int[2];


        if (directionGridIndex == null)
            return null;

        switch (direction)
        {
            case Movement.Up:
                directionGridIndex[0] = 0; //x
                directionGridIndex[1] = 1; // y
                break;
            case Movement.Down:
                directionGridIndex[0] = 0; //x
                directionGridIndex[1] = -1;// y
                break;
            case Movement.Left:
                directionGridIndex[0] = -1;//x
                directionGridIndex[1] = 0; // y
                break;
            case Movement.Right:
                directionGridIndex[0] = 1; //x
                directionGridIndex[1] = 0; // y
                break;
            case Movement.UpLeft:
                directionGridIndex[0] = -1; //x
                directionGridIndex[1] = 1;// y
                break;
            case Movement.UpRight:
                directionGridIndex[0] = 1; //x
                directionGridIndex[1] = 1; // y
                break;
            case Movement.DownLeft:
                directionGridIndex[0] = -1; //x
                directionGridIndex[1] = -1; // y
                break;
            case Movement.DownRight:
                directionGridIndex[0] = 1; //x
                directionGridIndex[1] = -1;  // y
                break;
            case Movement.Stay:
            default:
                return playerPos;

        }
        return directionGridIndex;
    }
    private void OnDestroy()
    {
        UnSubscribeEvent();
    }
    private void OnApplicationQuit()
    {
        UnSubscribeEvent();
    }
    void UnSubscribeEvent() { 
        LevelManager.ResetLevelParams -= ResetTunnel;
    }
    private void OnDrawGizmos()
    {
        if (!hideGridGizmoInScene && (tunnelGridPoint != null || tunnelGridPoint.Length >0))
        {
        Gizmos.color = Color.red;
            for (int x = 0; x < tunnelGridPoint.Length; x++)
            {
                for (int i = 0; i < tunnelPoint.GetLength(1); i++)  // on the X axis
                    for (int j = 0; j < tunnelPoint.GetLength(2); j++) // on the Y Axis
                        for (int k = 0; k < tunnelPoint.GetLength(0); k++) // on the Z Axis
                        {
                            if (tunnelGridPoint[x] == null)
                                continue;

                            Gizmos.DrawWireSphere(tunnelGridPoint[x].transform.position + new Vector3(i * distanceBetweenPoints, j * distanceBetweenPoints, k * distancebetweenDroneAndPlayer), 1f);
                        }
            }
        }
    }
}
