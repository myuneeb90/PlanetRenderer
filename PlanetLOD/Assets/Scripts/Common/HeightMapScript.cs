using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapScript
{
    public float[,] Data;
    public int Width;
    public int Height;

    public HeightMapScript(int width, int height, Texture2D hmTexture)
    {
        Width = width;
        Height = height;

        Data = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Data[x, y] = hmTexture.GetPixel(x, y).r;
            }
        }        
    }

    public float GetHeightValue(int x, int y)
    {
    //    return Data[x % (Width - 1), y % (Height - 1)];
        return Data[x, y];
    }
}
