using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CuboidCornerType
{
    TRB = 0,
    TLB = 1,
    TRF = 2,
    TLF = 3,
    BRB = 4,
    BLB = 5,
    BRF = 6,
    BLF = 7,
    NONE = 8
};

public enum CuboidEdgeType
{
    TB = 0,
    TR = 1,
    TF = 2,
    TL = 3,
    BB = 4,
    BR = 5,
    BF = 6,
    BL = 7,
    BKR = 8,
    BKL = 9,
    FR = 10,
    FL = 11,
    NONE = 12
}

public class CuboidHeightMapScript
{
    // public Texture2D Top;
    // public Texture2D Bottom;
    // public Texture2D Right;
    // public Texture2D Left;
    // public Texture2D Front;
    // public Texture2D Back;

    public HeightMapScript Top;
    public HeightMapScript Bottom;
    public HeightMapScript Right;
    public HeightMapScript Left;
    public HeightMapScript Front;
    public HeightMapScript Back;

    public int Tiling = 1;
    public float Height;
    public float TileX;
    public float TileY;

    public CuboidHeightMapScript(Texture2D top, Texture2D bottom, Texture2D right, 
                                 Texture2D left, Texture2D front, Texture2D back)
    {
        TileX = 1;
        TileY = 1;

        Top = new HeightMapScript(top.width, top.height, TileX, TileY, top);
        Bottom = new HeightMapScript(bottom.width, bottom.height, TileX, TileY, bottom);
        Right = new HeightMapScript(right.width, right.height, TileX, TileY, right);
        Left = new HeightMapScript(left.width, left.height, TileX, TileY, left);
        Front = new HeightMapScript(front.width, front.height, TileX, TileY, front);
        Back = new HeightMapScript(back.width, back.height, TileX, TileY, back); 

        Tiling = 1;

        Height = 0.03f;
    }

    public Vector3 GetCornerCoords(CuboidCornerType cornerType, float edgeLength, GridFaceType faceType = GridFaceType.NONE)
    {
        Vector3 coords = new Vector3();

        if(cornerType == CuboidCornerType.TRB)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3(1 - edgeLength, 1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3(1, 1 - edgeLength, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3(1 - edgeLength, 1 - edgeLength, -1);
            }

        //    coords = new Vector3(1, 1, -1);
        }
        else
        if(cornerType == CuboidCornerType.TLB)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3(-1 + edgeLength, 1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3(-1, 1 - edgeLength, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3(-1 + edgeLength, 1 - edgeLength, -1);
            }            
        //    coords = new Vector3(-1, 1, -1);
        }
        else
        if(cornerType == CuboidCornerType.TRF)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3(1 - edgeLength, 1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3(1, 1 - edgeLength, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3(1 - edgeLength, 1 - edgeLength, 1);
            } 

//            coords = new Vector3(1, 1, 1);
        }
        else
        if(cornerType == CuboidCornerType.TLF)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3(-1 + edgeLength, 1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3(-1, 1 - edgeLength, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3(-1 + edgeLength, 1 - edgeLength, 1);
            } 

        //    coords = new Vector3(-1, 1, 1);
        }
        else
        if(cornerType == CuboidCornerType.BRB)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3(1 - edgeLength, -1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3(1, -1 + edgeLength, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3(1 - edgeLength, -1 + edgeLength, -1);
            } 

         //   coords = new Vector3(1, -1, -1);
        }
        else
        if(cornerType == CuboidCornerType.BLB)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3(-1 + edgeLength, -1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3(-1, -1 + edgeLength, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3(-1 + edgeLength, -1 + edgeLength, -1);
            } 

        //    coords = new Vector3(-1, -1, -1);
        }
        else
        if(cornerType == CuboidCornerType.BRF)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3(1 - edgeLength, -1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3(1, -1 + edgeLength, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3(1 - edgeLength, -1 + edgeLength, 1);
            } 

         //   coords = new Vector3(1, -1, 1);
        }
        else
        if(cornerType == CuboidCornerType.BLF)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3(-1 + edgeLength, -1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3(-1, -1 + edgeLength, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3(-1 + edgeLength, -1 + edgeLength, 1);
            } 

       //     coords = new Vector3(-1, -1, 1);
        }         

        return coords;
    }

