using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapScript
{
    public float[,] Data;
    public float TileX;
    public float TileY;
    public int Width;
    public int Height;

    public HeightMapScript(int width, int height, float tileX, float tileY, Texture2D hmTexture)
    {
        Width = width;
        Height = height;
        TileX = tileX;
        TileY = tileY;

    //    Debug.Log("Width : " + Width + " : Height : " + Height);

        Data = new float[width, height];

    //    Debug.Log("Data.Length : " + Data.Length);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Data[x, y] = hmTexture.GetPixel(x, y).r;
            }
        } 

    //    this.ApplyGaussianBlur(0.01f);       
    }

    public void ApplyGaussianBlur(float weight)
    {
        float sum = 0;
        int foff = (Width - 1) / 2;
        float distance = 0;
        float constant = 1 / 2 * Mathf.PI * weight * weight;

        for(int y = -foff; y <= foff; y++)
        {
            for(int x = -foff; x <= foff; x++)
            {
                distance = ((y * y) + (x * x)) / (2 * weight * weight);
                Data[y + foff, x + foff] = constant * Mathf.Exp(-distance);
                sum += Data[y + foff, x + foff];
            }
        }

        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                Data[y, x] = Data[y, x] * (1 / sum);
            }
        }
    }

    public float GetHeightValue(float x, float y)
    {
        // float wx = x * Width * TileX;
        // float wy = y * Height * TileY;

        // int fx = Mathf.FloorToInt(wx);
        // int cx = Mathf.CeilToInt(wx);
        // int fy = Mathf.FloorToInt(wy);
        // int cy = Mathf.CeilToInt(wy);

        // float hx0z0 = Data[fx % Width, fy % Height] * 0.1f; // known height (x0, z0)
        // float hx1z0 = Data[cx % Width, fy % Height] * 0.1f; // known height (x1, z0)
        // float hx0z1 = Data[fx % Width, cy % Height] * 0.1f; // known height (x0, z1)
        // float hx1z1 = Data[cx % Width, cy % Height] * 0.1f;        

        // float u0v0 = hx0z0 * (cx - wx) * (cy - wy); // interpolated (x0, z0)
        // float u1v0 = hx1z0 * (wx - fx) * (cy - wy); // interpolated (x1, z0)
        // float u0v1 = hx0z1 * (cx - wx) * (wy - fy); // interpolated (x0, z1)
        // float u1v1 = hx1z1 * (wx - fx) * (wy - fy); // interpolated (x1, z1)

        // float h = u0v0 + u1v0 + u0v1 + u1v1;

        // return h;

        // x = x * Width * TileX + 0.5f;
        // y = y * Height * TileY + 0.5f;

        // int ix = Mathf.FloorToInt(x);
        // int iy = Mathf.FloorToInt(y);

        // float fx = x - ix;
        // float fy = y - iy;

        // Vector2 f = new Vector2(fx, fy);

        // f = f * f * f * (f * (f * 6.0f - new Vector2(15.0f, 15.0f)) + new Vector2(10.0f, 10.0f));

        // Vector2 p = new Vector2(ix, iy) + f;

        // p = (p - new Vector2(0.5f, 0.5f)) / Width;

        // return Data[(int)(p.x * TileX) % Width, (int)(p.y * TileY) % Height];

    //    return Data[Mathf.Abs((int)(x * Width)) * (int)TileX % Width, Mathf.Abs((int)(y * Height)) * (int)TileY % Height];
        // BiLinear Filtering
        
        x = Mathf.Round(x * 10000) * 0.0001f;
        y = Mathf.Round(y * 10000) * 0.0001f;

        int offset = 1;

        int ix = Mathf.Abs((int)(x * Width * (int)TileX));
        int iy = Mathf.Abs((int)(y * Height * (int)TileY));

        float h1 = Data[(ix - offset) % Width,  (iy - offset) % Height];
        float h2 = Data[(ix + offset) % Width,  (iy - offset) % Height];
        float h3 = Data[(ix - offset) % Width,  (iy + offset) % Height];
        float h4 = Data[(ix + offset) % Width,  (iy + offset) % Height];

        float tx = x - Mathf.Floor(x);
        float ty = y - Mathf.Floor(y);

    //  //   Debug.Log(tx + " : " + ty);

    // //    float height1 = Data[Mathf.Abs((int)(x * Width)) * (int)TileX % Width, Mathf.Abs((int)(y * Height)) * (int)TileY % Height];
    
        return this.GetBilinearValue(tx, ty, h1, h2, h3, h4);
    //     float height2 = this.GetBilinearValue(x, y, h1, h2, h3, h4);

    // //    Debug.Log("height1 : " + height1 + " : height2 : " + height2);

    //     return height2;

        // Bicubic Filtering
        // float _x = (Mathf.Abs(x) * Width) * TileX;
        // float _y = (Mathf.Abs(y) * Height) * TileY;

        // int fx = Mathf.Abs((int)(x * Width) * (int)TileX);
        // int fy = Mathf.Abs((int)(y * Height) * (int)TileY);
                

        // float h0 = Data[(fx - 1) % Width,  (fy - 1) % Height];
        // float h1 = Data[fx % Width,        (fy - 1) % Height];
        // float h2 = Data[(fx - 1) % Width,  fy % Height];
        // float h3 = Data[fx % Width,        fy % Height];

        // float h4 = Data[(fx + 1) % Width,  (fy - 1) % Height];
        // float h5 = Data[(fx + 2) % Width,  (fy - 1) % Height];
        // float h6 = Data[(fx + 1) % Width,  fy % Height];
        // float h7 = Data[(fx + 2) % Width,  fy % Height];

        // float h8 = Data[(fx - 1) % Width,  (fy + 1) % Height];
        // float h9 = Data[fx % Width,        (fy + 1) % Height];
        // float h10 = Data[(fx - 1) % Width, (fy + 2) % Height];
        // float h11 = Data[fx % Width,       (fy + 2) % Height];

        // float h12 = Data[(fx + 1) % Width, (fy + 1) % Height];
        // float h13 = Data[(fx + 2) % Width, (fy + 1) % Height];
        // float h14 = Data[(fx + 1) % Width, (fy + 2) % Height];
        // float h15 = Data[(fx + 2) % Width, (fy + 2) % Height];

        // float tx = _x - Mathf.Floor(_x);
        // float ty = _y - Mathf.Floor(_y);

        // float v0 = GetCubicValue(ty, h0, h2, h8, h10);
        // float v1 = GetCubicValue(ty, h1, h3, h9, h11);
        // float v2 = GetCubicValue(ty, h4, h6, h12, h14);
        // float v3 = GetCubicValue(ty, h5, h7, h13, h15);

        // return this.GetCubicValue(tx, v0, v1, v2, v3);  
    
    }

    private float GetCubicValue(float tx, float h0, float h1, float h2, float h3)
    {
        return h1 + 0.5f * tx * (h2 - h0 + tx * (2.0f * h0 - 5.0f * h1 + 4.0f * h2 - h3 + tx * (3.0f * (h1 - h2) + h3 - h0)));
    }    

    private float GetBicubicValue(float tx, float ty, float h0, float h1, float h2, float h3)
    {
        return this.GetCubicValue(tx, h0, h1, h2, h3) + this.GetCubicValue(ty, h0, h1, h2, h3);
    }  

    private Vector2 Fade(Vector2 t)
    {
    //    return new Vector2(-2 * t.x * t.x * t.x + 3 * t.x * t.x, -2 * t.y * t.y * t.y + 3 * t.y * t.y); // old curve
        return new Vector2(t.x * t.x * t.x * (t.x * (t.x * 6 - 15) + 10), t.y * t.y * t.y * (t.y * (t.y * 6 - 15) + 10)); // new curve
    }

    private float GetBilinearValue(float tx, float ty, float h1, float h2, float h3, float h4)
    {
        Vector2 f = Fade(new Vector2(tx, ty));

        float a = Mathf.Lerp(h1, h2, f.x);
        float b = Mathf.Lerp(h3, h4, f.x);

        return Mathf.Lerp(a, b, f.y);
    }      

