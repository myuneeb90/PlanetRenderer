using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoolScript
{
    public List<GridGeometryScript> Container;

    public Queue<GridGeometryScript> PrepareQueue;
    public Queue<GridGeometryScript> AvailableQueue;
    public Queue<GridGeometryScript> ProcessQueue;

    public List<GridGeometryScript> ReadyList;
    public List<GridGeometryScript> RenderList;
    public int ProcessCount = 0;

    public GridPoolScript(int gridCount, float size, int divisions, Material material)
    {
        Container = new List<GridGeometryScript>();

        for(int i = 0; i < gridCount; i++)
        {
            Container.Add(new GridGeometryScript(size, divisions, material));
        }           

        PrepareQueue = new Queue<GridGeometryScript>();
        AvailableQueue = new Queue<GridGeometryScript>();
        ProcessQueue = new Queue<GridGeometryScript>();
        ReadyList = new List<GridGeometryScript>();
        RenderList = new List<GridGeometryScript>();
    }

    public void Process(float radius, CuboidHeightMapScript cuboidHM)
    {
    //    Debug.Log("Process Count : " + ProcessCount);

    //     while(AvailableQueue.Count != 0)
    //     {
    //         GridGeometryScript grid = AvailableQueue.Dequeue();
        
    //    //     Debug.Log("grid.State in Available Queue : " + grid.State);
    //         grid.State = GridGeometryStates.AVAILABLE;
    //     }

        while(ProcessQueue.Count != 0)
        {
            GridGeometryScript grid = ProcessQueue.Dequeue();
            grid.Process(radius, cuboidHM);
            PrepareQueue.Enqueue(grid);
            ProcessCount++;
        }

        // for(int i = 0; i < Container.Count; i++)
        // {
        //     if(Container[i].State == GridGeometryStates.INPROCESS)
        //     {
        //         Container[i].Process(radius, cuboidHM);
        //         PrepareQueue.Enqueue(Container[i]);
        //         ProcessCount++;
        //     }
        // }

        if(ReadyList.Count > 0)
        {
            RenderList.Clear();
            RenderList.AddRange(ReadyList);
            ReadyList.Clear();
        }
    }

    public void Prepare(Camera sceneCamera)
    {
   //     Debug.Log("PrepareQueue.Count : " + PrepareQueue.Count);

        while(PrepareQueue.Count != 0)
        {
            GridGeometryScript grid = PrepareQueue.Dequeue();
            grid.Prepare(sceneCamera);
        }

    //    if(PrepareQueue.Count > 0)
        // {
        //     RenderList.Clear();
        //     for(int i = 0; i < Container.Count; i++)
        //     {
        //         if(Container[i].State == GridGeometryStates.RENDER)
        //         {
        //             RenderList.Add(Container[i]);                 
        //         }
        //     }            
        // }




        // for(int i = 0; i < Container.Count; i++)
        // {
        //     if(Container[i].State == GridGeometryStates.ISREADY)
        //     {
        //         Container[i].Prepare(sceneCamera);
        //     }
        // } 
    }

    public void Render()
    {
        for(int i = 0; i < RenderList.Count; i++)
        {
         //   if(RenderList[i].State == GridGeometryStates.RENDER)
            {
                RenderList[i].Render();
            }
        }
        // for(int i = 0; i < Container.Count; i++)
        // {
        //     if(Container[i].State == GridGeometryStates.RENDER)
        //     {
        //         Container[i].Render();
        //     }
        // }
    }
}
