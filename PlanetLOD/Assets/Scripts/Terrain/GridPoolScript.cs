using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class RenderableGridScript
// {
//     public int ID;
// }

public class GridMeshColliderScript
{
    public bool IsEmpty = false;
    public Vector3 Center;
    public MeshCollider Collider;

    public GridMeshColliderScript(MeshCollider collider)
    {
        Collider = collider;
        IsEmpty = true;
    }
}

public class GridPoolScript
{
    public List<GridGeometryScript> Container;
    public List<GridMeshScript> GridMeshContainer;

    // public GridMeshScript ColliderMesh;
    // public GridGeometryScript ColliderGeometry;

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

    public void Process(float radius, CuboidPrecisionHeightMapScript cuboidHM, DebuggerScript debugger)
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

    public void Prepare(Camera sceneCamera, float radius, Transform player, Matrix4x4 planetMatrix,
                        List<GridMeshColliderScript> colliders, float colliderRange, int lodDepth)
    {
        int prepareCount = PrepareQueue.Count;
        while(PrepareQueue.Count != 0)
        {
            GridGeometryScript grid = PrepareQueue.Dequeue();
            grid.Prepare(sceneCamera, GridMeshContainer[grid.ID], radius, player, planetMatrix);
        }

        if(prepareCount > 0)
        {
            RenderList.Clear();
            RenderList.AddRange(ReadyList);
            ReadyList.Clear();
        
//            FrustumPlanes = GeometryUtility.CalculateFrustumPlanes(sceneCamera);

            for(int i = 0; i < colliders.Count; i++)
            {
                if(colliders[i].IsEmpty == false)
                {
                    float distance = Vector3.Distance(colliders[i].Center, sceneCamera.transform.position);
                    if(distance > colliderRange)
                    {
                        colliders[i].IsEmpty = true;
                        colliders[i].Collider.sharedMesh = null;
                    }
                }
            }

            for(int i = 0; i < RenderList.Count; i++)
            {
                GridMeshScript gridMesh = GridMeshContainer[RenderList[i]];
                float distance = Vector3.Distance(gridMesh.Center, sceneCamera.transform.position);            
                if(gridMesh.LODIndex == (lodDepth - 1) && distance <= colliderRange)// && GeometryUtility.TestPlanesAABB(this.FrustumPlanes, gridMesh.BoundingBox))
                {
                    for(int j = 0; j < colliders.Count; j++)
                    {
                        if(colliders[j].IsEmpty == true)
                        {
                            colliders[j].Collider.sharedMesh = gridMesh.MeshObj;
                            colliders[j].IsEmpty = false;
                            colliders[j].Center = gridMesh.Center;
                            break;
                        }
                    }
                }
            }          
        }
    }

    public void Render(List<Material> gridMaterials, Camera sceneCamera, Matrix4x4 planetMatrix, Vector3 planetPosition)
    {
        FrustumPlanes = GeometryUtility.CalculateFrustumPlanes(sceneCamera);

        for(int i = 0; i < RenderList.Count; i++)
        {
            GridMeshScript gridMesh = GridMeshContainer[RenderList[i]];

       //     gridMesh.UpdateBoundingBox(planetMatrix, planetPosition);
         //   if (GeometryUtility.TestPlanesAABB(this.FrustumPlanes, gridMesh.BoundingBox))
            {            
                gridMesh.Render(gridMaterials[(int)gridMesh.FaceType], planetMatrix);
            //    gridMesh.DrawBoundingBox(Color.green);
            }
        }
    }
}
