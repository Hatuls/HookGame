
using UnityEngine;
public enum PlatFromType { Grabable, NotGrabable };
public class Platform : MonoBehaviour
{
    [SerializeField]
    PlatFromType platFromType;

    public void SetTexture() {

        var mat = GetComponent<MeshRenderer>().material;
        switch (platFromType)
        {
            case PlatFromType.Grabable:
                mat.color = Color.green;

                break;
            case PlatFromType.NotGrabable:
                mat.color = Color.red;

                break;
            default:
                break;
        }
    }
    public PlatFromType GetPlatFromType => platFromType;

    public void HighLightMe() { }
}

    