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
    

    [SerializeField] float EXP;
    private float NextLevelExp;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float maxJumpForce;
    [SerializeField] float grabRange;
    [SerializeField] float dashForce;
    [SerializeField] float dashTime;
    [SerializeField] Vector3 DragVector;
    float jumpLoadsInSec=10;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask ShildLayer;
    [SerializeField] LayerMask InterractionLayer;

    GameObject grabbedObj;

    GameObject jumpableSurface;

    bool hooked;


    [SerializeField] GameObject heldTechGun;

   

    TechGun _heldTechGun;

    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponents();
        GetTechGun();
       
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
        
        if (!_heldTechGun.grappled&&_inputForm.grapple)
        {
            
            Grapple();

        }else if(_heldTechGun.grappled && _inputForm.grapple)
        {
           
            ReleaseGrapple();
        }

    


     
      


     
    }

    public void CameraCommands()
    {
        _cameraController.MoveCamera(_inputForm.mouseVector);
        _cameraController.GetLookPos(heldTechGun);
    }

    #region PlayerCommands

    public void ChangeDrag()
    {
        if (Grounded())
        {
            _playerController.rb.drag = DragVector.x;
        }
        if (_heldTechGun.grappled)
        {
            _playerController.rb.drag = DragVector.z;
        }
       
        else { _playerController.rb.drag = DragVector.y; }
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
    }
 
    public void Grapple()
    {
        if (Physics.Raycast(_cameraController.FpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out RaycastHit hit, _heldTechGun.grappleSetting.GrapplingRange
            , _heldTechGun.grappleSetting.GraplingLayere))
        {
            _heldTechGun.Grapple(hit.point, hit.transform.gameObject,hit.point);
        }
    }
    public void ReleaseGrapple()
    {
        _heldTechGun.StopGrapple();
    }


    #endregion



 
    public bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2, GroundLayer);
    }

    


    
}