    public CuboidCornerType GetCornerType(Vector3 pos)
    {
        CuboidCornerType cornerType = CuboidCornerType.NONE;

        if(Mathf.Approximately(pos.x, 1.0f) == true && Mathf.Approximately(pos.y, 1.0f) == true && 
           Mathf.Approximately(pos.z, -1.0f) == true)
        {
            cornerType = CuboidCornerType.TRB; // top right back
        }
        else
        if(Mathf.Approximately(pos.x, -1.0f) == true && Mathf.Approximately(pos.y, 1.0f) == true && 
           Mathf.Approximately(pos.z, -1.0f) == true)
        {
            cornerType = CuboidCornerType.TLB; // top left back
        }
        else
        if(Mathf.Approximately(pos.x, 1.0f) == true && Mathf.Approximately(pos.y, 1.0f) == true && 
           Mathf.Approximately(pos.z, 1.0f) == true)
        {
            cornerType = CuboidCornerType.TRF; // top right front
        }
        else
        if(Mathf.Approximately(pos.x, -1.0f) == true && Mathf.Approximately(pos.y, 1.0f) == true && 
           Mathf.Approximately(pos.z, 1.0f) == true)
        {
            cornerType = CuboidCornerType.TLF; // top left front
        }
        else
        if(Mathf.Approximately(pos.x, 1.0f) == true && Mathf.Approximately(pos.y, -1.0f) == true && 
           Mathf.Approximately(pos.z, -1.0f) == true)
        {
            cornerType = CuboidCornerType.BRB; // bottom right back
        }
        else
        if(Mathf.Approximately(pos.x, -1.0f) == true && Mathf.Approximately(pos.y, -1.0f) == true && 
           Mathf.Approximately(pos.z, -1.0f) == true)
        {
            cornerType = CuboidCornerType.BLB; // bottom left back
        }
        else
        if(Mathf.Approximately(pos.x, 1.0f) == true && Mathf.Approximately(pos.y, -1.0f) == true && 
           Mathf.Approximately(pos.z, 1.0f) == true)
        {
            cornerType = CuboidCornerType.BRF; // bottom right front
        }
        else
        if(Mathf.Approximately(pos.x, -1.0f) == true && Mathf.Approximately(pos.y, -1.0f) == true && 
           Mathf.Approximately(pos.z, 1.0f) == true)
        {
            cornerType = CuboidCornerType.BLF; // bottom left front
        }

        return cornerType;
    }

