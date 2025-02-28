﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GridFaceType
{
    TOP = 0,
    BOTTOM = 1,
    RIGHT = 2,
    LEFT = 3,
    FRONT = 4,
    BACK = 5,
    NONE = 6
};

public enum GridGeometryStates
{
    AVAILABLE = 0,
    INPROCESS = 1,
    ISREADY = 2,
    RENDER = 3
};

public class GridMeshScript
{
    public Mesh MeshObj;
    public Bounds BoundingBox;
    public GridFaceType FaceType;
    private Vector3 v3FrontTopLeft;
    private Vector3 v3FrontTopRight;
    private Vector3 v3FrontBottomLeft;
    private Vector3 v3FrontBottomRight;
    private Vector3 v3BackTopLeft;
    private Vector3 v3BackTopRight;
    private Vector3 v3BackBottomLeft;
    private Vector3 v3BackBottomRight; 

    private Vector3[] BoundingPositions;
    private Matrix4x4 NormalMatrix;
    public Vector3 Center;
    public int LODIndex;

    public GridMeshScript()
    {
        MeshObj = new Mesh();
        BoundingPositions = new Vector3[8];
    }

    public void ComputeBoundingBox(Vector3[] bbPositions)
    {
        // Bounds bounds = BoundingBox;
                        
        // Vector3 v3Center = bounds.center;
        // Vector3 v3Extents = bounds.extents;
    
        // v3FrontTopLeft     = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        // v3FrontTopRight    = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        // v3FrontBottomLeft  = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        // v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        // v3BackTopLeft      = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        // v3BackTopRight     = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        // v3BackBottomLeft   = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        // v3BackBottomRight  = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        v3FrontTopLeft     = bbPositions[0];
        v3FrontTopRight    = bbPositions[1];
        v3BackTopLeft      = bbPositions[2];
        v3BackTopRight     = bbPositions[3];

        v3FrontBottomLeft  = bbPositions[4];
        v3FrontBottomRight = bbPositions[5];
        v3BackBottomLeft   = bbPositions[6];
        v3BackBottomRight  = bbPositions[7];


        // v3FrontTopLeft     = transform.TransformPoint(v3FrontTopLeft);
        // v3FrontTopRight    = transform.TransformPoint(v3FrontTopRight);
        // v3FrontBottomLeft  = transform.TransformPoint(v3FrontBottomLeft);
        // v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        // v3BackTopLeft      = transform.TransformPoint(v3BackTopLeft);
        // v3BackTopRight     = transform.TransformPoint(v3BackTopRight);
        // v3BackBottomLeft   = transform.TransformPoint(v3BackBottomLeft);
        // v3BackBottomRight  = transform.TransformPoint(v3BackBottomRight);
    }

    public void UpdateBoundingBox(Matrix4x4 planetMatrix, Vector3 planetPosition)
    {
        // for(int i = 0; i < BoundingPositions.Length; i++)
        // {
        //     BoundingPositions[i] = BoundingPositions[i] + planetPosition;
        // }

    //    BoundingBox = GeometryUtility.CalculateBounds(BoundingPositions, NormalMatrix);
    //    BoundingBox.center = BoundingBox.center + planetPosition;//planetMatrix.MultiplyVector(Center + planetPosition);// + planetPosition);

        // BoundingBox.center = planetMatrix.MultiplyPoint(Center);

        // v3FrontTopLeft     = BoundingPositions[0] + planetPosition;
        // v3FrontTopRight    = BoundingPositions[1] + planetPosition;
        // v3BackTopLeft      = BoundingPositions[2] + planetPosition;
        // v3BackTopRight     = BoundingPositions[3] + planetPosition;

        // v3FrontBottomLeft  = BoundingPositions[4] + planetPosition;
        // v3FrontBottomRight = BoundingPositions[5] + planetPosition;
        // v3BackBottomLeft   = BoundingPositions[6] + planetPosition;
        // v3BackBottomRight  = BoundingPositions[7] + planetPosition;
    }

