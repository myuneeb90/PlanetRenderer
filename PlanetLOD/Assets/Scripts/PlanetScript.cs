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

    void Awake()
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
    }

    void Start()
    {
        ProcessThread = new Thread(ProcessThreadHandler);
        ProcessThread.Start();
    }

    void Update()
    {
        if(SceneCamera.transform.position != CameraPosition)
        {
            CameraPosition = SceneCamera.transform.position;
            IsCameraUpdated = true;
            ProcessFrameCountOffset = 10;
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

            if(ProcessThread == null && ProcessFrameCountOffset > 0)// && IsCameraUpdated == true)
            {
                GridPool.Prepare(SceneCamera);

                IsProcessDone = false;

                ProcessThread = new Thread(ProcessThreadHandler);
                GridPool.ProcessCount = 0;
                ProcessThread.Start();
            }
        }

        GridPool.Render();        
    }

    void ProcessThreadHandler()
    {
        Debug.Log("Thread Executed Successfully!!!");
        if(IsProcessDone == false)
        {
      //      GridPool.ProcessCount = 0;
            TopFace.Update(CameraPosition, Radius, GridPool);
            BottomFace.Update(CameraPosition, Radius, GridPool);
            RightFace.Update(CameraPosition, Radius, GridPool);
            LeftFace.Update(CameraPosition, Radius, GridPool);
            FrontFace.Update(CameraPosition, Radius, GridPool);
            BackFace.Update(CameraPosition, Radius, GridPool);

            GridPool.Process(Radius);
        }

        IsProcessDone = true;
    }
}
