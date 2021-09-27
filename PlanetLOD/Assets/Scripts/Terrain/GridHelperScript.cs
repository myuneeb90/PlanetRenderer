using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelperScript
{
    public static Vector3 GetOrientationAngles(GridFaceType faceType)
    {
        Vector3 orientationAngles = new Vector3();

        if(faceType == GridFaceType.TOP)
        {
            orientationAngles = new Vector3(0, 0, 0);
        }
        else
        if(faceType == GridFaceType.BOTTOM)
        {
            orientationAngles = new Vector3(0, 0, 180);
        }
        else
        if(faceType == GridFaceType.RIGHT)
        {
            orientationAngles = new Vector3(0, 0, -90);
        }
        else
        if(faceType == GridFaceType.LEFT)
        {
            orientationAngles = new Vector3(0, 0, 90);
        }
        else
        if(faceType == GridFaceType.FRONT)
        {
            orientationAngles = new Vector3(90, 0, 0);
        }
        else
        if(faceType == GridFaceType.BACK)
        {
            orientationAngles = new Vector3(-90, 0, 0);
        } 

        return orientationAngles;
    }

    public static Vector3 GetCubeToSpherePosition(Vector3 v)
    {
        float x = v.x * Mathf.Sqrt(1 - ((v.y * v.y) / 2) - ((v.z * v.z) / 2) + (((v.y * v.y) * (v.z * v.z)) / 3));
        float y = v.y * Mathf.Sqrt(1 - ((v.z * v.z) / 2) - ((v.x * v.x) / 2) + (((v.z * v.z) * (v.x * v.x)) / 3));
        float z = v.z * Mathf.Sqrt(1 - ((v.x * v.x) / 2) - ((v.y * v.y) / 2) + (((v.x * v.x) * (v.y * v.y)) / 3));

        return new Vector3(x, y, z);
    } 
}
