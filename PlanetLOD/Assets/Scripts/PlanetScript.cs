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

    public Material TopMaterial;
    public Material BottomMaterial;
    public Material RightMaterial;
    public Material LeftMaterial;
    public Material FrontMaterial;
    public Material BackMaterial;

    public List<Texture2D> HeightMaps;

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
    private int ProcessFrameCountOffset;

    public Transform Player;
    public Transform PlayerCollider;

    private CuboidHeightMapScript CuboidHM;

   // private bool IsReady = false;

    private List<Material> GridMaterials;

    public DebuggerScript Debugger;

   // private bool FirstFrame = false;


    void Start()
    {
        GridMaterials = new List<Material>();
        GridMaterials.Add(TopMaterial);
        GridMaterials.Add(BottomMaterial);
        GridMaterials.Add(RightMaterial);
        GridMaterials.Add(LeftMaterial);
        GridMaterials.Add(FrontMaterial);
        GridMaterials.Add(BackMaterial);

        CuboidHM = new CuboidHeightMapScript(HeightMaps[0], HeightMaps[1], HeightMaps[2], 
                                             HeightMaps[3], HeightMaps[4], HeightMaps[5]);

        this.Construct();
    }

    void Update()
    {        
        Vector3 playerPosition = Player.position.normalized;

        Vector3 uvh = CuboidHM.GetHeightValue(GridHelperScript.GetSphereToCubePosition(playerPosition), GridFaceType.TOP, 0.1f / (float)Divisions);

        PlayerCollider.position = playerPosition * (1 + uvh.z) * (Radius - 1);
        PlayerCollider.up = playerPosition;

        float d1 = Vector3.Distance(Player.position, Vector3.zero);
        float d2 = Vector3.Distance(PlayerCollider.position, Vector3.zero);

        if(d1 < d2)
        {
            Player.position = playerPosition * (1 + uvh.z) * (Radius + 1);
        }
        
        this.Render();
    }

    void Construct()
    {
        Size = 2.0f;

        GridPool = new GridPoolScript(GridPoolCount, Size, Divisions);

        TopFace = new GridFaceScript(LODDepth, Size, Divisions, TopMaterial, GridFaceType.TOP);
        BottomFace = new GridFaceScript(LODDepth, Size, Divisions, BottomMaterial, GridFaceType.BOTTOM);
        RightFace = new GridFaceScript(LODDepth, Size, Divisions, RightMaterial, GridFaceType.RIGHT);
        LeftFace = new GridFaceScript(LODDepth, Size, Divisions, LeftMaterial, GridFaceType.LEFT);
        FrontFace = new GridFaceScript(LODDepth, Size, Divisions, FrontMaterial, GridFaceType.FRONT);
        BackFace = new GridFaceScript(LODDepth, Size, Divisions, BackMaterial, GridFaceType.BACK);

        Application.targetFrameRate = 60;

        CameraPosition = Vector3.one;

        ProcessFrameCountOffset = 10;

        ProcessThread = new Thread(ProcessThreadHandler);
        ProcessThread.Start();
    }

    void Render()
    {
        if(SceneCamera.transform.position != CameraPosition)
        {
            CameraPosition = SceneCamera.transform.position;
            ProcessFrameCountOffset = 10;
        }
        else
        {
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
                GridPool.Prepare(SceneCamera, Radius);

                IsProcessDone = false;

                ProcessThread = new Thread(ProcessThreadHandler);
                GridPool.ProcessCount = 0;
                ProcessThread.Start();
            }
        }

        if(GridPool != null)
        {
            GridPool.Render(GridMaterials, SceneCamera);
        }       
    }

    void ProcessThreadHandler()
    {
        if(IsProcessDone == false)
        {
            TopFace.Update(CameraPosition, Radius, GridPool);
            BottomFace.Update(CameraPosition, Radius, GridPool);
            RightFace.Update(CameraPosition, Radius, GridPool);
            LeftFace.Update(CameraPosition, Radius, GridPool);
            FrontFace.Update(CameraPosition, Radius, GridPool);
            BackFace.Update(CameraPosition, Radius, GridPool);

            GridPool.Process(Radius, CuboidHM, Debugger);
        }

        IsProcessDone = true;
    }
}
