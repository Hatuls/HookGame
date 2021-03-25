using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrapplingGun : MonoBehaviour
{

    public GrappleSetting grappleSetting;


    internal PlayerManager usePlayer;
    internal bool pulling;
    internal bool grappled; 
    internal bool frontConnected=true;

   [SerializeField] Transform frontHandSlot;
   [SerializeField] GameObject frontArm;
   [SerializeField] GameObject backArm;
    private GameObject currentFrontArm;
    private FrontArm _frontArm;
    private BackArm _backArm;
    private Vector3 grapplingEndPoint;
    private LineRenderer lineRenderer;

    [SerializeField] float startPullSpeed;
    [SerializeField] float MaxPullSpeed;
    [SerializeField] float PullIncrease;
    [SerializeField] float TimeForArmGrow;

    SpringJoint grappleJoint;
    GameObject grappleObj;
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
        
        _backArm = backArm.GetComponent<BackArm>();
    }

    public void LaunchFrontArm()
    {
        frontConnected = false;
        _frontArm.Launch(this);
    }

    IEnumerator PullCoru()
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
            
        
    }
    public void PullGrapple()
    {
        pulling = true;
        StartCoroutine(PullCoru());
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
        InitNewFrontArm();
    }
    public void InitNewFrontArm()
    {
        StartCoroutine(GrowArm());
    }
    IEnumerator GrowArm()
    {
        yield return new WaitForSeconds(TimeForArmGrow);
        currentFrontArm = Instantiate(frontArm, frontHandSlot);
        _frontArm = currentFrontArm.GetComponent<FrontArm>();
        frontConnected = true;
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
