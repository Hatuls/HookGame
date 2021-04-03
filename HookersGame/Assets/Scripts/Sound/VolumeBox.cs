using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeBox : MonoBehaviour
{
    [SerializeField] bool useBuffer;
    [SerializeField] int band;
    [SerializeField] float startScale, scaleMultiplier , maxScale;
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
