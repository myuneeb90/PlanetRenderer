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
    public int InScatteringPoints = 10;
    public int OpticalDepthPoints = 10;
    public float DensityFallOff = 12;    

    public Vector3 WaveLengths = new Vector3(700, 530, 440);
    public Color WaveLengthsColor;
    public float ScatteringStrength = 1;    

    [Range(0, 1)]
    public float AtmosphereScale = 1;


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
        float scatterR = Mathf.Pow(400 / (WaveLengths.x * 1000), 4) * ScatteringStrength;
        float scatterG = Mathf.Pow(400 / (WaveLengths.y * 1000), 4) * ScatteringStrength;
        float scatterB = Mathf.Pow(400 / (WaveLengths.z * 1000), 4) * ScatteringStrength;
        Vector3 scatteringCoefficients = new Vector3(scatterR, scatterG, scatterB);

        EffectMaterial.SetVector("CameraPos", SceneCamera.transform.position);
        EffectMaterial.SetMatrix("CameraToWorld", SceneCamera.cameraToWorldMatrix);
        EffectMaterial.SetMatrix("CameraInverseProjection", SceneCamera.projectionMatrix.inverse);   

        EffectMaterial.SetVector("PlanetCenter", Planet.position);
        EffectMaterial.SetFloat("PlanetRadius", PlanetRadius);
        EffectMaterial.SetFloat("AtmosphereRadius", AtmosphereRadius);
        EffectMaterial.SetVector("DirToSun", SunLight.transform.forward * -1);
        EffectMaterial.SetFloat("DensityFalloff", DensityFallOff);
        EffectMaterial.SetInt("NumOpticalDepthPoints", OpticalDepthPoints);
        EffectMaterial.SetInt("NumInScatteringPoints", InScatteringPoints);
        EffectMaterial.SetVector("ScatteringCoefficients", scatteringCoefficients);               

        Graphics.Blit(source, destination, EffectMaterial);
    }
}

