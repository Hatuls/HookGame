﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontArm : MonoBehaviour
{
    internal GrapplingGun LaunchBase; 

    Rigidbody rb;
    [SerializeField] float travelSpeed;
    [SerializeField] float travelDist;
    [SerializeField] float destroyDistOffset;
    
    
    bool attached=false;
    private bool connected=false;

    

    public void Launch(GrapplingGun techGun)
    {
        rb = gameObject.AddComponent<Rigidbody>();
        transform.parent = null;
        rb.useGravity = false;
        LaunchBase = techGun;
        connected = false;
        TravelTowards();
    }


    void TravelTowards()
    {
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //transform.LookAt(LaunchBase.transform.forward);
        rb.AddForce(transform.forward * travelSpeed);
        StartCoroutine(CheckRangeFromSource());
    }
    IEnumerator CheckRangeFromSource()
    {
        GetComponent<Collider>().enabled=true;
        while (travelDist+ destroyDistOffset > Vector3.Distance(transform.position, LaunchBase.transform.position)){
            yield return new WaitForFixedUpdate();
            if (attached)
            {

                break;
            }
        }
        if (!attached)
        {
            LaunchBase.InitNewFrontArm();
            Destroy(gameObject);
        }

    }
    void AttachToSurface(Vector3 attachedPoint, GameObject attachedObj)
    {
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.isKinematic=true;
        attached=true;
        SetGrapple(attachedPoint,attachedObj);

    }
     void SetGrapple(Vector3 attachedPoint, GameObject attachedObj)
    {
        LaunchBase.Grapple(attachedObj, attachedPoint);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!connected)
        {

        if (collision.gameObject.CompareTag("GrappleAble"))
        {
            AttachToSurface(transform.position,collision.gameObject);
        }else if (collision.gameObject.CompareTag("ChargeObject"))
        {
            AttachToSurface(transform.position, collision.gameObject);
            //LaunchBase.RecieveCharge(collision.gameObject.GetComponent<CellCharger>().TakeCharge());
        }else
        {
            Destroy(gameObject);
            LaunchBase.InitNewFrontArm();
        }
        }



    }
   
    public void AttatchRequest(Vector3 attachedPoint, GameObject attachedObj)
    {
        if (attached == false && connected == false)
        {
            AttachToSurface(attachedPoint, attachedObj);
        }
        else { Debug.Log("AttachRequestFailed"); }
    }
}