    public void DrawBoundingBox(Color color)
    {
        Debug.DrawLine(BoundingBox.center, BoundingBox.center + Vector3.up * 300, Color.yellow);

        Debug.DrawLine (v3FrontTopLeft, v3FrontTopRight, color);
        Debug.DrawLine (v3FrontTopRight, v3FrontBottomRight, color);
        Debug.DrawLine (v3FrontBottomRight, v3FrontBottomLeft, color);
        Debug.DrawLine (v3FrontBottomLeft, v3FrontTopLeft, color);
            
        Debug.DrawLine (v3BackTopLeft, v3BackTopRight, color);
        Debug.DrawLine (v3BackTopRight, v3BackBottomRight, color);
        Debug.DrawLine (v3BackBottomRight, v3BackBottomLeft, color);
        Debug.DrawLine (v3BackBottomLeft, v3BackTopLeft, color);
            
        Debug.DrawLine (v3FrontTopLeft, v3BackTopLeft, color);
        Debug.DrawLine (v3FrontTopRight, v3BackTopRight, color);
        Debug.DrawLine (v3FrontBottomRight, v3BackBottomRight, color);
        Debug.DrawLine (v3FrontBottomLeft, v3BackBottomLeft, color);
    }

    public void Prepare(Camera sceneCamera, Vector3[] vertexBuffer, Vector3[] normalBuffer, 
                        Vector2[] texcoordBuffer, Vector4[] tangentBuffer, int[] indexBuffer,
                        Vector3 bbCenter, float size, float radius, GridFaceType faceType,
                        int divisions, int divOffset, Transform player,
                        Matrix4x4 planetMatrix, int lodIndex)
    {
        FaceType = faceType;
        LODIndex = lodIndex;

        float distToCenter = (sceneCamera.farClipPlane - sceneCamera.nearClipPlane) / 2.0f;
        Vector3 center = -player.position + sceneCamera.transform.forward * distToCenter;
        float extremeBound = 1000000;
        MeshObj.bounds = new Bounds(center, Vector3.one * extremeBound);

        int x = 0, z = 0;        
        x = (divisions + divOffset) / 2; z = (divisions + divOffset) / 2;
        Center = vertexBuffer[x + z * (divisions + divOffset)];        

        Vector3 yOffset = Vector3.up * 100;
        x = 0; z = 0;
        BoundingPositions[0] = vertexBuffer[x + z * (divisions + divOffset)] + yOffset; // TUL
        x = divisions + divOffset - 1; z = 0;
        BoundingPositions[1] = vertexBuffer[x + z * (divisions + divOffset)] + yOffset; // TUR
        x = 0; z = divisions + divOffset - 1;
        BoundingPositions[2] = vertexBuffer[x + z * (divisions + divOffset)] + yOffset; // TDL
        x = divisions + divOffset - 1; z = divisions + divOffset - 1;
        BoundingPositions[3] = vertexBuffer[x + z * (divisions + divOffset)] + yOffset; // TDR

        x = 0; z = 0;
        BoundingPositions[4] = vertexBuffer[x + z * (divisions + divOffset)] - yOffset; // BUL
        x = divisions + divOffset - 1; z = 0;
        BoundingPositions[5] = vertexBuffer[x + z * (divisions + divOffset)] - yOffset; // BUR
        x = 0; z = divisions + divOffset - 1;
        BoundingPositions[6] = vertexBuffer[x + z * (divisions + divOffset)] - yOffset; // BDL
        x = divisions + divOffset - 1; z = divisions + divOffset - 1;
        BoundingPositions[7] = vertexBuffer[x + z * (divisions + divOffset)] - yOffset; // BDR

        // for(int i = 0; i < BoundingPositions.Length; i++)
        // {
        //     BoundingPositions[i] = planetMatrix.MultiplyPoint(BoundingPositions[i]);
        // }

        x = (divisions + divOffset - 1) / 2; z = (divisions + divOffset - 1) / 2;
        NormalMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.FromToRotation(Vector3.forward, normalBuffer[x + z * (divisions + divOffset)]), Vector3.one);
        BoundingBox = GeometryUtility.CalculateBounds(BoundingPositions, NormalMatrix);
        


        BoundingBox.center = Center;//planetMatrix.MultiplyPoint(bbCenter);