// catmull works by specifying 4 control points p0, p1, p2, p3 and a weight. The function is used to calculate a point n between p1 and p2 based
// on the weight. The weight is normalized, so if it's a value of 0 then the return value will be p1 and if its 1 it will return p2. 
// float catmullRom( float p0, float p1, float p2, float p3, float weight ) {
//     float weight2 = weight * weight;
//     return 0.5 * (
//         p0 * weight * ( ( 2.0 - weight ) * weight - 1.0 ) +
//         p1 * ( weight2 * ( 3.0 * weight - 5.0 ) + 2.0 ) +
//         p2 * weight * ( ( 4.0 - 3.0 * weight ) * weight + 1.0 ) +
//         p3 * ( weight - 1.0 ) * weight2 );
// }

// Performs a horizontal catmulrom operation at a given V value.
// float textureCubicU( sampler2D samp, vec2 uv00, float texel, float offsetV, float frac ) {
//     return catmullRom(
//         texture2DLod( samp, uv00 + vec2( -texel, offsetV ), 0.0 ).r,
//         texture2DLod( samp, uv00 + vec2( 0.0, offsetV ), 0.0 ).r,
//         texture2DLod( samp, uv00 + vec2( texel, offsetV ), 0.0 ).r,
//         texture2DLod( samp, uv00 + vec2( texel * 2.0, offsetV ), 0.0 ).r,
//     frac );
// }

