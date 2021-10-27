using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeightValueType
{
    MAIN = 0,
    EDGE = 1,
    CORNER = 2
};

public class PrecisionHeightMapScript
{
    public double[,] Data;
    public float TileX;
    public float TileY;
    public int Width;
    public int Height;

    public PrecisionHeightMapScript(int width, int height, float tileX, float tileY, Texture2D hmTexture)
    {
        Width = width;
        Height = height;
        TileX = tileX;
        TileY = tileY;

        Data = new double[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Data[x, y] = hmTexture.GetPixel(x, y).r;
            }
        } 
    }

    public double GetHeightValue(double x, double y)
    {
  //      return this.ApplyBicubicFilter(x, y);
//        if(vtype == HeightValueType.CORNER || vtype == HeightValueType.EDGE)
//        {
    //    return Data[Mathd.Abs((int)(x * TileX * Width)) % Width, Mathd.Abs((int)(y * TileY * Height)) % Height];
//        }
        // else
        // {
        //    return this.ApplyBilinearFilter(x, y);
            return this.ApplyCubicFilter(x, y);
        // }
      //  double h2 = Data[Mathd.Abs((int)(x * Width)) * (int)4 % Width, Mathd.Abs((int)(y * Height)) * (int)4 % Height];
      //  return (h1 + h2) / 2;// h1 + h2 * 0.25f;//(h1 + h2) / 2;
    }    

    // Bicubic
    private double ApplyCubicFilter(double x, double y)
    {
        int ix = Mathd.Abs((int)(x * TileX * Width));
        int iy = Mathd.Abs((int)(y * TileY * Height));                

        double h0 = Data[Mathd.Abs((ix - 1) % Width),  Mathd.Abs((iy - 1) % Height)];
        double h1 = Data[Mathd.Abs(ix % Width),        Mathd.Abs((iy - 1) % Height)];
        double h2 = Data[Mathd.Abs((ix - 1) % Width),  Mathd.Abs(iy % Height)];
        double h3 = Data[Mathd.Abs(ix % Width),        Mathd.Abs(iy % Height)];

        double h4 = Data[Mathd.Abs((ix + 1) % Width),  Mathd.Abs((iy - 1) % Height)];
        double h5 = Data[Mathd.Abs((ix + 2) % Width),  Mathd.Abs((iy - 1) % Height)];
        double h6 = Data[Mathd.Abs((ix + 1) % Width),  Mathd.Abs(iy % Height)];
        double h7 = Data[Mathd.Abs((ix + 2) % Width),  Mathd.Abs(iy % Height)];

        double h8 = Data[Mathd.Abs((ix - 1) % Width),  Mathd.Abs((iy + 1) % Height)];
        double h9 = Data[Mathd.Abs(ix % Width),        Mathd.Abs((iy + 1) % Height)];
        double h10 = Data[Mathd.Abs((ix - 1) % Width), Mathd.Abs((iy + 2) % Height)];
        double h11 = Data[Mathd.Abs(ix % Width),       Mathd.Abs((iy + 2) % Height)];

        double h12 = Data[Mathd.Abs((ix + 1) % Width), Mathd.Abs((iy + 1) % Height)];
        double h13 = Data[Mathd.Abs((ix + 2) % Width), Mathd.Abs((iy + 1) % Height)];
        double h14 = Data[Mathd.Abs((ix + 1) % Width), Mathd.Abs((iy + 2) % Height)];
        double h15 = Data[Mathd.Abs((ix + 2) % Width), Mathd.Abs((iy + 2) % Height)];

        double tx = x - Mathd.Floor(x);
        double ty = y - Mathd.Floor(y);

        double v0 = GetCubicValue(ty, h0, h2, h8, h10);
        double v1 = GetCubicValue(ty, h1, h3, h9, h11);
        double v2 = GetCubicValue(ty, h4, h6, h12, h14);
        double v3 = GetCubicValue(ty, h5, h7, h13, h15);

        return this.GetCubicValue(tx, v0, v1, v2, v3);          
    }

    private double GetCubicValue(double tx, double h0, double h1, double h2, double h3)
    {
        return h1 + 0.5f * tx * (h2 - h0 + tx * (2.0f * h0 - 5.0f * h1 + 4.0f * h2 - h3 + tx * (3.0f * (h1 - h2) + h3 - h0)));
    }    

    private double GetBicubicValue(double tx, double ty, double h0, double h1, double h2, double h3)
    {
        return this.GetCubicValue(tx, h0, h1, h2, h3) + this.GetCubicValue(ty, h0, h1, h2, h3);
    }     

    // Bilinear
    private double ApplyBicubicFilter(double x, double y)
    {
        int offset = 1;

        int ix = Mathd.Abs((int)(x * Width * (int)TileX));
        int iy = Mathd.Abs((int)(y * Height * (int)TileY));

        double h1 = Data[Mathd.Abs((ix - offset) % Width),  Mathd.Abs((iy - offset) % Height)];
        double h2 = Data[Mathd.Abs((ix + offset) % Width),  Mathd.Abs((iy - offset) % Height)];
        double h3 = Data[Mathd.Abs((ix - offset) % Width),  Mathd.Abs((iy + offset) % Height)];
        double h4 = Data[Mathd.Abs((ix + offset) % Width),  Mathd.Abs((iy + offset) % Height)];

        double tx = x - Mathd.Floor(x);
        double ty = y - Mathd.Floor(y);

        return this.GetBicubicValue(tx, ty, h1, h2, h3, h4);
    }    
    
    private double ApplyBilinearFilter(double x, double y)
    {
        int offset = 1;

        int ix = Mathd.Abs((int)(x * TileX * Width));
        int iy = Mathd.Abs((int)(y * TileY * Height));

        double h1 = Data[Mathd.Abs((ix) % Width),  Mathd.Abs((iy) % Height)];
        double h2 = Data[Mathd.Abs((ix + offset) % Width),  Mathd.Abs((iy) % Height)];
        double h3 = Data[Mathd.Abs((ix) % Width),  Mathd.Abs((iy + offset) % Height)];
        double h4 = Data[Mathd.Abs((ix + offset) % Width),  Mathd.Abs((iy + offset) % Height)];

        double tx = x - Mathd.Floor(x);
        double ty = y - Mathd.Floor(y);

        return this.GetBilinearValue(tx, ty, h1, h2, h3, h4);
    }

    private Vector2d Fade(Vector2d t)
    {
    //    return new Vector2(-2 * t.x * t.x * t.x + 3 * t.x * t.x, -2 * t.y * t.y * t.y + 3 * t.y * t.y); // old curve
        return new Vector2d(t.x * t.x * t.x * (t.x * (t.x * 6 - 15) + 10), t.y * t.y * t.y * (t.y * (t.y * 6 - 15) + 10)); // new curve
    }

    private double GetBilinearValue(double tx, double ty, double h1, double h2, double h3, double h4)
    {
        Vector2d f = Fade(new Vector2d(tx, ty));

        double a = h1 * (1 - tx) + h2 * tx;//Mathd.Lerp(h1, h2, f.x);
        double b = h3 * (1 - tx) + h4 * tx;// Mathd.Lerp(h3, h4, f.x);

        return a * (1 - ty) + b * ty; //Mathd.Lerp(a, b, f.y);
    }   
}
