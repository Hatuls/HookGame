
using UnityEngine;
public enum PlatFromType { Grabable, NotGrabable };
public class Platform : MonoBehaviour
{ 
    
    Platform pt;
    [SerializeField]
    private PlatFromType platFromType;
    public PlatFromType GetPlatFromType => platFromType;
    public void SubscribePlatform()
    {
     PlatformManager.SetPlatformTexture += SetTexture;
        PlatformManager.ResetPlatformEvent += PlatfromReset;
    }
    Vector3 position;
    Vector3 scale;
    private void Awake()
    {
     
         position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
     void SetTexture() {
    

        var mat = GetComponent<MeshRenderer>().material;
        switch (platFromType)
        {
            case PlatFromType.Grabable:
                mat.color = Color.green;
                // grabbable platform
                break;
            case PlatFromType.NotGrabable:
                mat.color = Color.red;
                // not grabbable platform
                break;
            default:
                break;
        }
    }
    public void MoveToward(in Transform targetPos)

    {
        LeanTween.move(gameObject, targetPos, 2f).setEase(LeanTweenType.easeInOutSine);
         LeanTween.rotateAround(gameObject,ToolClass.GetDirection(), 360f, 2f);
        LeanTween.scale(gameObject, Vector3.zero, 2f);
    }
  
    void PlatfromReset() {
        transform.position = position;
        transform.localScale = scale;}
    public void HighLightMe() { }

    private void OnDestroy()
    {
        UnSubscribeEvents();
    }

    void UnSubscribeEvents() {
        PlatformManager.SetPlatformTexture -= SetTexture;
        PlatformManager.ResetPlatformEvent -= PlatfromReset;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
