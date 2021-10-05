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
    public Vector3[] VertexBuffer;
    public Vector3[] NormalBuffer;
    public Vector4[] TangentBuffer;
    public Vector2[] TexcoordBuffer;
    public int[] IndexBuffer;

    public Vector3 Center;
    public Mesh Mesh;

    public GridMeshScript(int vbCount, int ibCount)
    {
        VertexBuffer = new Vector3[vbCount]; //new List<Vector3>();
        NormalBuffer = new Vector3[vbCount];//List<Vector3>();
        TexcoordBuffer = new Vector2[vbCount];//List<Vector2>();
        TangentBuffer = new Vector4[vbCount];//List<Vector4>();
        IndexBuffer = new int[ibCount];//List<int>();
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

        Mesh.vertices = VertexBuffer;//.ToArray();
        Mesh.normals = NormalBuffer;//.ToArray();
        Mesh.uv = TexcoordBuffer;//.ToArray();
        Mesh.tangents = TangentBuffer;//.ToArray();
        Mesh.triangles = IndexBuffer;//.ToArray();
     //   Mesh.RecalculateNormals();
     //   Mesh.RecalculateTangents();
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

        int vbCount = (divisions + divOffset) * (divisions + divOffset);
        int ibCount = (divisions + divOffsetMinusOne) * (divisions + divOffsetMinusOne) * 6;

        GridMesh = new GridMeshScript(vbCount, ibCount);

        // for(int z = 0; z < divisions + divOffset; z++)
        // {
        //     for(int x = 0; x < divisions + divOffset; x++)
        //     {
        //         Vector3 vertex = new Vector3();

        //         vertex.x = (-halfSize + edgeLength * x);
        //         vertex.z = (halfSize - edgeLength * z);
        //         vertex.y = 0;    

        //         GridMesh.VertexBuffer.Add(vertex);
        //         GridMesh.NormalBuffer.Add(Vector3.up);
        //         GridMesh.TexcoordBuffer.Add(new Vector2());
        //         GridMesh.TangentBuffer.Add(new Vector4());    
        //     }
        // }
        int count = 0;

        for(int z = 0; z < divisions + divOffsetMinusOne; z++)
        {
            for(int x = 0; x < divisions + divOffsetMinusOne; x++)
            {
                // Triangle 1
                GridMesh.IndexBuffer[count] = (x + z * (divisions + divOffset));
                count++;
                GridMesh.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset));
                count++;
                GridMesh.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset));
                count++;

                // Triangle 2
                GridMesh.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset));
                count++;
                GridMesh.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset));
                count++;
                GridMesh.IndexBuffer[count] = ((x + 1) + (z + 1) * (divisions + divOffset));                
                count++;
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

        List<Vector2> tileUVs = new List<Vector2>();
        tileUVs.AddRange(GridMesh.TexcoordBuffer);

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
                tileUVs[idx] = new Vector2(edgeLength + (float)x / (float)(Divisions), 
                                           edgeLength + (float)z / (float)Divisions);
                // GridMesh.TangentBuffer[idx] = new Vector4();    
            }
        }

        // Calculate Normals
        for(int i = 0; i < GridMesh.IndexBuffer.Length; i += 3)
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

        for(int i = 0; i < GridMesh.NormalBuffer.Length; i++)
        {
            GridMesh.NormalBuffer[i] = GridMesh.NormalBuffer[i].normalized;
        }

        // Calculate Tangents
        Vector3[] tan1 = new Vector3[GridMesh.VertexBuffer.Length];
        Vector3[] tan2 = new Vector3[GridMesh.VertexBuffer.Length];
        Vector4[] tangents = new Vector4[GridMesh.VertexBuffer.Length];

        for(int a = 0; a < GridMesh.IndexBuffer.Length; a += 3)
        {
            int i1 = GridMesh.IndexBuffer[a + 0];
            int i2 = GridMesh.IndexBuffer[a + 1];
            int i3 = GridMesh.IndexBuffer[a + 2];

            Vector3 v1 = edgeVertices[i1];
            Vector3 v2 = edgeVertices[i2];
            Vector3 v3 = edgeVertices[i3];

            Vector2 w1 = tileUVs[i1];
            Vector2 w2 = tileUVs[i2];
            Vector2 w3 = tileUVs[i3];

            float x1 = v2.x - v1.x;
            float x2 = v3.x - v1.x;
            float y1 = v2.y - v1.y;
            float y2 = v3.y - v1.y;
            float z1 = v2.z - v1.z;
            float z2 = v3.z - v1.z;

            float s1 = w2.x - w1.x;
            float s2 = w3.x - w1.x;
            float t1 = w2.y - w1.y;
            float t2 = w3.y - w1.y;

            float r = 1 / (s1 * t2 - s2 * t1);
            Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;
        }

        int vtxIdx = 0;

        for (int z = 0; z < Divisions + divOffset; z++)
        {
            for (int x = 0; x < Divisions + divOffset; x++)
            {
                vtxIdx = x + z * (Divisions + divOffset);

                Vector3 n = GridMesh.NormalBuffer[vtxIdx];
                Vector3 t = tan1[vtxIdx];
                Vector3.OrthoNormalize(ref n, ref t);
                tangents[vtxIdx].x = t.x;
                tangents[vtxIdx].y = t.y;
                tangents[vtxIdx].z = t.z;
                tangents[vtxIdx].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[vtxIdx]) < 0.0f) ? -1.0f : 1.0f;
                GridMesh.TangentBuffer[vtxIdx] = tangents[vtxIdx];
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
