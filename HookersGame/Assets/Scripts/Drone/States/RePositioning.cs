using System;
using UnityEngine;

public class RePositioning : StateAbst
{
   
    float minimumDistanceRayCheck = 15f;
    float currentTime;
    float timeTillRaycast =0.2f;
    float PositiveOrNegative;

    Transform closestObstacle;
   
    Vector3 hitPoint;
    Vector2 NewLocation; 

    Ray ray, zRay;
    RaycastHit hitInfo;


    public RePositioning(Enemy enmy) : base(enmy)
    {
   
    }
    public override void OnStateEnter()
    {
        currentTime = Time.time;
        NewLocation = Vector2.zero;
        closestObstacle = null;
    }
    public override Type Tick()
    {
        enemy.EveryTickCheck();



        // // get closest transform to the drone
        // if (closestObstacle == null)
        // {
        //     closestObstacle= PlatformManager.Instance.GetClosestObjectTransform(transform.position);
        //     return null;
        // }
        // //reset hitPoint
        // hitPoint = Vector3.zero;


        //// Check Z;

        // zRay = new Ray(_enemy.GetDroneBody.position, Vector3.forward);
        // Debug.DrawRay(zRay.origin, zRay.direction, Color.yellow);






        // // X and Y Avoidance



        // // check time to shoot ray toward closest object
        // // get the point of the hit
        // if (currentTime + timeTillRaycast < Time.time)
        // {
        //     currentTime = Time.time;

        //     if (Vector3.Distance(_enemy.GetDroneBody.position, closestObstacle.position) < minimumDistanceRayCheck)
        //     {
        //         ray = new Ray(_enemy.GetDroneBody.position, _enemy.GetNormalizedDirection(closestObstacle.position, _enemy.GetDroneBody.position));
        //         if (Physics.Raycast(ray, out hitInfo, 20f, PlatformManager.Instance.GetGrabableLayer))
        //         {
        //           Debug.DrawRay(_enemy.GetDroneBody.position, _enemy.GetNormalizedDirection(closestObstacle.position, _enemy.GetDroneBody.position), Color.red, 2f);
        //          hitPoint = hitInfo.point;
        //         } 
        //     }
        // }

        // // if nothing got hit  then there is no point to continue;
        // if (Vector3.zero == hitPoint)
        //     return null;



        // // we passes the object get the new closest object
        // if (hitInfo.point.z < _enemy.GetDroneBody.position.z)
        // {
        //     Debug.Log("Need To Change Transform");
        //     closestObstacle = null;
        //     return null;
        // }


        // // move from object if needed;
        // MoveOnXAxis(in hitPoint);
        // MoveOnYAxis(in hitPoint);



        enemy.EveryTickCheck();

        if (NewLocation == Vector2.zero)
        {
          //do
                NewLocation = EnemyManager.Instance.GetRandomPosition();
           //while (PlatformManager.Instance.CheckEnviroment(NewLocation));
        }



        if (NewLocation != Vector2.zero)
            transform.position = Vector3.Slerp(transform.position, new Vector3(NewLocation.x, NewLocation.y, transform.position.z), EnemyManager.GetDroneSpeed * Time.deltaTime);
        if (CheckIfDroneIsAtXYLocation())
        {
            return typeof(Wingle);
        }

        return null;
    }



    private void MoveOnYAxis(in Vector3 hitPoint)
    {
        if (Vector3.Distance(hitPoint, transform.position) < 1.5f)
        {
            PositiveOrNegative = transform.position.y >= hitPoint.y ? 1 : -1;
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * PositiveOrNegative , 0.2f);
        }
    }
    private void MoveOnXAxis(in Vector3 hitPoint)
    {
        if (Vector3.Distance(hitPoint, transform.position) < 1.5f)
        {
            PositiveOrNegative = transform.position.x >= hitPoint.x ? 1 : -1;

            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.left * PositiveOrNegative, 0.2f);
        }
    }

  
    bool CheckIfDroneIsAtXYLocation() {

        return Vector2.Distance(transform.position, NewLocation) < 0.1f;
    }
}
