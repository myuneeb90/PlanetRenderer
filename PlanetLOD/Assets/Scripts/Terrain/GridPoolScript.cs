using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class RenderableGridScript
// {
//     public int ID;
// }

public class GridPoolScript
{
    public List<GridGeometryScript> Container;
    public List<GridMeshScript> GridMeshContainer;

    public Queue<GridGeometryScript> PrepareQueue;
    public Queue<GridGeometryScript> ProcessQueue;

    public List<int> ReadyList;
    public List<int> RenderList;

    public Plane[] FrustumPlanes; 

    public int ProcessCount = 0;

    public GridPoolScript(int gridCount, float size, int divisions)
    {
        Container = new List<GridGeometryScript>();
        GridMeshContainer = new List<GridMeshScript>();

        for(int i = 0; i < gridCount; i++)
        {
            Container.Add(new GridGeometryScript(size, divisions, i));
            GridMeshScript gridMesh = new GridMeshScript();
            GridMeshContainer.Add(gridMesh);
        }           

        PrepareQueue = new Queue<GridGeometryScript>();
        ProcessQueue = new Queue<GridGeometryScript>();
        ReadyList = new List<int>();
        RenderList = new List<int>();
    }

    public void Process(float radius, CuboidHeightMapScript cuboidHM, DebuggerScript debugger)
    {
        while(ProcessQueue.Count != 0)
        {
            GridGeometryScript grid = ProcessQueue.Dequeue();
            grid.Process(radius, cuboidHM, debugger);
            PrepareQueue.Enqueue(grid);
            ProcessCount++;
        }

        if(PrepareQueue.Count > 0)
        {
            ReadyList.Clear();
            for(int i = 0; i < Container.Count; i++)
            {
                if(Container[i].State == GridGeometryStates.RENDER ||
                   Container[i].State == GridGeometryStates.ISREADY)
                {
                    ReadyList.Add(Container[i].ID);                 
                }
            }
        }    
    }

    public void Prepare(Camera sceneCamera, float radius)
    {
        int prepareCount = PrepareQueue.Count;
        while(PrepareQueue.Count != 0)
        {
            GridGeometryScript grid = PrepareQueue.Dequeue();
            grid.Prepare(sceneCamera, GridMeshContainer[grid.ID], radius);
        }

        if(prepareCount > 0)
        {
            RenderList.Clear();
            RenderList.AddRange(ReadyList);
            ReadyList.Clear();
        }
    }

    public void Render(List<Material> gridMaterials, Camera sceneCamera)
    {
        FrustumPlanes = GeometryUtility.CalculateFrustumPlanes(sceneCamera);

        for(int i = 0; i < RenderList.Count; i++)
        {
            GridMeshScript gridMesh = GridMeshContainer[RenderList[i]];

         //   if (GeometryUtility.TestPlanesAABB(this.FrustumPlanes, gridMesh.BoundingBox))
            {            
           //     if(gridMesh.FaceType == GridFaceType.TOP || gridMesh.FaceType == GridFaceType.RIGHT)// ||
//                   gridMesh.FaceType == GridFaceType.BACK)
                gridMesh.Render(gridMaterials[(int)gridMesh.FaceType]);
            //    gridMesh.DrawBoundingBox(Color.green);
            }
        }
    }
}
