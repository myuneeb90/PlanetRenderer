using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CaptureCuboidHeightMapScript : MonoBehaviour
{
    public List<Camera> CameraContainer;  
    public bool CaptureCubemap = false;

    public Texture2D TopHM;
    public Texture2D BottomHM;
    public Texture2D RightHM;
    public Texture2D LeftHM;
    public Texture2D FrontHM;
    public Texture2D BackHM;

    public string TexturePath;

    public bool IsReady = false;

    void LateUpdate()
    {
        if(CaptureCubemap == true)
        {
            this.CaptureCubemapTextures();
            IsReady = true;
            CaptureCubemap = false;
        }
    }  

    void CaptureCubemapTextures()
    {
        // Deactivate all cameras
        for(int i = 0; i < CameraContainer.Count; i++)
        {
            CameraContainer[i].gameObject.SetActive(false);
        }

        // Capture face one by one
        for(int i = 0; i < CameraContainer.Count; i++)
        {
            CameraContainer[i].gameObject.SetActive(true);

            RenderTexture renderTexture = new RenderTexture(CameraContainer[i].pixelWidth, CameraContainer[i].pixelHeight, 24);
            renderTexture.filterMode = FilterMode.Point;
            renderTexture.format = RenderTextureFormat.ARGBFloat;

            CameraContainer[i].targetTexture = renderTexture;

            CameraContainer[i].Render();
            RenderTexture.active = renderTexture;

            Texture2D newTexture = new Texture2D(CameraContainer[i].pixelWidth, CameraContainer[i].pixelHeight, TextureFormat.RGBAFloat, false);
            newTexture.alphaIsTransparency = true;
            newTexture.filterMode = FilterMode.Point;
            newTexture.wrapMode = TextureWrapMode.Clamp;

            newTexture.ReadPixels(new Rect(0, 0, CameraContainer[i].pixelWidth, CameraContainer[i].pixelHeight), 0, 0);
            newTexture.Apply();  

            int w = newTexture.width;

            Texture2D finalTexture = new Texture2D(w, w, TextureFormat.RGBAFloat, false);

            for (int z = 0; z < w; z++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color fcol = Color.black;
                    fcol = newTexture.GetPixel(x, z);
                    fcol.a = 1.0f;

                    finalTexture.SetPixel(x, z, fcol);
                }
            }

            finalTexture.Apply();            

            if(i == 0)
            {
                TopHM = finalTexture;
            }          
            else
            if(i == 1)
            {
                BottomHM = finalTexture;
            }
            else
            if(i == 2)
            {
                RightHM = finalTexture;
            }
            else
            if(i == 3)
            {
                LeftHM = finalTexture;
            }
            else
            if(i == 4)
            {
                FrontHM = finalTexture;
            }
            else
            if(i == 5)
            {
                BackHM = finalTexture;
            }

            CameraContainer[i].targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            CameraContainer[i].gameObject.SetActive(false);             
        }

        this.SaveTextureToFile(TopHM, "TopHM");
        this.SaveTextureToFile(BottomHM, "BottomHM");
        this.SaveTextureToFile(RightHM, "RightHM");
        this.SaveTextureToFile(LeftHM, "LeftHM");
        this.SaveTextureToFile(FrontHM, "FrontHM");
        this.SaveTextureToFile(BackHM, "BackHM");

//        TopHM.EncodeToPNG
    }

    private void SaveTextureToFile(Texture2D tex, string texName)
    {
        byte[] texBytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + TexturePath + texName + ".png", texBytes);
    }
}
