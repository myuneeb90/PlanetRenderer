using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoolScript
{
    public List<GridGeometryScript> Container;
    public int ProcessCount = 0;

    public GridPoolScript(int gridCount, float size, int divisions, Material material)
    {
        Container = new List<GridGeometryScript>();

        for(int i = 0; i < gridCount; i++)
        {
            Container.Add(new GridGeometryScript(size, divisions, material));
        }           
    }

    public void Process(float radius, CuboidHeightMapScript cuboidHM)
    {
    //    Debug.Log("Process Count : " + ProcessCount);
        for(int i = 0; i < Container.Count; i++)
        {
            if(Container[i].State == GridGeometryStates.INPROCESS)
            {
                Container[i].Process(radius, cuboidHM);
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
