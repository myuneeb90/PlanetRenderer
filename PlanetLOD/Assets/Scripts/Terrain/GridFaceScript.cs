using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFaceScript
{
    public GridNodeScript RootNode;
    private GridFaceType FaceType;
    private float FinalResolution;
    private int Divisions;
    private int LODDepth;

    public GridFaceScript(int lodDepth, float size, int divisions, Material material, GridFaceType faceType)
    {
        float minRes = 9.0f;
        float desiredRes = 11.0f;
        FinalResolution = minRes * Mathf.Max(desiredRes / 3, 1);

        LODDepth = lodDepth;
        FaceType = faceType;
        Divisions = divisions;

        RootNode = new GridNodeScript(null, GridNodeTypes.ROOT, new Vector3(0, 1, 0), size);
    }

    public void Update(Vector3 cameraPosition, float radius, GridPoolScript gridPool, Matrix4x4 planetMatrix)
    {
        RootNode.Update(null, RootNode, cameraPosition, FinalResolution, LODDepth, Divisions, 
                        FaceType, radius, gridPool, planetMatrix);
    }
}