    public Vector3 GetEdgeCoords(CuboidEdgeType edgeType, Vector3 pos, float edgeLength, GridFaceType faceType = GridFaceType.NONE)
    {
        Vector3 coords = new Vector3();

        if(edgeType == CuboidEdgeType.TB)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3(pos.x, 1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3(pos.x, 1 - edgeLength, -1);
            }
           // coords = new Vector3(pos.x, 1, -1);
        }
        else
        if(edgeType == CuboidEdgeType.TR)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3(1 - edgeLength, 1, pos.z);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3(1, 1 - edgeLength, pos.z);
            }            
            //  coords = new Vector3(1, 1, pos.z);
        }        
        else
        if(edgeType == CuboidEdgeType.TF)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3(pos.x, 1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3(pos.x, 1 - edgeLength, 1);
            }              
       //     coords = new Vector3(pos.x, 1, 1);
        }        
        else
        if(edgeType == CuboidEdgeType.TL)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3(-1 + edgeLength, 1, pos.z);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3(-1, 1 - edgeLength, pos.z);
            }              
         //   coords = new Vector3(-1, 1, pos.z);
        }        
        else
        if(edgeType == CuboidEdgeType.BB)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3(pos.x, -1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3(pos.x, -1 + edgeLength, -1);
            }              
          //  coords = new Vector3(pos.x, -1, -1);
        }        
        else
        if(edgeType == CuboidEdgeType.BR)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3(1 - edgeLength, -1, pos.z);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3(1, -1 + edgeLength, pos.z);
            }              
         //   coords = new Vector3(1, -1, pos.z);
        }        
        else
        if(edgeType == CuboidEdgeType.BF)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3(pos.x, -1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3(pos.x, -1 + edgeLength, 1);
            }              
          //  coords = new Vector3(pos.x, -1, 1);
        }        
        else
        if(edgeType == CuboidEdgeType.BL)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3(-1 + edgeLength, -1, pos.z);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3(-1, -1 + edgeLength, pos.z);
            }              
         //   coords = new Vector3(-1, -1, pos.z);
        }        
        else
        if(edgeType == CuboidEdgeType.BKR)
        {
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3(1 - edgeLength, pos.y, -1);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3(1, pos.y, -1 + edgeLength);
            }              
         //   coords = new Vector3(1, pos.y, -1);
        }        
        else
        if(edgeType == CuboidEdgeType.BKL)
        {
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3(-1 + edgeLength, pos.y, -1);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3(-1, pos.y, -1 + edgeLength);
            }              
        //    coords = new Vector3(-1, pos.y, -1);
        }        
        else
        if(edgeType == CuboidEdgeType.FR)
        {
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3(1 - edgeLength, pos.y, 1);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3(1, pos.y, 1 - edgeLength);
            }              
      //      coords = new Vector3(1, pos.y, 1);
        }        
        else
        if(edgeType == CuboidEdgeType.FL)
        {
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3(-1 + edgeLength, pos.y, 1);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3(-1, pos.y, 1 - edgeLength);
            }              
         //   coords = new Vector3(-1, pos.y, 1);
        }        

        return coords;
    }

    public CuboidEdgeType GetEdgeType(Vector3 pos)
    {
        CuboidEdgeType edgeType = CuboidEdgeType.NONE;

        if(Mathf.Approximately(pos.y, 1.0f) == true && Mathf.Approximately(pos.z, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.TB;
        }
        else
        if(Mathf.Approximately(pos.x, 1.0f) == true && Mathf.Approximately(pos.y, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.TR;
        }        
        else
        if(Mathf.Approximately(pos.y, 1.0f) == true && Mathf.Approximately(pos.z, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.TF;
        }        
        else
        if(Mathf.Approximately(pos.x, -1.0f) == true && Mathf.Approximately(pos.y, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.TL;
        }        
        else
        if(Mathf.Approximately(pos.y, -1.0f) == true && Mathf.Approximately(pos.z, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BB;
        }        
        else
        if(Mathf.Approximately(pos.x, 1.0f) == true && Mathf.Approximately(pos.y, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BR;
        }        
        else
        if(Mathf.Approximately(pos.y, -1.0f) == true && Mathf.Approximately(pos.z, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.BF;
        }        
        else
        if(Mathf.Approximately(pos.x, -1.0f) == true && Mathf.Approximately(pos.y, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BL;
        }        
        else
        if(Mathf.Approximately(pos.x, 1.0f) == true && Mathf.Approximately(pos.z, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BKR;
        }        
        else
        if(Mathf.Approximately(pos.x, -1.0f) == true && Mathf.Approximately(pos.z, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BKL;
        }        
        else
        if(Mathf.Approximately(pos.x, 1.0f) == true && Mathf.Approximately(pos.z, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.FR;
        }        
        else
        if(Mathf.Approximately(pos.x, -1.0f) == true && Mathf.Approximately(pos.z, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.FL;
        }        

        return edgeType;
    }

    public float GetEdgeHeightValue(CuboidEdgeType edgeType, Vector3 pos, float edgeLength)
    {
        float height = 0;

        Vector2 uv1 = new Vector2();
        Vector2 uv2 = new Vector2();        

        HeightMapScript face1 = null;
        HeightMapScript face2 = null;

        if(edgeType == CuboidEdgeType.TB)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.TOP), GridFaceType.TOP);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.BACK), GridFaceType.BACK);

            face1 = Top;
            face2 = Back;
        }
        else
        if(edgeType == CuboidEdgeType.TR)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.TOP), GridFaceType.TOP);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.RIGHT), GridFaceType.RIGHT);

            face1 = Top;
            face2 = Right;
        }
        else
        if(edgeType == CuboidEdgeType.TF)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.TOP), GridFaceType.TOP);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.FRONT), GridFaceType.FRONT);

            face1 = Top;
            face2 = Front;
        }
        else
        if(edgeType == CuboidEdgeType.TL)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.TOP), GridFaceType.TOP);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.LEFT), GridFaceType.LEFT);

            face1 = Top;
            face2 = Left;
        }
        else
        if(edgeType == CuboidEdgeType.BB)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.BOTTOM), GridFaceType.BOTTOM);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.BACK), GridFaceType.BACK);

            face1 = Bottom;
            face2 = Back;
        }
        else
        if(edgeType == CuboidEdgeType.BR)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.BOTTOM), GridFaceType.BOTTOM);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.RIGHT), GridFaceType.RIGHT);

            face1 = Bottom;
            face2 = Right;
        }
        else
        if(edgeType == CuboidEdgeType.BF)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.BOTTOM), GridFaceType.BOTTOM);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.FRONT), GridFaceType.FRONT);

            face1 = Bottom;
            face2 = Front;
        }
        else
        if(edgeType == CuboidEdgeType.BL)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.BOTTOM), GridFaceType.BOTTOM);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.LEFT), GridFaceType.LEFT);

            face1 = Bottom;
            face2 = Left;
        }
        else
        if(edgeType == CuboidEdgeType.BKR)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.BACK), GridFaceType.BACK);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.RIGHT), GridFaceType.RIGHT);

            face1 = Back;
            face2 = Right;
        }
        else
        if(edgeType == CuboidEdgeType.BKL)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.BACK), GridFaceType.BACK);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.LEFT), GridFaceType.LEFT);

            face1 = Back;
            face2 = Left;
        }
        else
        if(edgeType == CuboidEdgeType.FR)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.FRONT), GridFaceType.FRONT);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.RIGHT), GridFaceType.RIGHT);

            face1 = Front;
            face2 = Right;
        }
        else
        if(edgeType == CuboidEdgeType.FL)
        {
            uv1 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.FRONT), GridFaceType.FRONT);
            uv2 = this.GetFaceUV(this.GetEdgeCoords(edgeType, pos, edgeLength, GridFaceType.LEFT), GridFaceType.LEFT);

            face1 = Front;
            face2 = Left;
        }

        float h1 = face1.GetHeightValue(uv1.x, uv1.y) * Height;//face1.GetHeightValue((int)(uv1.x * face1.Width) * Tiling, (int)(uv1.y * face1.Height) * Tiling) * Height;
        float h2 = face2.GetHeightValue(uv2.x, uv2.y) * Height;// face2.GetHeightValue((int)(uv2.x * face2.Width) * Tiling, (int)(uv2.y * face2.Height) * Tiling) * Height;

        height = (h1 + h2) / 2;


        return height;
    }

    public float GetCornerHeightValue(CuboidCornerType cornerType, float edgeLength)
    {
        float height = 0;

        Vector2 uv1 = new Vector2();
        Vector2 uv2 = new Vector2();
        Vector2 uv3 = new Vector2();

        HeightMapScript face1 = null;
        HeightMapScript face2 = null;
        HeightMapScript face3 = null;

        if(cornerType == CuboidCornerType.TRB)
        {
            uv1 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.TOP), GridFaceType.TOP);
            uv2 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.RIGHT), GridFaceType.RIGHT);
            uv3 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.BACK), GridFaceType.BACK);

            face1 = Top;
            face2 = Right;
            face3 = Back;
        }
        else
        if(cornerType == CuboidCornerType.TLB)
        {
            uv1 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.TOP), GridFaceType.TOP);
            uv2 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.LEFT), GridFaceType.LEFT);
            uv3 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.BACK), GridFaceType.BACK);

            face1 = Top;
            face2 = Left;
            face3 = Back;
        }
        else
        if(cornerType == CuboidCornerType.TRF)
        {
            uv1 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.TOP), GridFaceType.TOP);
            uv2 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.RIGHT), GridFaceType.RIGHT);
            uv3 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.FRONT), GridFaceType.FRONT);

            face1 = Top;
            face2 = Right;
            face3 = Front;
        }
        else
        if(cornerType == CuboidCornerType.TLF)
        {
            uv1 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.TOP), GridFaceType.TOP);
            uv2 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.LEFT), GridFaceType.LEFT);
            uv3 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.FRONT), GridFaceType.FRONT);

            face1 = Top;
            face2 = Left;
            face3 = Front;
        }
        else
        if(cornerType == CuboidCornerType.BRB)
        {
            uv1 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.BOTTOM), GridFaceType.BOTTOM);
            uv2 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.RIGHT), GridFaceType.RIGHT);
            uv3 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.BACK), GridFaceType.BACK);

            face1 = Bottom;
            face2 = Right;
            face3 = Back;
        }
        else
        if(cornerType == CuboidCornerType.BLB)
        {
            uv1 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.BOTTOM), GridFaceType.BOTTOM);
            uv2 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.LEFT), GridFaceType.LEFT);
            uv3 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.BACK), GridFaceType.BACK);

            face1 = Bottom;
            face2 = Left;
            face3 = Back;
        }
        else
        if(cornerType == CuboidCornerType.BRF)
        {
            uv1 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.BOTTOM), GridFaceType.BOTTOM);
            uv2 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.RIGHT), GridFaceType.RIGHT);
            uv3 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.FRONT), GridFaceType.FRONT);

            face1 = Bottom;
            face2 = Right;
            face3 = Front;
        }
        else
        if(cornerType == CuboidCornerType.BLF)
        {
            uv1 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.BOTTOM), GridFaceType.BOTTOM);
            uv2 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.LEFT), GridFaceType.LEFT);
            uv3 = this.GetFaceUV(this.GetCornerCoords(cornerType, edgeLength, GridFaceType.FRONT), GridFaceType.FRONT);

            face1 = Bottom;
            face2 = Left;
            face3 = Front;
        }                                                        

        float h1 = face1.GetHeightValue(uv1.x, uv1.y) * Height;//face1.GetHeightValue((int)(uv1.x * face1.Width) * Tiling, (int)(uv1.y * face1.Height) * Tiling) * Height;
        float h2 = face2.GetHeightValue(uv2.x, uv2.y) * Height;//face2.GetHeightValue((int)(uv2.x * face2.Width) * Tiling, (int)(uv2.y * face2.Height) * Tiling) * Height;
        float h3 = face3.GetHeightValue(uv3.x, uv3.y) * Height;//face3.GetHeightValue((int)(uv3.x * face3.Width) * Tiling, (int)(uv3.y * face3.Height) * Tiling) * Height; 

        height = (h1 + h2 + h3) / 3;

        return height;
    }

    public float GetFaceHeightValue(GridFaceType faceType, Vector2 uv)
    {
        HeightMapScript selectedFace = null;

        if(faceType == GridFaceType.TOP)
        {
            selectedFace = Top;
        }
        else
        if(faceType == GridFaceType.BOTTOM)
        {
            selectedFace = Bottom;
        }
        else
        if(faceType == GridFaceType.RIGHT)
        {
            selectedFace = Right;
        }
        else
        if(faceType == GridFaceType.LEFT)
        {
            selectedFace = Left;
        }
        else
        if(faceType == GridFaceType.FRONT)
        {
            selectedFace = Front;
        }
        else
        if(faceType == GridFaceType.BACK)
        {
            selectedFace = Back;
        }

        return selectedFace.GetHeightValue(uv.x, uv.y) * Height;
//        return selectedFace.GetHeightValue((int)(uv.x * selectedFace.Width) * Tiling, (int)(uv.y * selectedFace.Height) * Tiling) * Height;
    }

    public Vector2 GetFaceUV(Vector3 pos, GridFaceType faceType)
    {
        float u = 0, v = 0;

        if(faceType == GridFaceType.TOP)
        {
            u = ((pos.x + 1) / 2);
            v = ((-pos.z + 1) / 2);
        }
        else
        if(faceType == GridFaceType.BOTTOM)
        {
            u = ((pos.x + 1) / 2);
            v = ((pos.z + 1) / 2);
        }
        else
        if(faceType == GridFaceType.RIGHT)
        {
            u = ((-pos.z + 1) / 2);
            v = ((pos.y + 1) / 2);
        }
        else
        if(faceType == GridFaceType.LEFT)
        {
            u = ((pos.z + 1) / 2);
            v = ((pos.y + 1) / 2);
        }
        else
        if(faceType == GridFaceType.FRONT)
        {
            u = ((pos.x + 1) / 2);
            v = ((pos.y + 1) / 2);
        }
        else
        if(faceType == GridFaceType.BACK)
        {
            u = ((-pos.x + 1) / 2);
            v = ((pos.y + 1) / 2);
        }        

        return new Vector2(u, v);
    }

    public Vector3 GetHeightValue(Vector3 pos, GridFaceType faceType, float edgeLength)
    {
        float height = 0;
        Vector2 uv = new Vector2();

        uv = this.GetFaceUV(pos, faceType);

        float absX = Mathf.Abs(pos.x);
        float absY = Mathf.Abs(pos.y);
        float absZ = Mathf.Abs(pos.z);

        if(faceType == GridFaceType.TOP || faceType == GridFaceType.BOTTOM)
        {
            if(absX > absY && absX > absZ)
            {
                if(pos.x > 0)
                {
                    faceType = GridFaceType.RIGHT;
                }
                else
                if(pos.x < 0)
                {
                    faceType = GridFaceType.LEFT;
                }
            }
            else
            if(absZ > absX && absZ > absY)
            {
                if(pos.z > 0)
                {
                    faceType = GridFaceType.FRONT;
                }
                else
                if(pos.z < 0)
                {
                    faceType = GridFaceType.BACK;
                }
            }            
        }
        else
        if(faceType == GridFaceType.LEFT || faceType == GridFaceType.RIGHT)
        {
            if(absY > absX && absY > absZ)
            {
                if(pos.y > 0)
                {
                    faceType = GridFaceType.TOP;
                }
                else
                if(pos.y < 0)
                {
                    faceType = GridFaceType.BOTTOM;
                }
            }
            else        
            if(absZ > absX && absZ > absY)
            {
                if(pos.z > 0)
                {
                    faceType = GridFaceType.FRONT;
                }
                else
                if(pos.z < 0)
                {
                    faceType = GridFaceType.BACK;
                }
            }
        }
        else
        if(faceType == GridFaceType.FRONT || faceType == GridFaceType.BACK)
        {
            if(absY > absX && absY > absZ)
            {
                if(pos.y > 0)
                {
                    faceType = GridFaceType.TOP;
                }
                else
                if(pos.y < 0)
                {
                    faceType = GridFaceType.BOTTOM;
                }
            }
            else
            if(absX > absY && absX > absZ)
            {
                if(pos.x > 0)
                {
                    faceType = GridFaceType.RIGHT;
                }
                else
                if(pos.x < 0)
                {
                    faceType = GridFaceType.LEFT;
                }
            }
        }    

        CuboidCornerType cornerType = CuboidCornerType.NONE;

        cornerType = this.GetCornerType(pos);

        Vector2 cuv = new Vector2();

        cuv = this.GetFaceUV(pos, faceType);

        if(cornerType != CuboidCornerType.NONE)
        {
            height = this.GetCornerHeightValue(cornerType, edgeLength);
        }
        else
        if(cornerType == CuboidCornerType.NONE)
        {
            // Add normal heights
            CuboidEdgeType edgeType = this.GetEdgeType(pos);

            if(edgeType != CuboidEdgeType.NONE)
            {
                height = this.GetEdgeHeightValue(edgeType, pos, edgeLength);
            }
            else
            {
                height = this.GetFaceHeightValue(faceType, cuv);
            }
        }

        return new Vector3(uv.x, uv.y, height);
    }
}