        v3FrontTopLeft     = BoundingPositions[0];
        v3FrontTopRight    = BoundingPositions[1];
        v3BackTopLeft      = BoundingPositions[2];
        v3BackTopRight     = BoundingPositions[3];

        v3FrontBottomLeft  = BoundingPositions[4];
        v3FrontBottomRight = BoundingPositions[5];
        v3BackBottomLeft   = BoundingPositions[6];
        v3BackBottomRight  = BoundingPositions[7];        
    //    this.ComputeBoundingBox(BoundingPositions);

        MeshObj.vertices = vertexBuffer;
        MeshObj.normals = normalBuffer;
        MeshObj.uv = texcoordBuffer;
        MeshObj.tangents = tangentBuffer;
        MeshObj.triangles = indexBuffer;
    }

    public void Render(Material gridMaterial, Matrix4x4 planetMatrix)
    {
        if(MeshObj != null)
        {
            Graphics.DrawMesh(MeshObj, planetMatrix, gridMaterial, 0);
        }

        // for(int i = 0; i < MeshObj.vertices.Length; i++)
        // {
        //     Debug.DrawRay(MeshObj.vertices[i], MeshObj.normals[i] * 10, Color.green);
            
        //     Vector3 tan = new Vector3(MeshObj.tangents[i].x, MeshObj.tangents[i].y, MeshObj.tangents[i].z);
        //     Debug.DrawRay(MeshObj.vertices[i], tan * 10, Color.red);
        // }
    }


}

public class GridGeometryScript
{
 //   public GridMeshScript GridMesh;
   // public Material GridMaterial;
    public GridFaceType FaceType;
    public Matrix4x4d FaceMatrix;

    public GridGeometryStates State;
    public int ID;

    public int LODIndex;

    public Vector3[] VertexBuffer;
    public Vector3[] NormalBuffer;
    public Vector4[] TangentBuffer;
    public Vector2[] TexcoordBuffer;
    public int[] IndexBuffer;

    public float Size;
    public Vector3 Center;
    public Vector3 BBCenter;
    public int Divisions;

    public GridGeometryScript(float size, int divisions, int id)
    {
    //    GridMaterial = material;
        State = GridGeometryStates.AVAILABLE;
        ID = id;

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

        VertexBuffer = new Vector3[vbCount];
        NormalBuffer = new Vector3[vbCount];
        TexcoordBuffer = new Vector2[vbCount];
        TangentBuffer = new Vector4[vbCount];
        IndexBuffer = new int[ibCount];

        this.ConstructIndexBuffer(divisions, divOffset, divOffsetMinusOne);
    }

