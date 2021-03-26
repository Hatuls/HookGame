using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrapplingGun : MonoBehaviour
{

    public GrappleSetting grappleSetting;


    internal bool pulling;
    internal bool grappled; 
    internal bool frontConnected=true;

   [SerializeField] Transform frontHandSlot;
   [SerializeField] GameObject frontArmObj;
   [SerializeField] GameObject backArmObj;

    private GameObject currentFrontArm;
    private FrontArm _frontArm;
    private BackArm _backArm;
    private Vector3 grapplingEndPoint;
    private LineRenderer _lineRenderer;

    

    SpringJoint grappleJoint;
    GameObject grappleObj;



    private void Start()
    {   
        InitNewFrontArm();
        GetComponents();
    }

    
    private void GetComponents()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _backArm = backArmObj.GetComponent<BackArm>();
    }
    public void ResetGun()
    {
        if (grappled)
            StopGrapple();

    }

    public void LaunchFrontArm()
    {
        frontConnected = false;
        if (_frontArm != null )
          _frontArm.Launch(this);
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
       StartCoroutine(DrawRopeCoru());
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
        StartCoroutine(GrowArmCoru());
    }


    IEnumerator PullCoru()
    {
        float currentSpeed = grappleSetting.startPullSpeed;
        while (pulling && currentSpeed < grappleSetting.maxPullSpeed && grappleJoint != null)
        {
            grappleJoint.maxDistance -= currentSpeed;
            grappleJoint.minDistance -= currentSpeed;
            yield return new WaitForFixedUpdate();
            currentSpeed += grappleSetting.pullIncrease;
        }
        while (pulling && grappleJoint != null)
        {
            if (grappleJoint.maxDistance > 0.01)
                grappleJoint.maxDistance -= currentSpeed;
            if (grappleJoint.maxDistance > 0.01)
                grappleJoint.minDistance -= currentSpeed;
            yield return new WaitForFixedUpdate();

        }


    }



    IEnumerator DrawRopeCoru()
    {
        _lineRenderer.enabled = true;
        while (grappled)
        {

            if (grappleObj != null)
            {


                _lineRenderer.SetPosition(0, _backArm._grappleSource.position);
                _lineRenderer.SetPosition(1, grapplingEndPoint);
            }
            yield return null;
        }
        _lineRenderer.enabled = false;

    }


    IEnumerator GrowArmCoru()
    {
        yield return new WaitForSeconds(grappleSetting.timeForArmGrow);
        currentFrontArm = Instantiate(frontArmObj, frontHandSlot);
        _frontArm = currentFrontArm.GetComponent<FrontArm>();
        frontConnected = true;
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
    public float startPullSpeed;
    public float maxPullSpeed;
    public float pullIncrease;
    public float timeForArmGrow;
    public LayerMask GraplingLayere;
}
