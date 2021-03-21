using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum Cam { RideCam,FpsCam}
   
    internal Camera FpsCam;

    [SerializeField] float mouseSensetivity;
    [SerializeField] Vector2 upDownViewRange;
    Vector2 currentRotation;
    GameObject player;
    

    private void Start()
    {
        FpsCam = GetComponentInChildren<Camera>();
        player = this.gameObject;
    }
   
    public void MoveCamera(Vector2 Axis)
    {
        currentRotation += Axis*mouseSensetivity;
        FpsCam.transform.localRotation = Quaternion.Euler(Mathf.Clamp(currentRotation.y,upDownViewRange.x, upDownViewRange.y), transform.rotation.y,0);
        transform.root.rotation = Quaternion.Euler(0, currentRotation.x, 0);
        
    }
    public void GetLookPos(GameObject gameObject,float distance)
    {
        gameObject.transform.LookAt(FpsCam.transform.forward * 10000f);
    }



}
