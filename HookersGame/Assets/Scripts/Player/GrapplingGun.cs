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
   [SerializeField] AudioSource audioSource;

    private GameObject currentFrontArm;
    internal FrontArm _frontArm;
    private BackArm _backArm;
    private Vector3 grapplingEndPoint;
    private LineRenderer _lineRenderer;

    

    SpringJoint grappleJoint;
    GameObject grappleObj;


    Coroutine DrawCoru;



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
        if (_frontArm != null)
        {
            audioSource.Play();
            _frontArm.Launch(this);
       StartCoroutine(DrawProceduralRope(_frontArm.gameObject));
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
         grapplingEndPoint = GrapplePos-ConnectedObject.transform.position;
        grappleJoint = transform.root.gameObject.AddComponent<SpringJoint>();
        grappleJoint.autoConfigureConnectedAnchor = false;
        

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
        _lineRenderer.material = _lineRenderer.materials[1];
        while (pulling && currentSpeed < grappleSetting.maxPullSpeed && grappleJoint != null)
        {
            
            grappleJoint.maxDistance -= currentSpeed;
            grappleJoint.minDistance -= currentSpeed;
         //   Debug.Log(grappleJoint.maxDistance);
            yield return new WaitForFixedUpdate();
            currentSpeed += grappleSetting.PullMultiplyer;
        }
        while (pulling && grappleJoint != null)
        {
       
         //   Debug.Log(grappleJoint.maxDistance);
            if (grappleJoint.maxDistance > 0.01)
                grappleJoint.maxDistance -= currentSpeed;
            if (grappleJoint.maxDistance > 0.01)
               grappleJoint.minDistance -= currentSpeed;

            yield return new WaitForFixedUpdate();

        }
        _lineRenderer.material = _lineRenderer.materials[0];
        

        

    }



    IEnumerator DrawRopeCoru()
    {
        _lineRenderer.enabled = true;
        while (grappled&&_frontArm!=null)
        {
            
            transform.LookAt(_frontArm.transform.position);
            //_backArm.transform.Rotate(90, 0, 0);
            
            if (grappleObj != null)
            {
                
                grappleJoint.connectedAnchor =grappleObj.transform.position+grapplingEndPoint;
                _lineRenderer.SetPosition(0, _backArm._grappleSource.position);
                _lineRenderer.SetPosition(1, grappleObj.transform.position + grapplingEndPoint);
            }
            yield return null;
        }
        _lineRenderer.material = _lineRenderer.materials[0];
        _lineRenderer.enabled = false;
    //    _backArm.transform.rotation= _backArm.startRot;

    }

    IEnumerator DrawProceduralRope(GameObject Referencedhand)
    {
        _lineRenderer.enabled = true;
        while (!grappled&&Referencedhand!=null)
        {


                _lineRenderer.SetPosition(0, _backArm._grappleSource.position);
                _lineRenderer.SetPosition(1, Referencedhand.transform.position + grapplingEndPoint);


            yield return null;
        }
        if (!grappled)
        {

        _lineRenderer.enabled = false;
        }

    }



    IEnumerator GrowArmCoru()
    {
        yield return new WaitForSeconds(grappleSetting.timeForArmGrow);
        if (currentFrontArm != null)
        {
            Destroy(currentFrontArm);
        }
        currentFrontArm = Instantiate(frontArmObj, frontHandSlot);
        _frontArm = currentFrontArm.GetComponent<FrontArm>();
        frontConnected = true;
    }


    private void OnDestroy()
    {

        if (_lineRenderer!= null)
        {

        _lineRenderer.enabled = false;
        }
        
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
    public float PullMultiplyer;
    public float timeForArmGrow;
    public LayerMask GraplingLayere;
}
