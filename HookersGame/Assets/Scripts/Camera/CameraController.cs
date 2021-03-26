using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{  
    internal Camera FpsCam;
    [SerializeField] float mouseSensetivity;
    [SerializeField] Vector2 upDownViewRange;
    Vector2 currentRotation;
    
    

    private void Start()
    {
        FpsCam = GetComponentInChildren<Camera>();
    }
   
    public void MoveCamera(Vector2 Axis)
    {
        currentRotation += Axis*mouseSensetivity;
        FpsCam.transform.localRotation = Quaternion.Euler(Mathf.Clamp(currentRotation.y,upDownViewRange.x, upDownViewRange.y), transform.rotation.y,0);
        transform.root.rotation = Quaternion.Euler(0, currentRotation.x, 0);
        
    }
    public void GetLookPos(GameObject gameObject,float distance)
    {
        var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hitPoint;

        //Vector3 lookAt = ray.direction * 500f + gameObject.transform.position;
        //gameObject.transform.LookAt(lookAt);


        if (Physics.Raycast(ray, out hitPoint, float.MaxValue))
        {
           gameObject.transform.LookAt(hitPoint.point);
        }
    }



}
