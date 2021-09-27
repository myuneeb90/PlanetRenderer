using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFaceScript
{
    public GridNodeScript RootNode;

 //   public List<GridLODScript> GridLODs;

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

        RootNode = new GridNodeScript(null, GridNodeTypes.ROOT, Vector3.zero, size);

        // GridLODs = new List<GridLODScript>();
    
        // for(int i = 0; i < lodDepth; i++)
        // {
        //     GridLODs.Add(new GridLODScript(i, size, divisions, material, FaceType));
        // }
    }

    public void Update(Vector3 cameraPosition, GridPoolScript gridPool)
    {
        RootNode.Update(null, RootNode, cameraPosition, FinalResolution, LODDepth, Divisions, FaceType, gridPool);
    
        // for(int i = 0; i < gridPool.Container.Count; i++)
        // {
        //     gridPool.Container[i].Update();
        // }    
    }

    // public void Render(GridPoolScript gridPool)
    // {
    //     for(int i = 0; i < gridPool.Container.Count; i++)
    //     {
    //         gridPool.Container[i].Render();
    //     }
    // }
}
