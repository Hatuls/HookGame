﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TechGun : MonoBehaviour
{




    internal PlayerManager usePlayer;
    public GrappleSetting grappleSetting;
    internal bool grappled; 
   [SerializeField] Transform FrontHandSlot;
   [SerializeField] GameObject FrontArm;
    private GameObject currentFrontArm;
   [SerializeField] GameObject BackArm;
    private FrontArm _frontArm;
    private BackArm _backArm;
    private Vector3 grapplingEndPoint;
    internal bool FrontConnected=true;
    private LineRenderer lineRenderer;
    GameObject grappleObj;
    SpringJoint grappleJoint;

    [SerializeField] float startPullSpeed;
    [SerializeField] float MaxPullSpeed;
    [SerializeField] float PullIncrease;
    [SerializeField] float TimeForArmGrow;

    internal bool pulling;
    private void Start()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        InitNewFrontArm();
        GetParts();
    }

    public void ResetGun()
    {
        if(grappled)
        StopGrapple();
        
    }
    public void GetParts()
    {
        
        _backArm = BackArm.GetComponent<BackArm>();
    }

    public void LaunchFrontArm()
    {
        FrontConnected = false;
        _frontArm.Launch(this);
    }

    IEnumerator Pull()
    {
        float currentSpeed = startPullSpeed;
        while (pulling && currentSpeed<MaxPullSpeed && grappleJoint != null)
        {
            grappleJoint.maxDistance -= currentSpeed;
            grappleJoint.minDistance -= currentSpeed;
            yield return new WaitForFixedUpdate();
            currentSpeed += PullIncrease;
        }
        while (pulling&&grappleJoint!=null)
        {
            if(grappleJoint.maxDistance>0.01)
            grappleJoint.maxDistance -= currentSpeed;
            if(grappleJoint.maxDistance>0.01)
            grappleJoint.minDistance -= currentSpeed;
            yield return new WaitForFixedUpdate();

        }
            Debug.Log(currentSpeed);
        
    }
    public void PullGrapple()
    {
        pulling = true;
        StartCoroutine(Pull());
    }
    

    public void Grapple(GameObject ConnectedObject,Vector3 GrapplePos)
    {
       

        grappleObj = ConnectedObject;
         grapplingEndPoint = GrapplePos;
        grappleJoint = transform.root.gameObject.AddComponent<SpringJoint>();
        grappleJoint.autoConfigureConnectedAnchor = false;
        grappleJoint.connectedAnchor = GrapplePos;

        float DistanceFromTarget = Vector3.Distance(transform.root.position, grappleObj.transform.position);
        //Grappling ranges
        grappleJoint.maxDistance = DistanceFromTarget * grappleSetting.maxGrappleDist;
        grappleJoint.minDistance = DistanceFromTarget * grappleSetting.minGrappleDist;

        //grappleFeel
        grappleJoint.spring = grappleSetting.spring;
        grappleJoint.damper = grappleSetting.damper;
        grappleJoint.massScale = grappleSetting.massScale;

        grappled = true;
       StartCoroutine(DrawRope());
    }
     IEnumerator DrawRope()
    {
        lineRenderer.enabled = true;
        while (grappled)
        {

            if (grappleObj != null)
            {

            
            lineRenderer.SetPosition(0, _backArm.GrappleSource.position);
            lineRenderer.SetPosition(1, grapplingEndPoint);
            }
            yield return null;
        }
        lineRenderer.enabled = false;

    }

    public void StopGrapple()
    {
        
        Destroy(grappleJoint);
        Destroy(currentFrontArm);
        grapplingEndPoint = Vector3.zero;
        grappled = false;
        Debug.Log("sd");
        InitNewFrontArm();
    }
    public void InitNewFrontArm()
    {
        StartCoroutine(NewArm());
    }
    IEnumerator NewArm()
    {
        yield return new WaitForSeconds(TimeForArmGrow);
        currentFrontArm = Instantiate(FrontArm, FrontHandSlot);
        _frontArm = currentFrontArm.GetComponent<FrontArm>();
        FrontConnected = true;
    }

    public void RecieveCharge(int ammount)
    {
        StopGrapple();
        usePlayer.RecieveCharge(ammount);
    }




}
[System.Serializable]
public class GrappleSetting
{
    public float maxGrappleDist;
    public float minGrappleDist;
    public float spring;
    public float damper;
    public float massScale;
    public float GrapplingRange;
    public LayerMask GraplingLayere;
}
