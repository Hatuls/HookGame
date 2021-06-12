
using UnityEngine;
 using System.Collections;
public enum PlatFromType { Grabable, NotGrabable ,DeathPlatform,TriggerPlatform,ForceInfluence,Distructable};
public class Platform : MonoBehaviour
{
    public bool IsTrigger() => isTrigger;
    public bool IsHookAble() => isHookable;

    [SerializeField] private bool isTrigger,isHookable,DeathPlatform,movingPlatform,ForceInfluence,Distructable;

    [Tooltip("Activate movingPlatforms for use")]
    [SerializeField] PlatformMovementSetting MovementSetting;
    [SerializeField] InfluenceSettings influenceSettings;
    private Coroutine movementCoru;
    private int posCounter=0, movementIdTween;




    Quaternion rotation; 
    Vector3 position;
    Vector3 scale;

    MeshRenderer _MR;

    [SerializeField] private PlatFromType platFromType;
    //private Collider col;
    private MeshCollider col;
    public PlatFromType GetPlatFromType => platFromType;


    private void Update()
    {
        if (movingPlatform&MovementSetting.Move)
        {
        ApplyMovement();  
        }
    }
    public void ApplyMovement()
    {
        if (MovementSetting.Rotate)
        {
            transform.Rotate(MovementSetting.rotation * MovementSetting.rotSpeed);
        }
        if (movementCoru == null)
        {

           
            if (posCounter >= MovementSetting.Stops.Length)
            {
                if (MovementSetting.Loop)
                {
                    movementCoru = StartCoroutine(PlatformMovement(position, MovementSetting.TimeBetweenStations));
                    posCounter = 0;
                    return;
                }
                else { MovementSetting.Move = false; return; }
                
            }
            movementCoru = StartCoroutine(PlatformMovement(MovementSetting.Stops[posCounter].position, MovementSetting.TimeBetweenStations));
            posCounter++;
        }
    }
    void BuildPlatform()
    {
        //foreach(Transform found in MovementSetting.Stops)
        //{
        //    found.transform.parent = null;
        //}
        col = GetComponent<MeshCollider>();
        if (isHookable)
        {
            gameObject.tag = "GrappleAble";

        }
        if (isTrigger)
        {
            col.isTrigger = true; 
        }
        else { col.isTrigger = false; }
        if (movingPlatform)
        {
            
        }
            
        
    }
    
    private void Start()
    {
        rotation = transform.rotation;
        BuildPlatform();
    }
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FrontArm")&&isHookable)
        {
            FrontArm frontArm = other.GetComponent<FrontArm>();


            if (frontArm!=null)
           frontArm.AttatchRequest(other.transform.position, this.gameObject);
            
            
            
        }
        if (other.gameObject.CompareTag("Player"))
        {
            
            if (ForceInfluence)
            {
                other.gameObject.GetComponent<PlayerManager>().ApplyForceToPlayer(influenceSettings);
            }
            if (Distructable)
            {
               gameObject.SetActive(false);
            }

            if (DeathPlatform)
            {
                LevelManager.Instance.ResetLevelValues();
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("yo");
            if (ForceInfluence)
            {

                collision.gameObject.GetComponent<PlayerManager>().ApplyForceToPlayer(influenceSettings);
            }
            if (Distructable)
            {
                gameObject.SetActive(false);
            }


            if (DeathPlatform)
            {
                LevelManager.Instance.ResetLevelValues();
            }

        }


    }
   
    IEnumerator PlatformMovement(Vector3 Target,float TimeToRTarget)
    {
        MoveTowards(Target, TimeToRTarget);
        yield return new WaitForSeconds(TimeToRTarget);
        movementCoru = null;
      
    }

    public void SubscribePlatform()
    {
        LevelManager.ResetPlatformEvent += SetTexture;
        LevelManager.ResetPlatformEvent += PlatfromReset;
    }
    
    public void SuckTowards(in Transform targetPos)
    {
        MovementSetting.Move = false;
        LeanTween.cancel(movementIdTween);
         LeanTween.move(gameObject, targetPos, 2f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.rotateAround(gameObject, ToolClass.GetDirection(), 360f, 2f);
         LeanTween.scale(gameObject, Vector3.zero, 2f);
    }
    public void MoveTowards(in Vector3 targetPos,float TimeBetweenStations)
    {

        movementIdTween = LeanTween.move(gameObject, targetPos, TimeBetweenStations).setEase(LeanTweenType.easeInOutSine).uniqueId;
 
    }



    private void Awake()
    {
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    private void SetTexture()
    {

        if (_MR == null)
            _MR = GetComponentInParent<MeshRenderer>();
        switch (platFromType)
        {
            case PlatFromType.Grabable:
                //_MR.material.color = Color.green;
                // grabbable platform
                break;
            case PlatFromType.NotGrabable:
               // _MR.material.color = Color.white;
                // not grabbable platform
                break;
            case PlatFromType.DeathPlatform:
              //  _MR.material.color = Color.red;
                // not grabbable platform
                break;
            case PlatFromType.TriggerPlatform:
               // _MR.material.color = Color.cyan;
                // not grabbable platform
                break;
            case PlatFromType.ForceInfluence:
              //  _MR.material.color = Color.yellow;
                // not grabbable platform
                break;
            default:
                break;
        }
    }

    private void PlatfromReset()
    {
        StopAllCoroutines();
        LeanTween.cancelAll();

        if (movingPlatform)
        {
            posCounter = 0;
            MovementSetting.Move = true;
        }

        if (platFromType != PlatFromType.ForceInfluence)
        transform.rotation = rotation;


        transform.position = position;
        transform.localScale = scale;
        gameObject.SetActive(true);
    }

    private void UnSubscribeEvents()
    {
        LevelManager.ResetPlatformEvent -= SetTexture;
        LevelManager.ResetPlatformEvent -= PlatfromReset;
    }
    private void OnDestroy()
    {
        UnSubscribeEvents();
    }
}

[System.Serializable]
internal class InfluenceSettings
{
    public PlayerManager.PlayerInfluenceType playerInfluence;
    public Vector3 Dir;
    public float Force;
    public float InfluenceTime;

}

[System.Serializable]
public class PlatformMovementSetting
{

    public bool Loop;
    public bool Move;
    public bool Rotate;
    public Transform[] Stops;
    public float TimeBetweenStations;
    public Vector3 rotation;
    public float rotSpeed;
    
}
