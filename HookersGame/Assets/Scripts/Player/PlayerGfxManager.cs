using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Linq;

[System.Serializable]
public class PlayerGfxManager 
{

    [SerializeField] RenderVolumeComponents RenderVolumeComponents;
    [SerializeField] PlayerEffectMenu playerEffectMenu;

    /// <summary>
    /// Enter Value
    /// </summary>
    /// <param name=""></param>
    /// 

    public void Init()
    {
        RenderVolumeComponents.CatchRenderComponents();
    }
    public void ModifyVolumeByDistance(float Distance)
    {
        if (Distance < RenderVolumeComponents.ChromaticAberrationStartDis)
        {
           // float intensity = (float)RenderVolumeComponents.ChromaticAberration.intensity.value+(Distance*RenderVolumeComponents.ChromaticAberrationIntensityPerDist);
            float intensity = ((RenderVolumeComponents.ChromaticAberrationStartDis - Distance) * RenderVolumeComponents.ChromaticAberrationIntensityPerDist);
            SetChromaticAberation(intensity);
        }

        if (Distance < RenderVolumeComponents.VingnetteStartDis)
        {
            //float intensity = (float)RenderVolumeComponents.Vignette.intensity.value + (Distance * RenderVolumeComponents.VingnetteIntensityPerDis);
            float intensity = ((RenderVolumeComponents.VingnetteStartDis-Distance) * RenderVolumeComponents.VingnetteIntensityPerDis);
            SetVinniet(intensity);
        }

    }

   
    void SetChromaticAberation(float newValue)
    {

        //Debug.Log(newValue);
        if (newValue < RenderVolumeComponents.ChromaticAberrationMaxIntensityValue && newValue > RenderVolumeComponents.ChromaticAberrationMinIntensityValue)
        {
            RenderVolumeComponents.ChromaticAberration.intensity.Override(newValue); // = new ClampedFloatParameter(newValue, newValue, newValue,true);
           Debug.Log("Chromaticaberration changed to" + RenderVolumeComponents.ChromaticAberration.intensity.value);

        }
    }

    void SetVinniet(float newValue)
    {
        //Debug.Log(newValue);
        if (newValue < RenderVolumeComponents.VingnetteMaxIntensityValue && newValue > RenderVolumeComponents.VingnetteMinIntensityValue)
        {

         
            RenderVolumeComponents.Vignette.intensity.Override(newValue);// = new ClampedFloatParameter(newValue, newValue, newValue,true);
            Debug.Log("Vinniet changed to"+ RenderVolumeComponents.Vignette.intensity.value);
            

        }
    }

    public void SetFieldOfView(Camera cam, Rigidbody rb, bool isPooling, float distFromVoid)
    {

        if (distFromVoid < playerEffectMenu.inverseFovStartDist)
        {

          //  float fov2 =(playerEffectMenu.inverseFovStartDist-distFromVoid) * playerEffectMenu.
            float fov = (((playerEffectMenu.inverseFovStartDist - distFromVoid)/100) * (playerEffectMenu.minFov+distFromVoid));
            if(fov>playerEffectMenu.minFov &&fov<playerEffectMenu.baseFieldOfView)
            cam.fieldOfView = fov;
            return;

        }
        else if(cam.fieldOfView<playerEffectMenu.baseFieldOfView)
        {
            cam.fieldOfView = playerEffectMenu.baseFieldOfView;

        }

        float speed = rb.velocity.magnitude;
        //        if (_grapplingGun.pulling) { }
        if (isPooling)//&& (_playerPhysicsManager.physicsState == PlayerPhysicsManager.PhysicsStates.leap))
        {
            cam.fieldOfView -= playerEffectMenu.FieldPerView;
        }
        else
        {

            if (cam.fieldOfView < playerEffectMenu.baseFieldOfView)
                cam.fieldOfView += playerEffectMenu.FieldPerView * 0.9f;
            if (cam.fieldOfView > playerEffectMenu.baseFieldOfView)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, playerEffectMenu.baseFieldOfView, 0.5f);
        }
    }


    public void ExecuteSpeedEffect(Rigidbody rb)
    {
        ParticleSystem ps = playerEffectMenu.SpeedPS;
        
        float speed = rb.velocity.magnitude;
        if (speed > playerEffectMenu.SpeedPs_startParticlesSpeed)
        {

            ParticleSystem.EmissionModule em = ps.emission;
            ParticleSystem.VelocityOverLifetimeModule ep = ps.velocityOverLifetime;
            if (!ps.isPlaying)
            {
                ps.Play();
                em.enabled = false;
                em.enabled = true;
                ep.enabled = false;
                ep.enabled = true;
            }


            ep.speedModifier = (speed - playerEffectMenu.SpeedPs_startParticlesSpeed) / playerEffectMenu.SpeedPs_particleSpeedperKmh;
            em.rateOverTime = (speed - playerEffectMenu.SpeedPs_startParticlesSpeed) / playerEffectMenu.SpeedPs_particleEmissionPerKmh;

        }
        else { if (!ps.isStopped) ps.Stop(); }
    }


    




}
[System.Serializable]
public class PlayerEffectMenu
{
    public ParticleSystem SpeedPS;
    public float SpeedPs_particleEmissionPerKmh;
    public float SpeedPs_particleSpeedperKmh;
    public float SpeedPs_startParticlesSpeed;


    public float FieldOfViewStartSpeed;
    public float FieldPerView;
    public float baseFieldOfView = 60;
    public float inverseFovStartDist;
    public float inverseFieldPerView;
    public float minFov;


}

[System.Serializable]
public class RenderVolumeComponents
{
    public Volume renderVol;

    
    internal ChromaticAberration ChromaticAberration;
    internal Vignette Vignette;

    [Header("ChromaticAberration")]

    public float ChromaticAberrationStartDis;
    [Range(0,1)]
    public float ChromaticAberrationMinIntensityValue, ChromaticAberrationMaxIntensityValue;
    [Range(0,1)]
    public float ChromaticAberrationIntensityPerDist;


    [Header("Vingnette")]

    public float VingnetteStartDis;
    [Range(0,1)]
    public float VingnetteMinIntensityValue, VingnetteMaxIntensityValue;
    [Range(0,1)]
    public float VingnetteIntensityPerDis;

    public void CatchRenderComponents()
    {
        renderVol.profile.TryGet<ChromaticAberration>(out ChromaticAberration);
        renderVol.profile.TryGet<Vignette>(out Vignette);
    }

}
