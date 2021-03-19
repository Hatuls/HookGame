
using UnityEngine;
public enum PlatFromType { Grabable, NotGrabable };
public class Platform : MonoBehaviour
{
    [SerializeField]
    private PlatFromType platFromType;
    public PlatFromType GetPlatFromType => platFromType;
    public void SubscribePlatform() 
     => PlatformManager.SetPlatformTexture += SetTexture;
    

    public void SetTexture() {

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
   

    public void HighLightMe() { }

    private void OnDestroy()
    => PlatformManager.SetPlatformTexture -= SetTexture;
    

    private void OnDisable()
    => PlatformManager.SetPlatformTexture -= SetTexture;
    
}

    