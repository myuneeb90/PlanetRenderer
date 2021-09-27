using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoolScript
{
    public List<GridGeometryScript> Container;
    public int ProcessCount = 0;

    public GridPoolScript(int gridCount, float size, int divisions, Material material)
    {
//        LODDepth = lodDepth;
        Container = new List<GridGeometryScript>();

        for(int i = 0; i < gridCount; i++)
        {
            Container.Add(new GridGeometryScript(size, divisions, material));
        }        

        // if(LODDepth == 0)
        // {
        //     Container.Add(new GridGeometryScript(size, divisions, material, faceType));
        // }
        // else
        // if(LODDepth == 1)
        // {
        //     for(int i = 0; i < 4; i++)
        //     {
        //         Container.Add(new GridGeometryScript(size, divisions, material, faceType));
        //     }
        // }
        // else
        // if(LODDepth == 2)
        // {
        //     for(int i = 0; i < 16; i++)
        //     {
        //         Container.Add(new GridGeometryScript(size, divisions, material, faceType));
        //     }
        // }        
        // else
        // if(LODDepth >= 3)
        // {
        //     for(int i = 0; i < 96; i++)
        //     {
        //         Container.Add(new GridGeometryScript(size, divisions, material, faceType));
        //     }
        // }        
    }

    public void Process()
    {
        Debug.Log("Process Count : " + ProcessCount);
    //    ProcessCount = 0;
        for(int i = 0; i < Container.Count; i++)
        {
            if(Container[i].State == GridGeometryStates.INPROCESS)
            {
                Container[i].Process();
                ProcessCount++;
            }
        } 
    }

    public void Prepare(Camera sceneCamera)
    {
        for(int i = 0; i < Container.Count; i++)
        {
            if(Container[i].State == GridGeometryStates.ISREADY)
            {
                Container[i].Prepare(sceneCamera);
            }
        } 
    }

    public void Render()
    {
        for(int i = 0; i < Container.Count; i++)
        {
            if(Container[i].State == GridGeometryStates.RENDER)
            {
                Container[i].Render();
            }
        }
    }
}
