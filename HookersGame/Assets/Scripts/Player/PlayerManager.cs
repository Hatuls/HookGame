using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharState { Mounted, UnMounted }
public class PlayerManager : MonoSingleton<PlayerManager>
{

    [SerializeField] PlayerPhysicsManager playerPhysicsManager;
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

    Transform StartPoint;

    public PlayerEffectMenu playerEffectMenu;

    [SerializeField] GameObject heldTechGun;
    TechGun _heldTechGun;

    [SerializeField] GameObject compressor;
    Compressor _compressor;

    
    ParticleSystem _speedPs;

    // Start is called before the first frame update
    public override void Init()
    {
        LevelManager.ResetLevelParams += ResetValues;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        GetComponents();
        GetTechGun();
        GetCompressor();
        playerPhysicsManager.InitPhysics(_playerController.rb, this);
       
    }
  
    void Update()
    {
        _inputForm = _inputManager.GetInput();
        UpdateUi();
        CameraCommands();
        ApplyInputs();
        playerPhysicsManager.CaulculatePhysics(Grounded(),_heldTechGun.grappled,_inputForm.pulse,wallCheck());
        
        
    }

   


    public void UpdateUi()
    {
        SpeedUi();
    }
    public void SpeedUi()
    {
        float speed = _playerController.rb.velocity.magnitude;
        if (speed > playerEffectMenu.SpeedPs_startParticlesSpeed)
        {
          
        ParticleSystem.EmissionModule em=_speedPs.emission;
        ParticleSystem.VelocityOverLifetimeModule ep = _speedPs.velocityOverLifetime;
            if (!_speedPs.isPlaying)
            {
                _speedPs.Play();
                em.enabled = false;
                em.enabled = true;
                ep.enabled = false;
                ep.enabled = true;
            }
            
     
    
            

        ep.speedModifier = (speed - playerEffectMenu.SpeedPs_startParticlesSpeed) / playerEffectMenu.SpeedPs_particleSpeedperKmh;
        em.rateOverTime = (speed-playerEffectMenu.SpeedPs_startParticlesSpeed) / playerEffectMenu.SpeedPs_particleEmissionPerKmh;

        



        }
        else { if(!_speedPs.isStopped)_speedPs.Stop(); }
        _playerUi.TriggerUi(Mathf.RoundToInt(speed));

    }
    public void ResetValues()
    {
        _compressor.ResetCompressor();
        _playerUi.ResetUi();
        _heldTechGun.ResetGun();
        _playerController.rb.velocity = Vector3.zero;
        ResetPlayerBody();
      
    }
    
    public void SetStartPoint(Transform Destination)
    {
        StartPoint = Destination;
        ResetPlayerBody();
    }
    public void ResetPlayerBody()
    {
       
        transform.rotation = StartPoint.rotation;
        transform.position = StartPoint.position;
    }
    public void UiEvent()
    {
        
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
        _playerUi = GetComponentInChildren<PlayerUI>();
        _speedPs = playerEffectMenu.SpeedPS.GetComponent<ParticleSystem>();
        _playerController.Dashforce = dashForce;
        _playerController.DashTime = dashTime;
    }
    public void Win()
    {
        _playerUi.Win();
    }
  
    
    public void ApplyInputs()
    {

        //if ((_inputForm.jump && (Grounded() || jumpableSurface!=null)) || (_inputForm.jump && Grounded() && jumpableSurface != null))
        //{
        //    _playerController.Jump(Vector3.up*jumpForce);   
        //}
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
    
    //public void ChangeDrag()
    //{
    //    if (Grounded())
    //    {
    //        _playerController.rb.drag = DragVector.x;
    //    }
    //    else { _playerController.rb.drag = DragVector.y; }
    //    if (_heldTechGun.grappled)
    //    {
    //        _playerController.rb.drag = DragVector.z;
    //    }
       
       
    //}
   
    
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
        return Physics.Raycast(transform.position, Vector3.down, 1, GroundLayer);
    }  
    public bool wallCheck()
    {
        return true;
    }
}
[System.Serializable]
public class PlayerEffectMenu 
{
    public GameObject SpeedPS;
    public float SpeedPs_particleEmissionPerKmh;
    public float SpeedPs_particleSpeedperKmh;
    public float SpeedPs_startParticlesSpeed;



}