// Samples a texture using a bicubic sampling algorithm. This essentially queries neighbouring
// pixels to get an average value.
// float textureBicubic( sampler2D samp, vec2 uv00, vec2 texel, vec2 frac ) {
//     return catmullRom(
//         textureCubicU( samp, uv00, texel.x, -texel.y, frac.x ),
//         textureCubicU( samp, uv00, texel.x, 0.0, frac.x ),
//         textureCubicU( samp, uv00, texel.x, texel.y, frac.x ),
//         textureCubicU( samp, uv00, texel.x, texel.y * 2.0, frac.x ),
//     frac.y );
// }

    // Gets the  UV coordinates based on the world X Z position
    // vec2 worldToMapSpace( vec2 worldPosition ) {
    //     return ( worldPosition / worldScale + 0.5 );
    // }


// Gets the height at a location p (world space)
// float getHeight( vec3 worldPosition )
// {
//     #ifdef USE_HEIGHTFIELD

//         vec2 heightUv = worldToMapSpace(worldPosition.xz);
//         vec2 tHeightSize = vec2( HEIGHTFIELD_WIDTH, HEIGHTFIELD_HEIGHT );

//         // If we increase the smoothness factor, the terrain becomes a lot smoother.
//         // This is because it has the effect of shrinking the texture size and increaing
//         // the texel size. Which means when we do sampling the samples are from farther away - making
//         // it smoother. However this means the terrain looks less like the original heightmap and so
//         // terrain picking goes a bit off. 
//         float smoothness = 1.1;
//         tHeightSize /= smoothness;

//         // The size of each texel
//         vec2 texel = vec2( 1.0 / tHeightSize );

//         // Find the top-left texel we need to sample.
//         vec2 heightUv00 = ( floor( heightUv * tHeightSize ) ) / tHeightSize;

//         // Determine the fraction across the 4-texel quad we need to compute.
//         vec2 frac = vec2( heightUv - heightUv00 ) * tHeightSize;

//         float coarseHeight = textureBicubic( heightfield, heightUv00, texel, frac );
//         return altitude * coarseHeight + heightOffset;
//     #else
//         return 0.0;
//     #endif
// }    
}
