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

  
    int currentTunnel = 0;
    int[] directionGridIndex;
    static int[] playerPos = new int[2] { 1, 1 };
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
        PlayerManager.Instance.SetPlayerStage(Stage.Tunnel);
    }
    internal void ExitTunnelStage()
    {
        PlayerManager.Instance.SetPlayerStage(Stage.City);
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

        directionGridIndex = new int[2];
        ResetTunnel();
    }

    void ResetTunnel() {

        currentTunnel = 0;
        playerPos[0] = 1;
        playerPos[1] = 1;
    }




    // ask ron if he prefer bool or vector
    public Vector3 MovePlayerOnGrid(Movement direction)
    {
        if (direction != Movement.Stay)
        {
            int[] dir = MovementEnumToIntArr(direction);

            // dir[0] -> x
            // dir[1] -> y

            // if it out of index array
            if (((dir[0] + playerPos[0]) >= 0 && (dir[0] + playerPos[0]) < tunnelPoint.GetLength(2))
             && (dir[1] + playerPos[1]) >= 0 && (dir[1] + playerPos[1]) < tunnelPoint.GetLength(1))
            {
                playerPos[0] += dir[0];
                playerPos[1] += dir[1];
            }
        }

        return tunnelPoint[currentTunnel, playerPos[0], playerPos[1]];
    }
    int[] MovementEnumToIntArr(Movement direction)
    {
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
