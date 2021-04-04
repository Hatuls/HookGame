using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FogVFX : MonoBehaviour
{
    [SerializeField] Gradient FogGradiant;
    public enum VolumeShapes { Cube,Triangle}
    [SerializeField] VolumeShapes Shape;
    [SerializeField] float ParicleScale;
    [Range(0.01f, 1f)]
    [SerializeField] float Density;


    VisualEffect FogGraph;
    
    // Start is called before the first frame update
    void Start()
    {
        DetachFromVolume();
        SetCosmetics();
    }
    public void DetachFromVolume()
    {
        FogGraph = GetComponentInChildren<VisualEffect>();
        FogGraph.transform.parent = null;
        FogGraph.transform.localScale = Vector3.one;
    }
    void VolumeFitting()
    {
        FogGraph.transform.position = transform.position;
        FogGraph.SetFloat("ScaleBySize", ConfiguerVolumeSize());
        FogGraph.SetFloat("ParticleScale", ParicleScale);
        FogGraph.SetFloat("FogDensity", Density);
        Debug.Log(ConfiguerVolumeSize());

        
    }
    public void SetCosmetics()
    {
        FogGraph.SetGradient("FogGradient", FogGradiant);
    }
    float ConfiguerVolumeSize()
    {
        float result = 0;
        switch (Shape)
        {
            case VolumeShapes.Cube:
                result = transform.localScale.x * transform.localScale.z * transform.localScale.y;
                break;
        }

        return result;

    }

    // Update is called once per frame
    void Update()
    {
        VolumeFitting();
        SetCosmetics();
    }
}
