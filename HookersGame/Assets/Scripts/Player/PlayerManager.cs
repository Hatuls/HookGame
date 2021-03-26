using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharState { Mounted, UnMounted }
public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] GameObject GrapplingGunObj;
    [SerializeField] GameObject CompressorObj;
    [SerializeField] float movementSpeed;
    [SerializeField] LayerMask GroundLayer;


    CameraController _cameraController;
    PlayerController _playerController;
    InputManager _inputManager;
    InputForm _inputForm;
    PlayerUI _playerUi;
    Transform StartPoint;
    GrapplingGun _grapplingGun;
    Compressor _compressor;
    ParticleSystem _speedPs;
    

    //[SerializeField] float jumpForce;
    //[SerializeField] float dashForce;
    //[SerializeField] float dashTime;
    

    [Header("Menus")]

    [SerializeField] PlayerPhysicsManager _playerPhysicsManager;
    public PlayerEffectMenu playerEffectMenu;
    // Start is called before the first frame update
    public override void Init()
    {
        GetStartPos();
        LevelManager.ResetLevelParams += ResetValues;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;    
        GetComponents();
        GetEquipment();
        _playerPhysicsManager.InitPhysics(_playerController.rb, this);
       
    }
    private void OnDestroy()
    {
        LevelManager.ResetLevelParams -= ResetValues;
        
    }
    private void OnDisable()
    {
        LevelManager.ResetLevelParams -= ResetValues;
        
    }

    void Update()
    {
        _inputForm = _inputManager.GetInput();
        UpdateUi();
        CameraCommands();
        ApplyInputs();
        _playerPhysicsManager.CaulculatePhysics(GroundCheck(),_grapplingGun.grappled,_inputForm.pulse,WallCheck());
    }

    private void UpdateUi()
    {
        SpeedEffect();
    }
    public void GetStartPos()
    {
        StartPoint = LevelManager.Instance.GetStartPointTransform;
    }

    private void SpeedEffect()
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
    }
    public void ResetValues()
    {
        _compressor.ResetCompressor();
        _playerUi.ResetUi();
        _grapplingGun.ResetGun();
        _playerController.rb.velocity = Vector3.zero;
        ResetPlayerBody();
      
    }
    private void ResetPlayerBody()
    {
       
        transform.rotation = StartPoint.rotation;
        transform.position = StartPoint.position;
    }
  
    private void FixedUpdate()
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
        //_playerController.Dashforce = dashForce;
        //_playerController.DashTime = dashTime;
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
        //if (_inputForm.dash)
        //{
        //    _playerController.Dash(_cameraController.transform.forward);
        //}
        if (!_grapplingGun.grappled&&_inputForm.grapple && _grapplingGun.frontConnected)
        {
            ShootArm();
        }
        if(_grapplingGun.grappled && _inputForm.releaseGrapple)
        {  
            ReleaseGrapple();
        }
        if (_inputForm.pulse)
        {
            UseCompressor();
        }
        if (_inputForm.pullGrapple && _grapplingGun.grappled)
        {
            if (!_grapplingGun.pulling)
            {
            _grapplingGun.PullGrapple();
            }

        }else if (_grapplingGun.grappled && _grapplingGun.pulling)
        {
            _grapplingGun.pulling = false;
        }
    }

    public void CameraCommands()
    {
        _cameraController.MoveCamera(_inputForm.mouseVector);
        _cameraController.GetLookPos(GrapplingGunObj,_grapplingGun.grappleSetting.GrapplingRange);
        _cameraController.GetLookPos(CompressorObj,_grapplingGun.grappleSetting.GrapplingRange);
    }
   

    
    public void GetEquipment()
    {
        _grapplingGun = GrapplingGunObj.GetComponent<GrapplingGun>();
        _compressor = CompressorObj.GetComponent<Compressor>();
    }

    public void ShootArm()
    {
            _grapplingGun.LaunchFrontArm();
    }

    public void ReleaseGrapple()
    {
        _grapplingGun.StopGrapple();
    }

    public void UseCompressor()
    {
        _compressor.Pulse();   
    }
   

    public bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1, GroundLayer);
    }  
    public bool WallCheck()
    {
        return true;
    }
    //Saved Comments We Might Use
    #region CommentedSaves


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


    //public void RecieveCharge(int ammount)
    //{
    //    _compressor.Charge(ammount);
    //}

    //IEnumerator GetDashBoost(float DashBoost, float boostTime)
    //{

    //    _playerController.Dashforce += DashBoost;
    //    yield return new WaitForSeconds(boostTime);
    //    _playerController.Dashforce -= DashBoost;

    //}


    //public void ApplyDashBoost(float DashBoost, float boostTime)
    //{
    //    StartCoroutine(GetDashBoost(DashBoost, boostTime));
    //}
    #endregion
}
[System.Serializable]
public class PlayerEffectMenu 
{
    public GameObject SpeedPS;
    public float SpeedPs_particleEmissionPerKmh;
    public float SpeedPs_particleSpeedperKmh;
    public float SpeedPs_startParticlesSpeed;

}

