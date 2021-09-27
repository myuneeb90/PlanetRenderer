using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AtmosphereScript : MonoBehaviour
{
    public Camera SceneCamera;
    public Light SunLight;
    public Transform Planet;
    public float PlanetRadius;
    public float AtmosphereRadius;
    public Material EffectMaterial;

    void Start()
    {
        if (null == EffectMaterial || null == EffectMaterial.shader || 
            !EffectMaterial.shader.isSupported)
        {
            enabled = false;
            return;
        }	        

        SceneCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        EffectMaterial.SetVector("CameraPos", SceneCamera.transform.position);
        EffectMaterial.SetMatrix("CameraToWorld", SceneCamera.cameraToWorldMatrix);
        EffectMaterial.SetMatrix("CameraInverseProjection", SceneCamera.projectionMatrix.inverse);   

        EffectMaterial.SetVector("PlanetCenter", Planet.position);
        EffectMaterial.SetFloat("PlanetRadius", PlanetRadius);
        EffectMaterial.SetFloat("AtmosphereRadius", AtmosphereRadius);
        EffectMaterial.SetVector("DirToSun", SunLight.transform.forward * -1);

        Graphics.Blit(source, destination, EffectMaterial);
    }
}

