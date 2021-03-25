using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontArm : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] float TravelSpeed;
    [SerializeField] float TravelDist;
    [SerializeField] float DestroyDistOffset;
    
    
    internal GrapplingGun LaunchBase;
    bool attached=false;
    private bool connected=false;

    

    public void Launch(GrapplingGun techGun)
    {
        connected = false;
        LaunchBase = techGun;
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        TravelTowards();
    }


    void TravelTowards()
    {
        transform.parent = null;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //transform.LookAt(LaunchBase.transform.forward);
        rb.AddForce(transform.forward * TravelSpeed);
        StartCoroutine(CheckRangeFromSource());
    }
    IEnumerator CheckRangeFromSource()
    {
        GetComponent<Collider>().enabled=true;
        while (TravelDist+ DestroyDistOffset > Vector3.Distance(transform.position, LaunchBase.transform.position)){
            yield return new WaitForFixedUpdate();
            if (attached)
            {

                break;
            }
        }
        if (!attached)
        {
            LaunchBase.InitNewFrontArm();
            Destroy(this);
        }

    }
    void AttachToSurface(Vector3 attachedPoint, GameObject attachedObj)
    {
        attached=true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.isKinematic=true;
        SetGrapple(attachedPoint,attachedObj);
    }
    public void SetGrapple(Vector3 attachedPoint, GameObject attachedObj)
    {
        LaunchBase.Grapple(attachedObj, attachedPoint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GrappleAble"))
        {
            AttachToSurface(transform.position,collision.gameObject);
        }else if (collision.gameObject.CompareTag("ChargeObject"))
        {
            AttachToSurface(transform.position, collision.gameObject);
            LaunchBase.RecieveCharge(collision.gameObject.GetComponent<CellCharger>().TakeCharge());
        }else
        {
            Destroy(gameObject);
            LaunchBase.InitNewFrontArm();
        }



    }
}
