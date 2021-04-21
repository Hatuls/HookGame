using UnityEngine;

public class VolumeBox : MonoBehaviour
{
    [SerializeField] bool useBuffer;
    [SerializeField] int band;
    [SerializeField] float startScale, scaleMultiplier , maxScale;
public int GetSetBand { get => band;
    set
        {
            if (value <= 0)
                band = 0;
            else if (value >= 7)
                band = 7;
            else
                band = value;
           
        }
    
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
             ScaleY(),
            transform.localScale.z);
    }
    float ScaleY() {

     float y= ((useBuffer ? SoundManager._audioBandBuffer[band] : SoundManager._audioBand[band]) * maxScale) + startScale;
        if (y > maxScale && y != 0)
            y = maxScale;
        return y;
    }
}
