using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    Vector3 movementVector;
    Vector2 mouseVector;

    bool CalculatingDash;
    bool useDash;
    [SerializeField] float WaitForDash;

    internal bool grabMode=false;
   

    IEnumerator WaitLoop;
    public InputForm GetInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        if (Input.GetKeyDown(KeyCode.W)&&!CalculatingDash)
        {
            CalculatingDash = true;
            WaitLoop = WaitForKeyDown(KeyCode.W);
            StartCoroutine(WaitLoop);

        }
        

        InputForm _inputForm = new InputForm();


       
       
        _inputForm.movementVector = new Vector3(horizontal, 0, vertical);
        _inputForm.mouseVector = new Vector2(mouseX, -mouseY);
      

       

        
       
            _inputForm.grapple = Input.GetButtonDown("Fire1");
            _inputForm.pullGrapple = Input.GetButton("Fire2");
            _inputForm.ReleaseGrapple = Input.GetButtonDown("Fire1");
   
     
            _inputForm.pulse = Input.GetButtonDown("Fire3");
            
            _inputForm.jump = Input.GetButtonDown("Jump");

           

            _inputForm.dash = applyDash(useDash);
       
        




        return _inputForm; 
    }
    
    public bool applyDash(bool apply)
    {
        if (apply) useDash = false;

        return apply;
    }
  
    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        yield return new WaitForEndOfFrame();
        Invoke("ResetLoop", WaitForDash);
        while (!Input.GetKeyDown(keyCode))
        {
            yield return null;
        }
        useDash = true;
    }
    public void ResetLoop()
    {
        
        StopCoroutine(WaitLoop);
        CalculatingDash = false;
    }
    


}
public class InputForm
{
    public Vector3 movementVector;
    public Vector2 mouseVector;
    public float Scrollwheel;

    public bool jump, ReleaseGrapple, dash, pulse, release, pullGrapple, grapple;
}

