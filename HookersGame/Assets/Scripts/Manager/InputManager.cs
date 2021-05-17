using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    Vector3 movementVector;
    Vector2 mouseVector;
    Stage currentStage;

  
    public void SetStage(Stage stage)
    {
        currentStage = stage;

    }
    public InputForm GetInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        InputForm _inputForm = new InputForm();

        _inputForm.movementVector = new Vector3(horizontal, 0, vertical);
        _inputForm.mouseVector = new Vector2(mouseX, -mouseY);


        switch (currentStage) 
        {
            case Stage.City:
                _inputForm.cityInputs.grapple = Input.GetButton("Fire1");
                _inputForm.cityInputs.pullGrapple = Input.GetButtonDown("Fire2");
                _inputForm.cityInputs.releasePullGrapple = Input.GetButtonUp("Fire2");
               // _inputForm.cityInputs.releaseGrapple = Input.GetButtonUp("Fire1");
                _inputForm.cityInputs.pulse = Input.GetButtonDown("Fire3");
                break;


            case Stage.Tunnel:
               

                _inputForm.tunnelInputs.Shoot = Input.GetButtonDown("TunnelShoot");
                _inputForm.tunnelInputs.up = Input.GetButtonDown("TunnelUp");
                _inputForm.tunnelInputs.down = Input.GetButtonDown("TunnelDown");
                _inputForm.tunnelInputs.left = Input.GetButtonDown("TunnelLeft");
                _inputForm.tunnelInputs.right = Input.GetButtonDown("TunnelRight");
               
                break;

        }

        return _inputForm;
    }




}
public class InputForm
{
    public Vector3 movementVector;
    public Vector2 mouseVector;
    //CityInputs
    public class City { public bool releaseGrapple, dash, pulse, release, pullGrapple, grapple, releasePullGrapple; }
    public City cityInputs=new City();
    //TunnelInputs
     public class TunnelInputs { public bool Shoot, up, down, left, right; }
    public TunnelInputs tunnelInputs = new TunnelInputs();
    

}



