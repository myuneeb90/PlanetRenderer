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
    public float GridColliderRange = 500;
    public int GridColliderPoolCount;
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
    private Vector3 LastPosition;
    private int ProcessFrameCountOffset;

  //  public Vector3 PlanetPosition;
    public GameObject ColliderPrefab;
    private List<GridMeshColliderScript> Colliders;

    public Transform Player;
    public Rigidbody PlayerRB;
    public Transform PlayerCollider;

    private CuboidPrecisionHeightMapScript CuboidHM;

   // private bool IsReady = false;

    private List<Material> GridMaterials;

    public DebuggerScript Debugger;

   // private bool FirstFrame = false;

    private Matrix4x4 PlanetMatrix;

    private void SpawnColliders()
    {
        Colliders = new List<GridMeshColliderScript>();
        for(int i = 0; i < GridColliderPoolCount; i++)
        {
            GameObject newCollider = (GameObject)Instantiate(ColliderPrefab, Vector3.zero, Quaternion.identity);
            newCollider.transform.SetParent(this.transform);
            Colliders.Add(new GridMeshColliderScript(newCollider.GetComponent<MeshCollider>()));
        }
    }


    void Start()
    {
        this.SpawnColliders();

        GridMaterials = new List<Material>();
        GridMaterials.Add(TopMaterial);
        GridMaterials.Add(BottomMaterial);
        GridMaterials.Add(RightMaterial);
        GridMaterials.Add(LeftMaterial);
        GridMaterials.Add(FrontMaterial);
        GridMaterials.Add(BackMaterial);

        LastPosition = Player.transform.position;
    //    LastPosition = -Player.Position;
//        PlanetPosition = -LastCameraPosition;
    //    this.transform.position = LastPosition;

        CuboidHM = new CuboidPrecisionHeightMapScript(HeightMaps[0], HeightMaps[1], HeightMaps[2], 
                                                      HeightMaps[3], HeightMaps[4], HeightMaps[5]);

        this.Construct(); 
    }

    void Update()
    {        


//        this.transform.position = this.transform.position - Player.OffsetPosition;

    //    CameraPosition = Player.OffsetPosition;

   //     PlanetMatrix = Matrix4x4.TRS(PlanetPosition, Quaternion.identity, Vector3.one);

        PlanetMatrix = this.transform.localToWorldMatrix;
        
        this.Render();
    }

    void FixedUpdate()
    {
    //      Vector3 playerPosition = Player.position.normalized;

    //      Vector3 stcPos = GridHelperScript.GetSphereToCubePosition(playerPosition);

    //      stcPos.x = Mathf.Round(stcPos.x * 10000) * 0.0001f;
    //      stcPos.y = Mathf.Round(stcPos.y * 10000) * 0.0001f;
    //      stcPos.z = Mathf.Round(stcPos.z * 10000) * 0.0001f;

    //      float edgeLength = 0.1f / (float)Divisions;

    //      edgeLength = Mathf.Round(edgeLength * 10000) * 0.0001f;

    //      Vector3 uvh = CuboidHM.GetHeightValue(stcPos, GridFaceType.TOP, edgeLength);

    // //   //  PlayerRB.isKinematic = true;
    // //  //   Player.position = playerPosition * (1 + uvh.z) * (Radius + 4);
    // //  //   PlayerRB.isKinematic = false;

    // //     PlayerCollider.position = Vector3.MoveTowards(PlayerCollider.position, playerPosition * (1 + uvh.z) * (Radius), 20 * Time.deltaTime);
    // //     PlayerCollider.up = playerPosition;

    //     Vector3 surfacePos = playerPosition * (1 + uvh.z) * (Radius);

    //     float d1 = Vector3.Distance(Player.transform.position, Vector3.zero);
    //     float d2 = Vector3.Distance(surfacePos, Vector3.zero);

    //     if(d1 < d2)
    //     {
    //         Player.position = surfacePos;
    //     }
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

    //    LastCameraPosition = Player.Position;

        ProcessFrameCountOffset = 10;

        ProcessThread = new Thread(ProcessThreadHandler);
        ProcessThread.Start();
    }

    void Render()
    {
        if(Player.transform.position != LastPosition)
        {
          //  LastCameraPosition = -Player.Position;
          //  PlanetPosition = -LastCameraPosition;
           // this.transform.position = LastCameraPosition;
            LastPosition = Player.transform.position;
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
                GridPool.Prepare(SceneCamera, Radius, Player, this.transform.localToWorldMatrix, Colliders, GridColliderRange, this.LODDepth);

                IsProcessDone = false;

                ProcessThread = new Thread(ProcessThreadHandler);
                GridPool.ProcessCount = 0;
                ProcessThread.Start();
            }
        }

        if(GridPool != null)
        {
            GridPool.Render(GridMaterials, SceneCamera, this.transform.localToWorldMatrix, this.transform.position);
        }       
    }

    void ProcessThreadHandler()
    {
        if(IsProcessDone == false)
        {
            TopFace.Update(LastPosition, Radius, GridPool, PlanetMatrix);
            BottomFace.Update(LastPosition, Radius, GridPool, PlanetMatrix);
            RightFace.Update(LastPosition, Radius, GridPool, PlanetMatrix);
            LeftFace.Update(LastPosition, Radius, GridPool, PlanetMatrix);
            FrontFace.Update(LastPosition, Radius, GridPool, PlanetMatrix);
            BackFace.Update(LastPosition, Radius, GridPool, PlanetMatrix);

            GridPool.Process(Radius, CuboidHM, Debugger);
        }

        IsProcessDone = true;
    }
}
