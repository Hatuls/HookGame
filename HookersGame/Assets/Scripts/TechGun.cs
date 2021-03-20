using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TechGun : MonoBehaviour
{

    [SerializeField] Transform Source;


    

    public Transform[] bulletSource;
    internal int currentAmmo;
    [SerializeField] int bulletsPerShot;






    private LineRenderer lineRenderer;
    SpringJoint grappleJoint;
    GameObject grappleObj;
    public GrappleSetting grappleSetting;

    internal bool grappled;

    Vector3 grapplingEndPoint;

    

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Grapple(Vector3 Target,GameObject ConnectedObject,Vector3 GrapplePos)
    {
        

        grappleObj = ConnectedObject;
         grapplingEndPoint = GrapplePos;
        grappleJoint = transform.root.gameObject.AddComponent<SpringJoint>();
        grappleJoint.autoConfigureConnectedAnchor = false;
        grappleJoint.connectedAnchor = grappleObj.transform.position;

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
            

            grappleJoint.connectedAnchor = grappleObj.transform.position;
        lineRenderer.SetPosition(0, Source.position);
        lineRenderer.SetPosition(1, grapplingEndPoint);
            yield return null;
        }
        lineRenderer.enabled = false;

    }
    public void StopGrapple()
    {
        Debug.Log("yo");
        Destroy(grappleJoint);
        grapplingEndPoint = Vector3.zero;
        grappled = false;
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
