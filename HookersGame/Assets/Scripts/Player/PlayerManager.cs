using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum Stage {City,Tunnel }
public class PlayerManager : MonoSingleton<PlayerManager>
{
    public enum InputInfluenceState { QTE , Beat}
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
    bool inputEnabled=true;
    

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
       // Debug.Log(SoundManager.IsByBeat);

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
              
                if (inputEnabled)
                {
                  ApplyInputs();
                    
                }
                break;
        }
        #region hackInputs

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetCurrentStage(Stage.Tunnel);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetCurrentStage(Stage.City);
        }
        #endregion


    }

    public void SetCurrentStage(Stage stage)
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
        SetFieldOfView();
    }
    private void GetStartPos()
    {
        StartPoint = LevelManager.Instance.GetStartPointTransform;
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



    public void DisableInput(InputInfluenceState state,float Duration)
    {
        StartCoroutine(DisableInputCoru(state, Duration));
    }

    private void FixedUpdate()
    {
        if (_inputForm != null&&!Influenced)
        {
            rb.AddRelativeForce(_inputForm.movementVector.normalized * movementSpeed);
        }
    }
    //will be transfered to another script!
    #region GFX
    public void SetFieldOfView()
    {
        float speed = rb.velocity.magnitude;
        //        if (_grapplingGun.pulling) { }
        if (_grapplingGun.pulling)//&& (_playerPhysicsManager.physicsState == PlayerPhysicsManager.PhysicsStates.leap))
        {
            _cameraController.FpsCam.fieldOfView -= playerEffectMenu.FieldPerView;
        }
        else
        {

            if (_cameraController.FpsCam.fieldOfView < playerEffectMenu.baseFieldOfView)
                _cameraController.FpsCam.fieldOfView += playerEffectMenu.FieldPerView*0.9f;
            if (_cameraController.FpsCam.fieldOfView > playerEffectMenu.baseFieldOfView)
                _cameraController.FpsCam.fieldOfView=Mathf.Lerp(_cameraController.FpsCam.fieldOfView, playerEffectMenu.baseFieldOfView, 0.5f);
        }

        
    }
    private void SpeedEffect()
    {
        float speed = rb.velocity.magnitude;
        if (speed > playerEffectMenu.SpeedPs_startParticlesSpeed)
        {

            ParticleSystem.EmissionModule em = _speedPs.emission;
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
            em.rateOverTime = (speed - playerEffectMenu.SpeedPs_startParticlesSpeed) / playerEffectMenu.SpeedPs_particleEmissionPerKmh;

        }
        else { if (!_speedPs.isStopped) _speedPs.Stop(); }
    }

    #endregion

    #region Physics

    internal void ApplyForceToPlayer(InfluenceSettings influenceSettings)
    {
        Influenced = true;
        StartCoroutine(PlayerForceInfluence(influenceSettings.playerInfluence, influenceSettings.InfluenceTime, influenceSettings.Force, influenceSettings.Dir));
    }
    IEnumerator PlayerForceInfluence(PlayerInfluenceType influenceType, float influenceTime, float force, Vector3 Dir)
    {
        float Timeloop = Time.time;

        rb.velocity = Vector3.zero;
        _playerPhysicsManager.PhysicsRequest(PlayerPhysicsManager.PhysicsStates.DefaultAir);
        switch (influenceType)
        {
            case PlayerInfluenceType.Impulse:
                while (Timeloop + influenceTime > Time.time)
                {
                    rb.AddForce(Dir * force, ForceMode.Impulse);
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
                    rb.AddExplosionForce(force, Dir, 10);
                    yield return new WaitForFixedUpdate();
                }
                break;
        }


        yield return null;
        Influenced = false;
    }
    #endregion
    private void GetComponents()
    {
        rb = GetComponent<Rigidbody>();
        _cameraController = GetComponentInChildren<CameraController>();
        _inputManager = GetComponent<InputManager>();
        _speedPs = playerEffectMenu.SpeedPS.GetComponent<ParticleSystem>();
     
    }
    #region Inputs
    public void ApplyInputs()
    {
        switch (currentStage)
        {

            case Stage.City:

                if (!_grapplingGun.grappled && _inputForm.cityInputs.grapple && _grapplingGun.frontConnected)
                {
                    ShootArm();
                }
                if (!_grapplingGun.frontConnected && !_inputForm.cityInputs.grapple && _grapplingGun._frontArm)
                {
                    ReleaseGrapple();
                }
                if (_inputForm.cityInputs.pulse)
                {
                    Debug.Log("Pulse CurrentlyDown");
                    //CurrentlyUnUsed
                    //UseCompressor();
                }
                if (_inputForm.cityInputs.pullGrapple && _grapplingGun.grappled)
                {
                    if (!_grapplingGun.pulling)
                    {
                        _grapplingGun.PullGrapple();
                    }
                   

                }
                if ((_inputForm.cityInputs.releasePullGrapple && _grapplingGun.grappled)||!_grapplingGun.grappled)
                {
                    if (_grapplingGun.pulling)
                        _grapplingGun.pulling = false;
                        
                   


                }


                break;


            case Stage.Tunnel:

                if (Input.GetKeyDown(KeyCode.I))
                {

                    StartCoroutine(DisableInputCoru(InputInfluenceState.QTE, 8));
                }

                if (_inputForm.tunnelInputs.Shoot)
                {
                    ShootArm();
                    StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));
                    return;
                }

                if (_inputForm.tunnelInputs.up)
                {
                    if (_inputForm.tunnelInputs.left)
                    {
                        StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));
                        TunnelMovement(Movement.UpLeft);
                        return;
                    }

                    if (_inputForm.tunnelInputs.right)
                    {
                        TunnelMovement(Movement.UpRight);
                        StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));

                        return;
                    }


                    TunnelMovement(Movement.Up);
                    StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));
                    return;

                }

                if (_inputForm.tunnelInputs.down)
                {
                    if (_inputForm.tunnelInputs.left)
                    {
                        TunnelMovement(Movement.DownLeft);
                        StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));
                        return;
                    }

                    if (_inputForm.tunnelInputs.right)
                    {
                        TunnelMovement(Movement.DownRight);
                        StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));

                        return;
                    }


                    TunnelMovement(Movement.Down);
                    StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));
                    return;

                }

                if (_inputForm.tunnelInputs.left)
                {

                    TunnelMovement(Movement.Left);
                    StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));
                    return;
                }

                if (_inputForm.tunnelInputs.right)
                {

                    TunnelMovement(Movement.Right);
                    StartCoroutine(DisableInputCoru(InputInfluenceState.Beat, 0));
                    return;
                }
                break;

        }

    }
    #endregion

    private IEnumerator DisableInputCoru(InputInfluenceState state, float stateDuration)
    {
        inputEnabled = false;
      // Debug.Log("CoruCheck!");
        switch (state)
        {
            case InputInfluenceState.QTE:
                yield return new WaitForSeconds(stateDuration);
                break;
            case InputInfluenceState.Beat:
               yield return new WaitUntil(() => SoundManager.IsValidInputByBeat == true);
                break;
            default:
                break;
        }
        inputEnabled = true;
    }
    #region Movement
    public void TunnelMovement(Movement movement)
    {
        Vector3 Dir = TunnelManager.Instance.MoveOnGrid(true,movement);
        LeanTween.move(gameObject, Dir, SoundManager.GetBeatAmountInSeconds());
    }


    #endregion

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
    #region Abilities
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
    #endregion


    #region Checks
    public bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1, GroundLayer);
    }
    public bool WallCheck()
    {
        return true;
    }

    #endregion


    #region Events
    private void OnDestroy()
    {
        LevelManager.ResetLevelParams -= ResetValues;

    }
    private void OnDisable()
    {
        LevelManager.ResetLevelParams -= ResetValues;

    }
    #endregion



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


    public float FieldOfViewStartSpeed;
    public float FieldPerView;
    public float baseFieldOfView=60;

}