    public void ConstructIndexBuffer(int divisions, int divOffset, int divOffsetMinusOne)
    {
        int count = 0;

        for(int z = 0; z < divisions + divOffsetMinusOne; z++)
        {
            for(int x = 0; x < divisions + divOffsetMinusOne; x++)
            {
                // Triangle 1
                this.IndexBuffer[count] = (x + z * (divisions + divOffset)); // v0
                count++;
                this.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset)); // v1
                count++;
                this.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset)); // v2
                count++;

                // Triangle 2
                this.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset)); // v2
                count++;
                this.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset)); // v1
                count++;
                this.IndexBuffer[count] = ((x + 1) + (z + 1) * (divisions + divOffset)); // v3                
                count++;
            }
        }  
    }

    public void FixIndexBuffer(int divisions, int divOffset, int divOffsetMinusOne)
    {
        if(FaceType == GridFaceType.FRONT || FaceType == GridFaceType.BACK)
        {
            int count = 0;

            for(int z = 0; z < divisions + divOffsetMinusOne; z++)
            {
                for(int x = 0; x < divisions + divOffsetMinusOne; x++)
                {
                    if(x == 0 || x == 1 || x == divisions + divOffsetMinusOne - 2 || x == divisions + divOffsetMinusOne - 1)
                    {
                        // Triangle 1
                        this.IndexBuffer[count] = (x + z * (divisions + divOffset)); // v0
                        count++;
                        this.IndexBuffer[count] = ((x + 1) + (z + 1) * (divisions + divOffset)); // v3  
                        count++;
                        this.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset)); // v2
                        count++;

                        // Triangle 2
                        this.IndexBuffer[count] = (x + z * (divisions + divOffset)); // v0
                        count++;
                        this.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset)); // v1
                        count++;
                        this.IndexBuffer[count] = ((x + 1) + (z + 1) * (divisions + divOffset)); // v3               
                        count++;
                    }
                    else
                    {
                        // Triangle 1
                        this.IndexBuffer[count] = (x + z * (divisions + divOffset)); // v0
                        count++;
                        this.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset)); // v1
                        count++;
                        this.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset)); // v2
                        count++;

                        // Triangle 2
                        this.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset)); // v2
                        count++;
                        this.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset)); // v1
                        count++;
                        this.IndexBuffer[count] = ((x + 1) + (z + 1) * (divisions + divOffset)); // v3                
                        count++;                        
                    }                    
                }
            }
        }
        else
        {
            int count = 0;

            for(int z = 0; z < divisions + divOffsetMinusOne; z++)
            {
                for(int x = 0; x < divisions + divOffsetMinusOne; x++)
                {
                    // Triangle 1
                    this.IndexBuffer[count] = (x + z * (divisions + divOffset)); // v0
                    count++;
                    this.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset)); // v1
                    count++;
                    this.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset)); // v2
                    count++;

                    // Triangle 2
                    this.IndexBuffer[count] = (x + (z + 1) * (divisions + divOffset)); // v2
                    count++;
                    this.IndexBuffer[count] = ((x + 1) + z * (divisions + divOffset)); // v1
                    count++;
                    this.IndexBuffer[count] = ((x + 1) + (z + 1) * (divisions + divOffset)); // v3                
                    count++;
                }
            }              
        }
    }

    private bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }

    public void Process(float radius, CuboidPrecisionHeightMapScript cuboidHM, DebuggerScript debugger)
    {
        Vector3 orientationAngles = GridHelperScript.GetOrientationAngles(FaceType);
        FaceMatrix = Matrix4x4d.TRS(Vector3.zero, orientationAngles, Vector3.one);

        double edgeLength = Size / (double)Divisions;
        double halfSize = Size / 2 + edgeLength;
        int divOffset = 3;
        int divOffsetMinusOne =  divOffset - 1; 

        List<Vector3> edgeVertices = new List<Vector3>();
        edgeVertices.AddRange(VertexBuffer);

        List<Vector2> tileUVs = new List<Vector2>();
        tileUVs.AddRange(TexcoordBuffer);

       this.FixIndexBuffer(Divisions, divOffset, divOffsetMinusOne);

  //      Debug.Log("Grid Face -------- " + FaceType);

        for(int z = 0; z < Divisions + divOffset; z++)
        {
            for(int x = 0; x < Divisions + divOffset; x++)
            {      
                int idx = x + z * (Divisions + divOffset);
                Vector3d vertex = new Vector3d();
                Vector3d edgeVertex = new Vector3d();

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

                    if(Mathd.Approximately(Mathd.Abs(vertex.x), 1.0f) == true ||
                       Mathd.Approximately(Mathd.Abs(vertex.z), 1.0f) == true)
                    {
                        edgeVertex.x = vertex.x;
                        edgeVertex.z = vertex.z;
                        edgeVertex.y = Center.y - edgeLength;
                    }                      
                }

                Vector3d cubePos = FaceMatrix.MultiplyVector(vertex);
                Vector3d edgeCubePos = FaceMatrix.MultiplyVector(edgeVertex);

                // edgeCubePos.x = Mathd.Round(edgeCubePos.x * 10000000) * 0.0000001f;
                // edgeCubePos.y = Mathd.Round(edgeCubePos.y * 10000000) * 0.0000001f;
                // edgeCubePos.z = Mathd.Round(edgeCubePos.z * 10000000) * 0.0000001f;

                // Vector3 spherePos = GridHelperScript.GetCubeToSpherePosition(cubePos);
                // Vector3 edgeSpherePos = GridHelperScript.GetCubeToSpherePosition(edgeCubePos);

                Vector3d spherePosD = GridHelperScript.GetCubeToSpherePosition(new Vector3d(cubePos.x, cubePos.y, cubePos.z));
                Vector3d edgeSpherePosD = GridHelperScript.GetCubeToSpherePosition(new Vector3d(edgeCubePos.x, edgeCubePos.y, edgeCubePos.z));

                Vector3d uvh = cuboidHM.GetHeightValue(cubePos, FaceType, edgeLength);
                Vector3d edgeUvh = cuboidHM.GetHeightValue(edgeCubePos, FaceType, edgeLength);

                Vector3d finalSpherePos = new Vector3d();

                if(x > 0 && z > 0 && x < Divisions + divOffsetMinusOne && z < Divisions + divOffsetMinusOne)
                {
                    finalSpherePos = (spherePosD * (1 + uvh.z) * radius);
                }
                else
                {
                    finalSpherePos = spherePosD * (1 + uvh.z - edgeLength) * radius;
                }

                Vector3d finalEdgeSpherePos = edgeSpherePosD * (1 + edgeUvh.z) * radius;

                VertexBuffer[idx] = new Vector3((float)finalSpherePos.x, (float)finalSpherePos.y, (float)finalSpherePos.z);
                edgeVertices[idx] = new Vector3((float)finalEdgeSpherePos.x, (float)finalEdgeSpherePos.y, (float)finalEdgeSpherePos.z);
                
                TexcoordBuffer[idx] = new Vector2((float)uvh.x, (float)uvh.y);
                tileUVs[idx] = new Vector2((float)(edgeLength + (float)x / (float)(Divisions)), 
                                           (float)(edgeLength + (float)z / (float)Divisions));
            }
        }

        // Calculate Normals
        for(int i = 0; i < IndexBuffer.Length; i += 3)
        {
            int i0 = IndexBuffer[i];
            int i1 = IndexBuffer[i + 1];
            int i2 = IndexBuffer[i + 2];

            Vector3 v0 = edgeVertices[i0];
            Vector3 v1 = edgeVertices[i1];
            Vector3 v2 = edgeVertices[i2];

            Vector3 u = v1 - v0;
            Vector3 v = v2 - v0;

            Vector3 n = Vector3.Cross(u, v);

            NormalBuffer[i0] += n;
            NormalBuffer[i1] += n;
            NormalBuffer[i2] += n;        
        }

        for(int i = 0; i < NormalBuffer.Length; i++)
        {
            NormalBuffer[i] = NormalBuffer[i].normalized;
        }

        // Calculate Tangents
        Vector3[] tan1 = new Vector3[VertexBuffer.Length];
        Vector3[] tan2 = new Vector3[VertexBuffer.Length];
        Vector4[] tangents = new Vector4[VertexBuffer.Length];

        for(int a = 0; a < IndexBuffer.Length; a += 3)
        {
            int i1 = IndexBuffer[a + 0];
            int i2 = IndexBuffer[a + 1];
            int i3 = IndexBuffer[a + 2];

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

                Vector3 n = NormalBuffer[vtxIdx];
                Vector3 t = tan1[vtxIdx];
                Vector3.OrthoNormalize(ref n, ref t);
                tangents[vtxIdx].x = t.x;
                tangents[vtxIdx].y = t.y;
                tangents[vtxIdx].z = t.z;
                tangents[vtxIdx].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[vtxIdx]) < 0.0f) ? -1.0f : 1.0f;
                TangentBuffer[vtxIdx] = tangents[vtxIdx];
            }
        }

        State = GridGeometryStates.ISREADY;
    }

    public void Prepare(Camera sceneCamera, GridMeshScript gridMesh, float radius, Transform player,
                        Matrix4x4 planetMatrix)
    {
        gridMesh.Prepare(sceneCamera, VertexBuffer, NormalBuffer, TexcoordBuffer, TangentBuffer, IndexBuffer,
                         this.BBCenter, this.Size, (radius / 1.45f), FaceType, this.Divisions, 3, player,
                         planetMatrix, this.LODIndex);
        State = GridGeometryStates.RENDER;
    }    
}
