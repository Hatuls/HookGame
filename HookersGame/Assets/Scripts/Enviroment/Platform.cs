
using UnityEngine;
public enum PlatFromType { Grabable, NotGrabable };
public class Platform : MonoBehaviour
{

    Vector3 position;
    Vector3 scale;

    MeshRenderer _MR;

    [SerializeField] private PlatFromType platFromType;
    public PlatFromType GetPlatFromType => platFromType;


    public void SubscribePlatform()
    {
        PlatformManager.ResetPlatformEvent += SetTexture;
        PlatformManager.ResetPlatformEvent += PlatfromReset;
    }
    public void MoveToward(in Transform targetPos)
    {
        LeanTween.move(gameObject, targetPos, 2f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.rotateAround(gameObject, ToolClass.GetDirection(), 360f, 2f);
        LeanTween.scale(gameObject, Vector3.zero, 2f);
    }


    private void Awake()
    {
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    private void SetTexture()
    {

        if (_MR == null)
            _MR = GetComponent<MeshRenderer>();
        switch (platFromType)
        {
            case PlatFromType.Grabable:
                _MR.material.color = Color.green;
                // grabbable platform
                break;
            case PlatFromType.NotGrabable:
                _MR.material.color = Color.red;
                // not grabbable platform
                break;
            default:
                break;
        }
    }


    private void PlatfromReset()
    {
        transform.position = position;
        transform.localScale = scale;
    }

    private void UnSubscribeEvents()
    {
        PlatformManager.ResetPlatformEvent -= SetTexture;
        PlatformManager.ResetPlatformEvent -= PlatfromReset;
    }
    private void OnDestroy()
    {
        UnSubscribeEvents();
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
