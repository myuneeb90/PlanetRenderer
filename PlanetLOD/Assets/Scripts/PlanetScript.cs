using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PlanetScript : MonoBehaviour
{
    public int LODDepth;
    [HideInInspector]
    public float Size = 2.0f;
    public int Divisions;
    public int GridPoolCount;
    public float Radius;

    public Camera SceneCamera;

    public Material Material;


    public GridFaceScript TopFace;
    public GridFaceScript BottomFace;
    public GridFaceScript RightFace;
    public GridFaceScript LeftFace;
    public GridFaceScript FrontFace;
    public GridFaceScript BackFace;

    public GridPoolScript GridPool;
    private Thread ProcessThread = null;
    private bool IsProcessDone;
    private Vector3 CameraPosition;
    private bool IsCameraUpdated = false;
    private int ProcessFrameCountOffset;

    public CaptureCuboidHeightMapScript CaptureCuboidHeightMap;
    public BrushSpawnerScript BrushSpawner;
    private CuboidHeightMapScript CuboidHM;

    private bool IsReady = false;


    void Start()
    {
        if(BrushSpawner.Spawn == true)
        {
            BrushSpawner.SpawnBrushes();
            CaptureCuboidHeightMap.CaptureCubemap = true;
        }
    }

    void Update()
    {
        if(CaptureCuboidHeightMap.IsReady == true)
        {
            CuboidHM = new CuboidHeightMapScript(CaptureCuboidHeightMap.TopHM, CaptureCuboidHeightMap.BottomHM, 
                                                 CaptureCuboidHeightMap.RightHM, CaptureCuboidHeightMap.LeftHM, 
                                                 CaptureCuboidHeightMap.FrontHM, CaptureCuboidHeightMap.BackHM);
            this.Construct();
            IsReady = true;
            CaptureCuboidHeightMap.IsReady = false;
            BrushSpawner.gameObject.SetActive(false);
            Screen.SetResolution(1920, 1080, false);
        }

        if(IsReady == true)
        {
            this.Render();
        }
    }

    void Construct()
    {
        Size = 2.0f;

        GridPool = new GridPoolScript(GridPoolCount, Size, Divisions, Material);

        TopFace = new GridFaceScript(LODDepth, Size, Divisions, Material, GridFaceType.TOP);
        BottomFace = new GridFaceScript(LODDepth, Size, Divisions, Material, GridFaceType.BOTTOM);
        RightFace = new GridFaceScript(LODDepth, Size, Divisions, Material, GridFaceType.RIGHT);
        LeftFace = new GridFaceScript(LODDepth, Size, Divisions, Material, GridFaceType.LEFT);
        FrontFace = new GridFaceScript(LODDepth, Size, Divisions, Material, GridFaceType.FRONT);
        BackFace = new GridFaceScript(LODDepth, Size, Divisions, Material, GridFaceType.BACK);

        Application.targetFrameRate = 60;

        CameraPosition = Vector3.one;

        ProcessFrameCountOffset = 10;

        ProcessThread = new Thread(ProcessThreadHandler);
        ProcessThread.Start();
    }

    void Render()
    {
        if(GridPool != null)
        {
            GridPool.Render();
        } 

        if(SceneCamera.transform.position != CameraPosition)
        {
            CameraPosition = SceneCamera.transform.position;
            IsCameraUpdated = true;
            ProcessFrameCountOffset = 1;
        }
        else
        {
            IsCameraUpdated = false;
            ProcessFrameCountOffset--;

            if(ProcessFrameCountOffset <= 0)
            {
                ProcessFrameCountOffset = 0;
            }
        }

        if(IsProcessDone == true)
        {
            if(ProcessThread != null)
            {
                ProcessThread = null;
            }

            if(ProcessThread == null && ProcessFrameCountOffset > 0)
            {
                GridPool.Prepare(SceneCamera);

                IsProcessDone = false;

                ProcessThread = new Thread(ProcessThreadHandler);
                GridPool.ProcessCount = 0;
                ProcessThread.Start();
            }
        }

      
    }

    void ProcessThreadHandler()
    {
        Debug.Log("Thread Executed Successfully!!!");
        if(IsProcessDone == false)
        {
            TopFace.Update(CameraPosition, Radius, GridPool);
            BottomFace.Update(CameraPosition, Radius, GridPool);
            RightFace.Update(CameraPosition, Radius, GridPool);
            LeftFace.Update(CameraPosition, Radius, GridPool);
            FrontFace.Update(CameraPosition, Radius, GridPool);
            BackFace.Update(CameraPosition, Radius, GridPool);

            GridPool.Process(Radius, CuboidHM);
        }

        IsProcessDone = true;
    }
}
