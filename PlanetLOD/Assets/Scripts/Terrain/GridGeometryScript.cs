using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GridMeshScript()
    {
        MeshObj = new Mesh();
    }

    public void ComputeBoundingBox()
    {
        Bounds bounds = BoundingBox;
                        
        Vector3 v3Center = bounds.center;
        Vector3 v3Extents = bounds.extents;
    
        v3FrontTopLeft     = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        v3FrontTopRight    = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        v3FrontBottomLeft  = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        v3BackTopLeft      = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        v3BackTopRight     = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        v3BackBottomLeft   = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        v3BackBottomRight  = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner
            
        // v3FrontTopLeft     = transform.TransformPoint(v3FrontTopLeft);
        // v3FrontTopRight    = transform.TransformPoint(v3FrontTopRight);
        // v3FrontBottomLeft  = transform.TransformPoint(v3FrontBottomLeft);
        // v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        // v3BackTopLeft      = transform.TransformPoint(v3BackTopLeft);
        // v3BackTopRight     = transform.TransformPoint(v3BackTopRight);
        // v3BackBottomLeft   = transform.TransformPoint(v3BackBottomLeft);
        // v3BackBottomRight  = transform.TransformPoint(v3BackBottomRight);
    }

    public void DrawBoundingBox(Color color)
    {
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
                        Vector3 bbCenter, float size, float radius, GridFaceType faceType)
    {
        FaceType = faceType;

        float distToCenter = (sceneCamera.farClipPlane - sceneCamera.nearClipPlane) / 2.0f;
        Vector3 center = sceneCamera.transform.position + sceneCamera.transform.forward * distToCenter;
        float extremeBound = 1000000;
        MeshObj.bounds = new Bounds(center, Vector3.one * extremeBound);

        BoundingBox = new Bounds(bbCenter, new Vector3(size * radius, size * radius, size * radius));

    //    this.ComputeBoundingBox();

        MeshObj.vertices = vertexBuffer;
        MeshObj.normals = normalBuffer;
        MeshObj.uv = texcoordBuffer;
        MeshObj.tangents = tangentBuffer;
        MeshObj.triangles = indexBuffer;
    }

    public void Render(Material gridMaterial)
    {
        if(MeshObj != null)
        {
            Graphics.DrawMesh(MeshObj, Matrix4x4.identity, gridMaterial, 0);
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
    public Matrix4x4 FaceMatrix;

    public GridGeometryStates State;
    public int ID;

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
                if(FaceType == GridFaceType.FRONT || FaceType == GridFaceType.BACK)
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

    public void Process(float radius, CuboidHeightMapScript cuboidHM, DebuggerScript debugger)
    {
        Vector3 orientationAngles = GridHelperScript.GetOrientationAngles(FaceType);
        FaceMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(orientationAngles), Vector3.one);

        float edgeLength = Size / (float)Divisions;
        float halfSize = Size / 2 + edgeLength;
        int divOffset = 3;
        int divOffsetMinusOne =  divOffset - 1; 

        List<Vector3> edgeVertices = new List<Vector3>();
        edgeVertices.AddRange(VertexBuffer);

        List<Vector2> tileUVs = new List<Vector2>();
        tileUVs.AddRange(TexcoordBuffer);

   //     List<Matrix4x4> jacobianMatrices = new List<Matrix4x4>();
   //     this.ConstructIndexBuffer(Divisions, divOffset, divOffsetMinusOne);
     //   Center.y += 0.1f;
       this.FixIndexBuffer(Divisions, divOffset, divOffsetMinusOne);

  //      Debug.Log("Grid Face -------- " + FaceType);

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

                GridFaceType selectedFace = FaceType;
                // float threshold = 0.0f;

                
                // if(FaceType == GridFaceType.TOP || FaceType == GridFaceType.BOTTOM)
                // {
                //     bool isOnXAxis = Mathf.Approximately(Mathf.Abs(edgeVertex.x), 1.0f);
                //     bool isOnZAxis = Mathf.Approximately(Mathf.Abs(edgeVertex.z), 1.0f);

                //     if(isOnXAxis == true && isOnZAxis == false)
                //     {
                //         if(edgeVertex.x > 0)
                //         {
                //             edgeVertex.x = 1.0f;
                //             selectedFace = GridFaceType.RIGHT;
                //         }
                //         else
                //         if(edgeVertex.x < 0)
                //         {
                //             edgeVertex.x = -1.0f;
                //             selectedFace = GridFaceType.LEFT;
                //         }
                //     }
                //     else
                //     if(isOnXAxis == false && isOnZAxis == true)
                //     {
                //         if(edgeVertex.z > 0)
                //         {
                //             edgeVertex.z = 1;
                //             selectedFace = GridFaceType.FRONT;
                //         }
                //         else
                //         if(edgeVertex.z < 0)
                //         {
                //             edgeVertex.z = -1;
                //             selectedFace = GridFaceType.BACK;
                //         }
                //     }
                // }
                // else
                // if(FaceType == GridFaceType.RIGHT || FaceType == GridFaceType.LEFT)
                // {
                //     bool isOnXAxis = Mathf.Approximately(Mathf.Abs(edgeVertex.x), 1.0f);
                //     bool isOnZAxis = Mathf.Approximately(Mathf.Abs(edgeVertex.z), 1.0f);

                //     if(isOnXAxis == true && isOnZAxis == false)
                //     {
                //         if(edgeVertex.x > 0)
                //         {
                //             selectedFace = GridFaceType.BOTTOM;
                //             edgeVertex.x = 1.0f;
                //         }
                //         else
                //         if(edgeVertex.x < 0)
                //         {
                //             selectedFace = GridFaceType.TOP;
                //             edgeVertex.x = -1.0f;
                //         }
                //     }
                //     else
                //     if(isOnXAxis == false && isOnZAxis == true)
                //     {
                //         if(edgeVertex.z > 0)
                //         {
                //             selectedFace = GridFaceType.FRONT;
                //             edgeVertex.z = 1.0f;
                //         }
                //         else
                //         if(edgeVertex.z < 0)
                //         {
                //             selectedFace = GridFaceType.BACK;
                //             edgeVertex.z = -1.0f;
                //         }
                //     }
                // }              


                Vector3 cubePos = FaceMatrix.MultiplyVector(vertex);
                Vector3 edgeCubePos = FaceMatrix.MultiplyVector(edgeVertex);

                // if(FaceType == GridFaceType.RIGHT)
                // if(edgeCubePos.x < 0.5f)
                // {
                //     edgeCubePos.x = 0.5f; 
                // }

                edgeCubePos.x = Mathf.Round(edgeCubePos.x * 10000) * 0.0001f;
                edgeCubePos.y = Mathf.Round(edgeCubePos.y * 10000) * 0.0001f;
                edgeCubePos.z = Mathf.Round(edgeCubePos.z * 10000) * 0.0001f;

                // if(FaceType == GridFaceType.TOP)
                // {
                //     if(x == Divisions + divOffsetMinusOne)
                //     {
                //         Debug.Log("edgeCubePos : " + edgeCubePos.ToString("F8"));
                //     }
                // }
                // else
                // if(FaceType == GridFaceType.RIGHT)
                // {
                //     if(x == 0)
                //     {
                //         bool isApprox = Mathf.Approximately(edgeCubePos.x, 1.0f); 

                //         // if(isApprox == true)
                //         // {
                //         //     edgeCubePos.x = 1.0f;
                //         // }
                        

                //         Debug.Log("edgeCubePos : " + edgeCubePos.ToString("F8") + " : isApprox x : 1 " + isApprox);
                //     }
                // }

//                string edgeCubePosStr = edgeCubePos.ToString("F6");



                Vector3 spherePos = GridHelperScript.GetCubeToSpherePosition(cubePos);
                Vector3 edgeSpherePos = GridHelperScript.GetCubeToSpherePosition(edgeCubePos);

                Vector3 uvh = cuboidHM.GetHeightValue(cubePos, FaceType, edgeLength);
                Vector3 edgeUvh = new Vector3();



                edgeUvh = cuboidHM.GetHeightValue(edgeCubePos, selectedFace, edgeLength);


                // if(x > 2 && z > 2 && x < Divisions + divOffsetMinusOne - 2 && z < Divisions + divOffsetMinusOne - 2)
                // {

                // }
                // else
                // {

                    // edgeSpherePos.x = Mathf.Floor(edgeSpherePos.x);
                    // edgeSpherePos.y = Mathf.Floor(edgeSpherePos.y);
                    // edgeSpherePos.z = Mathf.Floor(edgeSpherePos.z);

                    // if(FaceType == GridFaceType.TOP)
                    // {
                    //     if(x == Divisions + divOffsetMinusOne - 2)
                    //     {
                    //         Debug.Log("edgeSpherePos : " + edgeSpherePos.ToString("F4") + " : edgeCubePos : " + edgeCubePos.ToString("F8") + " : idx : " + idx + " : height : " + edgeUvh.z + " : edgeVertex : " + edgeVertex.ToString("F8"));
                            
                    //         Vector3 fval = edgeSpherePos * (1 + edgeUvh.z) * radius;

                    //         fval.x = Mathf.Floor(fval.x);
                    //         fval.y = Mathf.Floor(fval.y);
                    //         fval.z = Mathf.Floor(fval.z);

                    //         debugger.TopGridPositions.Add(fval);
                    //     }
                    // }
                    // else
                    // if(FaceType == GridFaceType.RIGHT)
                    // {
                    //     if(x == 0)
                    //     {
                    //         Debug.Log("edgeSpherePos : " + edgeSpherePos.ToString("F4") + " : edgeCubePos : " + edgeCubePos.ToString("F8") + " : idx : " + idx + " : height : " + edgeUvh.z + " : edgeVertex : " + edgeVertex.ToString("F8"));
                    //         Vector3 fval = edgeSpherePos * (1 + edgeUvh.z) * radius;                            

                    //         fval.x = Mathf.Floor(fval.x);
                    //         fval.y = Mathf.Floor(fval.y);
                    //         fval.z = Mathf.Floor(fval.z);

                    //         debugger.RightGridPositions.Add(fval);
                    //     }
                    // }
            //    }


                // if(FaceType == GridFaceType.TOP)
                // {
                //     if(x == 0)
                //     {
                //         cuboidHM.GetHeightValue(edgeCubePos, GridFaceType.LEFT, edgeLength);
                //     }

                //     if(z == 0)
                //     {
                //         cuboidHM.GetHeightValue(edgeCubePos, GridFaceType.FRONT, edgeLength);
                //     }

                //     if(x == Divisions + divOffsetMinusOne)
                //     {
                //         cuboidHM.GetHeightValue(edgeCubePos, GridFaceType.RIGHT, edgeLength);
                //     }

                //     if(z == Divisions + divOffsetMinusOne)
                //     {
                //         cuboidHM.GetHeightValue(edgeCubePos, GridFaceType.BACK, edgeLength);
                //     }
                // }
                
                // if(x > 0 && z > 0 && x < Divisions + divOffsetMinusOne && z < Divisions + divOffsetMinusOne)
                // {

                // }

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

                // if(FaceType == GridFaceType.BACK)
                // {
                //     VertexBuffer[idx] = finalEdgeSpherePos;
                // }
                // else
                {
                    VertexBuffer[idx] = finalSpherePos;
                }
                edgeVertices[idx] = finalEdgeSpherePos;
                
                // GridMesh.NormalBuffer[idx] = Vector3.up;
                TexcoordBuffer[idx] = new Vector2(uvh.x, uvh.y);
                tileUVs[idx] = new Vector2(edgeLength + (float)x / (float)(Divisions), 
                                           edgeLength + (float)z / (float)Divisions);

                // float lenW = Mathf.Sqrt(uvh.x * uvh.x + uvh.y * uvh.y + 1);
                // float h = uvh.z;

            //    Matrix4x4 jacobianMatrix = GridHelperScript.GetJacobianMatrix(h, lenW, uvh.x, uvh.y);
                // GridMesh.TangentBuffer[idx] = new Vector4();    
            //    jacobianMatrices.Add(jacobianMatrix);
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
   //     Vector3[] norm = new Vector3[VertexBuffer.Length];
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

            // norm[i1] += Vector3.Cross(tan1[i1], tan2[i1]);
            // norm[i2] += Vector3.Cross(tan1[i2], tan2[i2]);
            // norm[i3] += Vector3.Cross(tan1[i3], tan2[i3]);

        }

        int vtxIdx = 0;

        for (int z = 0; z < Divisions + divOffset; z++)
        {
            for (int x = 0; x < Divisions + divOffset; x++)
            {
                vtxIdx = x + z * (Divisions + divOffset);

                // Matrix4x4 jacobianMat = GridHelperScript.GetJacobianMatrix()

             //   NormalBuffer[vtxIdx] = norm[vtxIdx].normalized;
                Vector3 n = NormalBuffer[vtxIdx];//norm[vtxIdx];
                Vector3 t = tan1[vtxIdx];
                Vector3.OrthoNormalize(ref n, ref t);
                tangents[vtxIdx].x = t.x;
                tangents[vtxIdx].y = t.y;
                tangents[vtxIdx].z = t.z;
                tangents[vtxIdx].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[vtxIdx]) < 0.0f) ? -1.0f : 1.0f;
                TangentBuffer[vtxIdx] = tangents[vtxIdx];
            //    NormalBuffer[vtxIdx] = Vector3.Cross(tan1[vtxIdx], tan2[vtxIdx]);//n.normalized;//norm[vtxIdx].normalized;
            }
        }

  //      gridMesh.Center = Center;

        State = GridGeometryStates.ISREADY;
    }

    public void Prepare(Camera sceneCamera, GridMeshScript gridMesh, float radius)
    {
     //   GridMesh.Prepare(sceneCamera);
        gridMesh.Prepare(sceneCamera, VertexBuffer, NormalBuffer, TexcoordBuffer, TangentBuffer, IndexBuffer,
                         this.BBCenter, this.Size, (radius / 1.45f), FaceType);
        State = GridGeometryStates.RENDER;
    }

    public void Render()
    {
     //   Graphics.DrawMesh(GridMesh.Mesh, Matrix4x4.identity, GridMaterial, 0);
    }

    
}
