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

    public static Matrix4x4 GetJacobianMatrix(float height, float length, float u, float v)
    {
        float h = height;
        float w = length;
        float s = u;
        float t = v;

        Matrix4x4 mat = new Matrix4x4();

        Vector3 ts = new Vector3(h / (w * (1 - (s * s) / (w * w))), -(s * t * h) / (w * w * w), -(s * h) / (w * w * w));
        Vector3 tt = new Vector3(-(s * t * h) / (w * w * w), h / (w * (1 - (t * t) / (w * w))), -(t * h) / (w * w * w));
        Vector3 th = new Vector3(s / w, t / w, 1 / w);

        mat[0,0] = ts.x; mat[0,1] = ts.y; mat[0,2] = ts.z; mat[0,3] = 0; 
        mat[1,0] = tt.x; mat[1,1] = tt.y; mat[1,2] = tt.z; mat[1,3] = 0; 
        mat[2,0] = th.x; mat[2,1] = th.y; mat[2,2] = th.z; mat[2,3] = 0; 
        mat[3,0] = 0;    mat[3,1] = 0;    mat[3,2] = 0;    mat[3,3] = 1; 

        return mat;
    }
}
