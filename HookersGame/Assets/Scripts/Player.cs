using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharState { Mounted, UnMounted }
public class Player : MonoBehaviour
{
    CameraController _cameraController;
    PlayerController _playerController;

    InputManager _inputManager;
    InputForm _inputForm;
    PlayerUI _playerUi;
    
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float grabRange;
    [SerializeField] float dashForce;
    [SerializeField] float dashTime;
    [SerializeField] Vector3 DragVector; 
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask ShildLayer;
    [SerializeField] LayerMask InterractionLayer;

    GameObject grabbedObj;
    GameObject jumpableSurface;

    [SerializeField] GameObject heldTechGun;
    TechGun _heldTechGun;

    [SerializeField] GameObject compressor;
    Compressor _compressor;

    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponents();
        GetTechGun();
        GetCompressor();
       
    }
  
    void Update()
    {
        _inputForm = _inputManager.GetInput();
        CameraCommands();
        applyInputs();
        ChangeDrag();
    }
    public void FixedUpdate()
    {
        if (_inputForm != null)
        {
            _playerController.Move(_inputForm.movementVector.normalized * movementSpeed);
        }
    }

  
    public void GetComponents()
    {
        _cameraController = GetComponentInChildren<CameraController>();
        _playerController = GetComponent<PlayerController>();
        _inputManager = GetComponent<InputManager>();
        _playerUi = GetComponent<PlayerUI>();
        _playerController.Dashforce = dashForce;
        _playerController.DashTime = dashTime;
    }
  
    
    public void applyInputs()
    {

        if ((_inputForm.jump && (Grounded() || jumpableSurface!=null)) || (_inputForm.jump && Grounded() && jumpableSurface != null))
        {
            _playerController.Jump(Vector3.up*jumpForce);   
        }
        if (_inputForm.dash)
        {
            _playerController.Dash(_cameraController.transform.forward);
        }
        if (!_heldTechGun.grappled&&_inputForm.grapple && _heldTechGun.FrontConnected)
        {
            ShootArm();
        }
        if(_heldTechGun.grappled && _inputForm.ReleaseGrapple)
        {  
            ReleaseGrapple();
        }
        if (_inputForm.pulse)
        {
            UseCompressor();
        }
        if (_inputForm.pullGrapple && _heldTechGun.grappled)
        {
            if (!_heldTechGun.pulling)
            {
            _heldTechGun.PullGrapple();
            }

        }else if (_heldTechGun.grappled && _heldTechGun.pulling)
        {
            _heldTechGun.pulling = false;
        }
    }

    public void CameraCommands()
    {
        _cameraController.MoveCamera(_inputForm.mouseVector);
        _cameraController.GetLookPos(heldTechGun,_heldTechGun.grappleSetting.GrapplingRange);
        _cameraController.GetLookPos(compressor,_heldTechGun.grappleSetting.GrapplingRange);
    }

    #region PlayerCommands

    public void ChangeDrag()
    {
        if (Grounded())
        {
            _playerController.rb.drag = DragVector.x;
        }
        else { _playerController.rb.drag = DragVector.y; }
        if (_heldTechGun.grappled)
        {
            _playerController.rb.drag = DragVector.z;
        }
       
       
    }
    public void RecieveCharge(int ammount)
    {
        _compressor.Charge(ammount);
    }

    IEnumerator GetDashBoost(float DashBoost, float boostTime)
    {

        _playerController.Dashforce += DashBoost;
        yield return new WaitForSeconds(boostTime);
        _playerController.Dashforce -= DashBoost;

    }

 
    public void ApplyDashBoost(float DashBoost, float boostTime)
    {
        StartCoroutine(GetDashBoost(DashBoost, boostTime));
    }
    #endregion

   

    #region GunCommands
    //All Commands Related To Guns
    public void GetTechGun()
    {
        _heldTechGun = heldTechGun.GetComponent<TechGun>();
        _heldTechGun.usePlayer = this;
    }

    public void ShootArm()
    {
       
            _heldTechGun.LaunchFrontArm();
        
    }

    public void ReleaseGrapple()
    {
        _heldTechGun.StopGrapple();
    }
    public void GetCompressor()
    {
        _compressor = compressor.GetComponent<Compressor>();
    }
    public void UseCompressor()
    {
        _compressor.Pulse();   
    }


    
   #endregion

    public bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2, GroundLayer);
    }  
}
