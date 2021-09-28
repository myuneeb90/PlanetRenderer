using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridFaceType
{
    TOP = 0,
    BOTTOM,
    RIGHT,
    LEFT,
    FRONT,
    BACK,
    NONE
};

public enum GridGeometryStates
{
    AVAILABLE = 0,
    INPROCESS = 1,
    ISREADY = 2,
    RENDER = 3
}

public class GridMeshScript
{
    public List<Vector3> VertexBuffer;
    public List<Vector3> NormalBuffer;
    public List<Vector4> TangentBuffer;
    public List<Vector2> TexcoordBuffer;
    public List<int> IndexBuffer;

    public Vector3 Center;
    public Mesh Mesh;

    public GridMeshScript()
    {
        VertexBuffer = new List<Vector3>();
        NormalBuffer = new List<Vector3>();
        TexcoordBuffer = new List<Vector2>();
        TangentBuffer = new List<Vector4>();
        IndexBuffer = new List<int>();
        Mesh = null;
    }

    public void Prepare(Camera sceneCamera)
    {
        if(Mesh == null)
        {
            Mesh = new Mesh();
        }

        float distToCenter = (sceneCamera.farClipPlane - sceneCamera.nearClipPlane) / 2.0f;
        Vector3 center = sceneCamera.transform.position + sceneCamera.transform.forward * distToCenter;
        float extremeBound = 10000;
        Mesh.bounds = new Bounds(center, Vector3.one * extremeBound);

        Mesh.vertices = VertexBuffer.ToArray();
        Mesh.normals = NormalBuffer.ToArray();
        Mesh.uv = TexcoordBuffer.ToArray();
        Mesh.tangents = TangentBuffer.ToArray();
        Mesh.triangles = IndexBuffer.ToArray();
        Mesh.RecalculateNormals();
        Mesh.RecalculateTangents();
    }
}

public class GridGeometryScript
{
    public GridMeshScript GridMesh;
    public Material GridMaterial;
    public GridFaceType FaceType;
    public Matrix4x4 FaceMatrix;

    public GridGeometryStates State;
//    public bool IsOccupied;

    public float Size;
    public Vector3 Center;
    public int Divisions;

    public GridGeometryScript(float size, int divisions, Material material)
    {
        GridMaterial = material;
        State = GridGeometryStates.AVAILABLE;
        this.Construct(size, divisions);
    }

    public void Construct(float size, int divisions)
    {
        float edgeLength = size / (float)divisions;
        float halfSize = size / 2 + edgeLength;
        int divOffset = 1;
        int divOffsetMinusOne =  divOffset - 1;    

        GridMesh = new GridMeshScript();

        for(int z = 0; z < divisions + divOffset; z++)
        {
            for(int x = 0; x < divisions + divOffset; x++)
            {
                Vector3 vertex = new Vector3();

                vertex.x = (-halfSize + edgeLength * x);
                vertex.z = (halfSize - edgeLength * z);
                vertex.y = 0;    

                GridMesh.VertexBuffer.Add(vertex);
                GridMesh.NormalBuffer.Add(Vector3.up);
                GridMesh.TexcoordBuffer.Add(new Vector2());
                GridMesh.TangentBuffer.Add(new Vector4());    
            }
        }

        for(int z = 0; z < divisions + divOffsetMinusOne; z++)
        {
            for(int x = 0; x < divisions + divOffsetMinusOne; x++)
            {
                // Triangle 1
                GridMesh.IndexBuffer.Add((x + z * (divisions + divOffset)));
                GridMesh.IndexBuffer.Add(((x + 1) + z * (divisions + divOffset)));
                GridMesh.IndexBuffer.Add((x + (z + 1) * (divisions + divOffset)));

                // Triangle 2
                GridMesh.IndexBuffer.Add((x + (z + 1) * (divisions + divOffset)));
                GridMesh.IndexBuffer.Add(((x + 1) + z * (divisions + divOffset)));
                GridMesh.IndexBuffer.Add(((x + 1) + (z + 1) * (divisions + divOffset)));                
            }
        }  
    }

    public void Process(float radius, CuboidHeightMapScript cuboidHM)
    {
        Vector3 orientationAngles = GridHelperScript.GetOrientationAngles(FaceType);
        FaceMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(orientationAngles), Vector3.one);


        float edgeLength = Size / (float)Divisions;
        float halfSize = Size / 2;// + edgeLength;
        int divOffset = 1;
        int divOffsetMinusOne =  divOffset - 1; 

        for(int z = 0; z < Divisions + divOffset; z++)
        {
            for(int x = 0; x < Divisions + divOffset; x++)
            {      
                int idx = x + z * (Divisions + divOffset);
                Vector3 vertex = new Vector3();

                vertex.x = Center.x + (-halfSize + edgeLength * x);
                vertex.z = Center.z + (halfSize - edgeLength * z);
                vertex.y = Center.y; 
                Vector3 cubePos = FaceMatrix.MultiplyVector(vertex);
                Vector3 spherePos = GridHelperScript.GetCubeToSpherePosition(cubePos);

                Vector3 uvh = cuboidHM.GetHeightValue(cubePos, FaceType, edgeLength);

                GridMesh.VertexBuffer[idx] = spherePos * (1 + uvh.z) * radius;
                // GridMesh.NormalBuffer[idx] = Vector3.up;
                // GridMesh.TexcoordBuffer[idx] = new Vector2();
                // GridMesh.TangentBuffer[idx] = new Vector4();    
            }
        }

        GridMesh.Center = Center;

        State = GridGeometryStates.ISREADY;
    }

    public void Prepare(Camera sceneCamera)
    {
        GridMesh.Prepare(sceneCamera);
        State = GridGeometryStates.RENDER;
    }

    public void Render()
    {
        Graphics.DrawMesh(GridMesh.Mesh, Matrix4x4.identity, GridMaterial, 0);
    }

    
}
