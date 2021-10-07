using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridNodeStates
{
    MERGE = 0,
    SPLIT = 1
};

public enum GridNodeTypes
{
    ROOT = 0,
    NORTHWEST = 1,
    NORTHEAST = 2,
    SOUTHWEST = 3,
    SOUTHEAST = 4
};

public class GridNodeScript
{
    public GridNodeScript Parent;
    public GridNodeScript[] Children; // Order (0)NW, (1)NE, 2(SW), 3(SE)

    public float Size;
    public int LODIndex;
    public int GridIndex;
    public Vector3 Center;

    public GridNodeStates State;
    public GridNodeTypes Type;

    public GridNodeScript(GridNodeScript parent, GridNodeTypes type, Vector3 center, float size)
    {
        Type = type;
        State = GridNodeStates.MERGE;
        GridIndex = -1;

        Children = new GridNodeScript[4];

        for(int i = 0; i < Children.Length; i++)
        {
            Children[i] = null;
        }

        if(Type == GridNodeTypes.ROOT)
        {
            Center = center;
            Size = size;
            LODIndex = 0;
            Parent = null;
        }
        else
        {
            Parent = parent;

            if(Parent == null)
            {
                return;
            }

            float quarterSize = Parent.Size / 4;
            this.Size = Parent.Size / 2;
            this.LODIndex = Parent.LODIndex + 1;

            if (Type == GridNodeTypes.NORTHWEST)
            {
                this.Center = Parent.Center + new Vector3(-quarterSize, 0, quarterSize);
            }
            else
            if (Type == GridNodeTypes.NORTHEAST)
            {
                this.Center = Parent.Center + new Vector3(quarterSize, 0, quarterSize);
            }
            else
            if (Type == GridNodeTypes.SOUTHWEST)
            {
                this.Center = Parent.Center + new Vector3(-quarterSize, 0, -quarterSize);
            }
            else
            if (Type == GridNodeTypes.SOUTHEAST)
            {
                this.Center = Parent.Center + new Vector3(quarterSize, 0, -quarterSize);
            }            
        }
    }

    public void Update(GridNodeScript parent, GridNodeScript thisNode, Vector3 cameraPosition, 
                       float finalResolution, int maxDepth, int divisions, GridFaceType faceType,
                       float radius, GridPoolScript gridPool)
    {
        if(thisNode == null)
        {
            return;
        }
        else
        {
            Vector3 orientationAngles = GridHelperScript.GetOrientationAngles(faceType);
            Matrix4x4 faceMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(orientationAngles), Vector3.one);
            Vector3 faceCenter = faceMatrix.MultiplyVector(thisNode.Center);

            Vector3 newCenter = GridHelperScript.GetCubeToSpherePosition(faceCenter) * radius;

        //    Debug.Log("newCenter : " + newCenter + " : LODIndex : " + LODIndex);

            float viewDistance = (Mathf.Abs(cameraPosition.x - newCenter.x) + 
                                  Mathf.Abs(cameraPosition.y - newCenter.y) + 
                                  Mathf.Abs(cameraPosition.z - newCenter.z));
            float f = viewDistance / (thisNode.Size * radius * finalResolution);
            if(f < 0.1f)
            {
                thisNode.State = GridNodeStates.SPLIT;
            }
            else
            {
                thisNode.State = GridNodeStates.MERGE;
            }

            if(thisNode.LODIndex == maxDepth - 1)
            {
                thisNode.State = GridNodeStates.MERGE;
            }

            if(thisNode.State == GridNodeStates.SPLIT)
            {
                // Perform action with LOD Container
                if(thisNode.GridIndex != -1)
                {
                    gridPool.Container[thisNode.GridIndex].State = GridGeometryStates.AVAILABLE;
                //    gridPool.AvailableQueue.Enqueue(gridPool.Container[thisNode.GridIndex]);
                    thisNode.GridIndex = -1;
                }

                // Spawn Children
                for(int i = 0; i < 4; i++)
                {
                    if(thisNode.Children[i] == null)
                    {
                        GridNodeTypes type = (GridNodeTypes)(i + 1);
                        thisNode.Children[i] = new GridNodeScript(thisNode, type, thisNode.Center, thisNode.Size);
                    }
                    thisNode.Update(thisNode, thisNode.Children[i], cameraPosition, finalResolution, maxDepth, divisions, faceType, radius, gridPool);
                }
            }
            else
            if(thisNode.State == GridNodeStates.MERGE)
            {
                // Destroy Children and remove from grid lod container
                for(int i = 0; i < 4; i++)
                {
                    if(thisNode.Children[i] != null)
                    {
                        if(thisNode.Children[i].GridIndex != -1)
                        {
                           gridPool.Container[thisNode.Children[i].GridIndex].State = GridGeometryStates.AVAILABLE;
                        //    gridPool.AvailableQueue.Enqueue(gridPool.Container[thisNode.Children[i].GridIndex]);
                            thisNode.Children[i].GridIndex = -1;
                        }

                        thisNode.Children[i] = null;
                    }
                }

                // Construct node mesh
                if(thisNode.GridIndex == -1)
                {
                    for(int i = 0; i < gridPool.Container.Count; i++)
                    {
                        if(gridPool.Container[i].State == GridGeometryStates.AVAILABLE)
                        {
                            thisNode.GridIndex = i;
                            gridPool.Container[i].Size = thisNode.Size;
                            gridPool.Container[i].Center = thisNode.Center;
                            
                            gridPool.Container[i].Divisions = divisions;
                            gridPool.Container[i].FaceType = faceType;
                            gridPool.Container[i].State = GridGeometryStates.INPROCESS;
                            gridPool.ProcessQueue.Enqueue(gridPool.Container[i]);
                            // gridPool.ReadyList.Add(gridPool.Container[i]);
                            break;
                        }
                    }
                }
            }
        }
    }
}
