using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public enum InputMode { City,Tunnel}
    public InputMode inputMode=InputMode.City;
  
    Vector3 movementVector;
    Vector2 mouseVector;

    public InputForm GetInput()
    {
 

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");




        InputForm _inputForm = new InputForm();

        switch (inputMode) 
        {
            case InputMode.City:

                _inputForm.movementVector = new Vector3(horizontal, 0, vertical);
                _inputForm.mouseVector = new Vector2(mouseX, -mouseY);
                _inputForm.grapple = Input.GetButtonDown("Fire1");
                _inputForm.pullGrapple = Input.GetButton("Fire2");
                _inputForm.releaseGrapple = Input.GetButtonDown("Fire1");
                _inputForm.pulse = Input.GetButtonDown("Fire3");

                break;


            case InputMode.Tunnel:




                break;

                
        }








          






        return _inputForm;
    }





}
public class InputForm
{
    public Vector3 movementVector;
    public Vector2 mouseVector;
    public bool releaseGrapple, dash, pulse, release, pullGrapple, grapple;
}
