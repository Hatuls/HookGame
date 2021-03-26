using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    internal Rigidbody rb;
    
    private float dashTime;
    private float dashforce;
    public float Dashforce { get => dashforce; set => dashforce = value; }
    public float DashTime { get => dashTime; set => dashTime = value; }
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Move(Vector3 movementVector)
    {
        rb.AddRelativeForce(movementVector);       
    }
     
    //public void Jump(Vector3 jumpVector)
    //{
    //    rb.AddForce(jumpVector);
    //}

    //public void Dash(Vector3 Dir)
    //{
    //    StartCoroutine(DashLoop(Dir));
    //}
    //public IEnumerator DashLoop(Vector3 Dir)
    //{
    //    float time = Time.time;
    //    while (Time.time < (time + dashTime)) 
    //    {
    //        rb.AddForce(Dir * dashforce,ForceMode.Impulse);
    //        //rb.velocity = transform.forward * dashforce;
    //        yield return null;
    //    }
    
    //}
  
 
}
