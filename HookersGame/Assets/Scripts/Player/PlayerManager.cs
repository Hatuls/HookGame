using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum Stage {City,Tunnel }
public class PlayerManager : MonoSingleton<PlayerManager>
{

    internal enum PlayerInfluenceType {linear,Impulse,explosion}
    [SerializeField] Stage currentStage;

    internal Rigidbody rb;
    bool Influenced=false;

    [SerializeField] GameObject GrapplingGunObj;
    [SerializeField] GameObject CompressorObj;
    [SerializeField] float movementSpeed;
    [SerializeField] LayerMask GroundLayer;


    CameraController _cameraController;
    InputManager _inputManager;
    InputForm _inputForm;
    Transform StartPoint;
    GrapplingGun _grapplingGun;
    Compressor _compressor;
    ParticleSystem _speedPs;

    

    //waiting for rei's events
    bool onBit=true;
    

    [Header("Menus")]

    [SerializeField] PlayerPhysicsManager _playerPhysicsManager;
    public PlayerEffectMenu playerEffectMenu;
    // Start is called before the first frame update
    public override void Init()
    {
       
        LevelManager.ResetLevelParams += ResetValues;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;    
        GetComponents();
        GetEquipment();
        _playerPhysicsManager.InitPhysics(rb, this);
        _inputManager.SetStage(currentStage);
       
        
    }
   
   
    void Update()
    {
        _inputForm = _inputManager.GetInput();
        UpdateUi();
        CameraCommands();

        switch (currentStage)
        {
            case Stage.City:
            ApplyInputs();
        _playerPhysicsManager.CaulculatePhysics(GroundCheck(),_grapplingGun.grappled,_inputForm.cityInputs.pulse,WallCheck());

                break;

            case Stage.Tunnel:
                if (onBit)
                {
                  ApplyInputs();
                    
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetPlayerStage(Stage.Tunnel);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetPlayerStage(Stage.City);
        }

    }

    public void SetPlayerStage(Stage stage)
    {
        if (stage != currentStage)
        {
            currentStage = stage;
            _inputManager.SetStage(stage);

            switch (stage) 
            {
                case Stage.City:

                    rb.isKinematic = false;
                    rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    GetComponent<Collider>().isTrigger = false;
                    break;


                case Stage.Tunnel:
                    TunnelMovement(Movement.Stay);
                    GetComponent<Collider>().isTrigger = true;
                    rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    rb.isKinematic = true;
                    break;

            }



        }
       
        


    
    }

    public void UpdateUi()
    {
        SpeedEffect();
    }
    private void GetStartPos()
    {
        StartPoint = LevelManager.Instance.GetStartPointTransform;
    }

    private void SpeedEffect()
    {
        float speed = rb.velocity.magnitude;
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
        if(StartPoint!= LevelManager.Instance.GetStartPointTransform)
        {
            GetStartPos();
        }
        _compressor.ResetCompressor();
        _grapplingGun.ResetGun();
        rb.velocity = Vector3.zero;
        ResetPlayerBody();
      
    }
    private void ResetPlayerBody()
    {
       
        transform.rotation = StartPoint.rotation;
        transform.position = StartPoint.position;
    }


    private void FixedUpdate()
    {
        if (_inputForm != null&&!Influenced)
        {
            rb.AddRelativeForce(_inputForm.movementVector.normalized * movementSpeed);
        }
    }
    internal void ApplyForceToPlayer(InfluenceSettings influenceSettings)
    {
        Influenced = true;
        StartCoroutine(PlayerForceInfluence(influenceSettings.playerInfluence, influenceSettings.InfluenceTime, influenceSettings.Force, influenceSettings.Dir));
    }
    IEnumerator PlayerForceInfluence(PlayerInfluenceType influenceType,float influenceTime,float force,Vector3 Dir)
    {
        float Timeloop = Time.time;

        rb.velocity = Vector3.zero;
        switch (influenceType) 
        {
            case PlayerInfluenceType.Impulse:
                while (Timeloop + influenceTime > Time.time)
                {
                rb.AddForce(Dir * force,ForceMode.Impulse);
                    yield return new WaitForFixedUpdate();
                }
                break;

            case PlayerInfluenceType.linear:
                while (Timeloop + influenceTime > Time.time)
                {
                rb.AddForce(Dir * force, ForceMode.Acceleration);
                    yield return new WaitForFixedUpdate();
                }
                break;

            case PlayerInfluenceType.explosion:
                while (Timeloop + influenceTime > Time.time)
                {
                rb.AddExplosionForce(force, Dir, 5);
                    yield return new WaitForFixedUpdate();
                } 
                break;
        }


        yield return null;
        Influenced = false;
    }

  
    private void GetComponents()
    {
        rb = GetComponent<Rigidbody>();
        _cameraController = GetComponentInChildren<CameraController>();
        _inputManager = GetComponent<InputManager>();
        _speedPs = playerEffectMenu.SpeedPS.GetComponent<ParticleSystem>();
     
    } 
    
    public void ApplyInputs()
    {
        switch (currentStage) 
        {

            case Stage.City:

                if (!_grapplingGun.grappled && _inputForm.cityInputs.grapple && _grapplingGun.frontConnected)
                {
                    ShootArm();
                }
                if (_grapplingGun.grappled && _inputForm.cityInputs.releaseGrapple)
                {
                    ReleaseGrapple();
                }
                if (_inputForm.cityInputs.pulse)
                {
                    UseCompressor();
                }
                if (_inputForm.cityInputs.pullGrapple && _grapplingGun.grappled)
                {
                    if (!_grapplingGun.pulling)
                    {
                        _grapplingGun.PullGrapple();
                    }

                }
                else if (_grapplingGun.grappled && _grapplingGun.pulling)
                {
                    _grapplingGun.pulling = false;
                }

                break;


            case Stage.Tunnel:
               
                if (_inputForm.tunnelInputs.Shoot)
                {
                    ShootArm();
                    return;
                }

                if (_inputForm.tunnelInputs.up)
                {
                    if (_inputForm.tunnelInputs.left)
                    {
                        TunnelMovement(Movement.UpLeft);
                    return;
                    }

                    if (_inputForm.tunnelInputs.right)
                    {
                        TunnelMovement(Movement.UpRight);
                        
                    return;
                    }


                        TunnelMovement(Movement.Up);
                    return;

                }

                if (_inputForm.tunnelInputs.down)
                {
                    if (_inputForm.tunnelInputs.left)
                    {
                        TunnelMovement(Movement.DownLeft);
                        return;
                    }

                    if (_inputForm.tunnelInputs.right)
                    {
                        TunnelMovement(Movement.DownRight);

                        return;
                    }


                    TunnelMovement(Movement.Down);
                    return;

                }

                if (_inputForm.tunnelInputs.left)
                {

                    TunnelMovement(Movement.Left);
                    return;
                }

                if (_inputForm.tunnelInputs.right)
                {

                    TunnelMovement(Movement.Right);
                    return;
                }



                break;


        }

    }
    public void TunnelMovement(Movement movement)
    {
        Debug.Log("yo");
        Vector3 Dir= TunnelManager.Instance.MovePlayerOnGrid(movement);
        LeanTween.move(gameObject, Dir, 1);
    }
    private void CameraCommands()
    {
        _cameraController.MoveCamera(_inputForm.mouseVector);
        _cameraController.GetLookPos(GrapplingGunObj,_grapplingGun.grappleSetting.GrapplingRange);
        _cameraController.GetLookPos(CompressorObj,_grapplingGun.grappleSetting.GrapplingRange);
    }
   

    
    private void GetEquipment()
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

    private void OnDestroy()
    {
        LevelManager.ResetLevelParams -= ResetValues;

    }
    private void OnDisable()
    {
        LevelManager.ResetLevelParams -= ResetValues;

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

