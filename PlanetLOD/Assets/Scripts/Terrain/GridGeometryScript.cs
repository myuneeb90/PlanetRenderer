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
        float extremeBound = 1000000;
        Mesh.bounds = new Bounds(center, Vector3.one * extremeBound);

        Mesh.vertices = VertexBuffer.ToArray();
        Mesh.normals = NormalBuffer.ToArray();
        Mesh.uv = TexcoordBuffer.ToArray();
        Mesh.tangents = TangentBuffer.ToArray();
        Mesh.triangles = IndexBuffer.ToArray();
     //   Mesh.RecalculateNormals();
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
        int divOffset = 3;
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
        float halfSize = Size / 2 + edgeLength;
        int divOffset = 3;
        int divOffsetMinusOne =  divOffset - 1; 

        List<Vector3> edgeVertices = new List<Vector3>();
        edgeVertices.AddRange(GridMesh.VertexBuffer);

        for(int z = 0; z < Divisions + divOffset; z++)
        {
            for(int x = 0; x < Divisions + divOffset; x++)
            {      
                int idx = x + z * (Divisions + divOffset);
                Vector3 vertex = new Vector3();
                Vector3 edgeVertex = new Vector3();

                vertex.x = Center.x + (-halfSize + edgeLength * x);
                vertex.z = Center.z + (halfSize - edgeLength * z);
                vertex.y = Center.y; 

                edgeVertex.x = vertex.x;
                edgeVertex.z = vertex.z;


                if(x > 0 && z > 0 && x < Divisions + divOffsetMinusOne && z < Divisions + divOffsetMinusOne)
                {
                    vertex.y = Center.y;
                    edgeVertex.y = Center.y;
                }
                else
                {
                    vertex.y = Center.y;
                    edgeVertex.y = Center.y;

                    if(x == 0)
                    {
                        vertex.x = vertex.x + edgeLength;
                    }

                    if(z == 0)
                    {
                        vertex.z = vertex.z - edgeLength;
                    }

                    if(x == Divisions + divOffsetMinusOne)
                    {
                        vertex.x = vertex.x - edgeLength;
                    }

                    if(z == Divisions + divOffsetMinusOne)
                    {
                        vertex.z = vertex.z + edgeLength;
                    }

                    if(Mathf.Approximately(Mathf.Abs(vertex.x), 1.0f) == true ||
                       Mathf.Approximately(Mathf.Abs(vertex.z), 1.0f) == true)
                    {
                        edgeVertex.x = vertex.x;
                        edgeVertex.z = vertex.z;
                        edgeVertex.y = Center.y - edgeLength;
                    }
                }



                Vector3 cubePos = FaceMatrix.MultiplyVector(vertex);
                Vector3 edgeCubePos = FaceMatrix.MultiplyVector(edgeVertex);
                
                Vector3 spherePos = GridHelperScript.GetCubeToSpherePosition(cubePos);
                Vector3 edgeSpherePos = GridHelperScript.GetCubeToSpherePosition(edgeCubePos);

                Vector3 uvh = cuboidHM.GetHeightValue(cubePos, FaceType, edgeLength);
                Vector3 edgeUvh = cuboidHM.GetHeightValue(edgeCubePos, FaceType, edgeLength);

                Vector3 finalSpherePos = new Vector3();

                if(x > 0 && z > 0 && x < Divisions + divOffsetMinusOne && z < Divisions + divOffsetMinusOne)
                {
                    finalSpherePos = spherePos * (1 + uvh.z) * radius;
                }
                else
                {
                    finalSpherePos = spherePos * (1 + uvh.z - edgeLength) * radius;
                }

                Vector3 finalEdgeSpherePos = edgeSpherePos * (1 + edgeUvh.z) * radius;

                GridMesh.VertexBuffer[idx] = finalSpherePos;
                edgeVertices[idx] = finalEdgeSpherePos;
                
                // GridMesh.NormalBuffer[idx] = Vector3.up;
                GridMesh.TexcoordBuffer[idx] = new Vector2(uvh.x, uvh.y);
                // GridMesh.TangentBuffer[idx] = new Vector4();    
            }
        }

        // Calculate Normals
        for(int i = 0; i < GridMesh.IndexBuffer.Count; i += 3)
        {
            int i0 = GridMesh.IndexBuffer[i];
            int i1 = GridMesh.IndexBuffer[i + 1];
            int i2 = GridMesh.IndexBuffer[i + 2];

            Vector3 v0 = edgeVertices[i0];
            Vector3 v1 = edgeVertices[i1];
            Vector3 v2 = edgeVertices[i2];

            Vector3 u = v1 - v0;
            Vector3 v = v2 - v0;

            Vector3 n = Vector3.Cross(u, v);

            GridMesh.NormalBuffer[i0] += n;
            GridMesh.NormalBuffer[i1] += n;
            GridMesh.NormalBuffer[i2] += n;        
        }

        for(int i = 0; i < GridMesh.NormalBuffer.Count; i++)
        {
            GridMesh.NormalBuffer[i] = GridMesh.NormalBuffer[i].normalized;
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
