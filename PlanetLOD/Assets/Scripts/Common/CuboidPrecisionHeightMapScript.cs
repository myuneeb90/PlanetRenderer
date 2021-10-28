using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboidPrecisionHeightMapScript
{
    public PrecisionHeightMapScript Top;
    public PrecisionHeightMapScript Bottom;
    public PrecisionHeightMapScript Right;
    public PrecisionHeightMapScript Left;
    public PrecisionHeightMapScript Front;
    public PrecisionHeightMapScript Back;

    public double Height;
    public float TileX;
    public float TileY;

    public CuboidPrecisionHeightMapScript(Texture2D top, Texture2D bottom, Texture2D right, 
                                          Texture2D left, Texture2D front, Texture2D back)
    {
        TileX = 2;
        TileY = 2;

        Top = new PrecisionHeightMapScript(top.width, top.height, TileX, TileY, top);
        Bottom = new PrecisionHeightMapScript(bottom.width, bottom.height, TileX, TileY, bottom);
        Right = new PrecisionHeightMapScript(right.width, right.height, TileX, TileY, right);
        Left = new PrecisionHeightMapScript(left.width, left.height, TileX, TileY, left);
        Front = new PrecisionHeightMapScript(front.width, front.height, TileX, TileY, front);
        Back = new PrecisionHeightMapScript(back.width, back.height, TileX, TileY, back); 

        Height = 0.01f;
    }

    public Vector3d GetCornerCoords(CuboidCornerType cornerType, double edgeLength, GridFaceType faceType = GridFaceType.NONE)
    {
        Vector3d coords = new Vector3d();

        if(cornerType == CuboidCornerType.TRB)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3d(1 - edgeLength, 1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3d(1, 1 - edgeLength, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3d(1 - edgeLength, 1 - edgeLength, -1);
            }

        //    coords = new Vector3d(1, 1, -1);
        }
        else
        if(cornerType == CuboidCornerType.TLB)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3d(-1 + edgeLength, 1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3d(-1, 1 - edgeLength, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3d(-1 + edgeLength, 1 - edgeLength, -1);
            }            
        //    coords = new Vector3d(-1, 1, -1);
        }
        else
        if(cornerType == CuboidCornerType.TRF)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3d(1 - edgeLength, 1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3d(1, 1 - edgeLength, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3d(1 - edgeLength, 1 - edgeLength, 1);
            } 

//            coords = new Vector3d(1, 1, 1);
        }
        else
        if(cornerType == CuboidCornerType.TLF)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3d(-1 + edgeLength, 1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3d(-1, 1 - edgeLength, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3d(-1 + edgeLength, 1 - edgeLength, 1);
            } 

        //    coords = new Vector3d(-1, 1, 1);
        }
        else
        if(cornerType == CuboidCornerType.BRB)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3d(1 - edgeLength, -1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3d(1, -1 + edgeLength, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3d(1 - edgeLength, -1 + edgeLength, -1);
            } 

         //   coords = new Vector3d(1, -1, -1);
        }
        else
        if(cornerType == CuboidCornerType.BLB)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3d(-1 + edgeLength, -1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3d(-1, -1 + edgeLength, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3d(-1 + edgeLength, -1 + edgeLength, -1);
            } 

        //    coords = new Vector3d(-1, -1, -1);
        }
        else
        if(cornerType == CuboidCornerType.BRF)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3d(1 - edgeLength, -1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3d(1, -1 + edgeLength, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3d(1 - edgeLength, -1 + edgeLength, 1);
            } 

         //   coords = new Vector3d(1, -1, 1);
        }
        else
        if(cornerType == CuboidCornerType.BLF)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3d(-1 + edgeLength, -1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3d(-1, -1 + edgeLength, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3d(-1 + edgeLength, -1 + edgeLength, 1);
            } 

       //     coords = new Vector3d(-1, -1, 1);
        }         

        return coords;
    }

    public CuboidCornerType GetCornerType(Vector3d pos)
    {
        CuboidCornerType cornerType = CuboidCornerType.NONE;

        if(Mathd.Approximately(pos.x, 1.0f) == true && Mathd.Approximately(pos.y, 1.0f) == true && 
           Mathd.Approximately(pos.z, -1.0f) == true)
        {
            cornerType = CuboidCornerType.TRB; // top right back
        }
        else
        if(Mathd.Approximately(pos.x, -1.0f) == true && Mathd.Approximately(pos.y, 1.0f) == true && 
           Mathd.Approximately(pos.z, -1.0f) == true)
        {
            cornerType = CuboidCornerType.TLB; // top left back
        }
        else
        if(Mathd.Approximately(pos.x, 1.0f) == true && Mathd.Approximately(pos.y, 1.0f) == true && 
           Mathd.Approximately(pos.z, 1.0f) == true)
        {
            cornerType = CuboidCornerType.TRF; // top right front
        }
        else
        if(Mathd.Approximately(pos.x, -1.0f) == true && Mathd.Approximately(pos.y, 1.0f) == true && 
           Mathd.Approximately(pos.z, 1.0f) == true)
        {
            cornerType = CuboidCornerType.TLF; // top left front
        }
        else
        if(Mathd.Approximately(pos.x, 1.0f) == true && Mathd.Approximately(pos.y, -1.0f) == true && 
           Mathd.Approximately(pos.z, -1.0f) == true)
        {
            cornerType = CuboidCornerType.BRB; // bottom right back
        }
        else
        if(Mathd.Approximately(pos.x, -1.0f) == true && Mathd.Approximately(pos.y, -1.0f) == true && 
           Mathd.Approximately(pos.z, -1.0f) == true)
        {
            cornerType = CuboidCornerType.BLB; // bottom left back
        }
        else
        if(Mathd.Approximately(pos.x, 1.0f) == true && Mathd.Approximately(pos.y, -1.0f) == true && 
           Mathd.Approximately(pos.z, 1.0f) == true)
        {
            cornerType = CuboidCornerType.BRF; // bottom right front
        }
        else
        if(Mathd.Approximately(pos.x, -1.0f) == true && Mathd.Approximately(pos.y, -1.0f) == true && 
           Mathd.Approximately(pos.z, 1.0f) == true)
        {
            cornerType = CuboidCornerType.BLF; // bottom left front
        }

        return cornerType;
    }

    public Vector3d GetEdgeCoords(CuboidEdgeType edgeType, Vector3d pos, double edgeLength, GridFaceType faceType = GridFaceType.NONE)
    {
        Vector3d coords = new Vector3d();

        if(edgeType == CuboidEdgeType.TB)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3d(pos.x, 1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3d(pos.x, 1 - edgeLength, -1);
            }
           // coords = new Vector3d(pos.x, 1, -1);
        }
        else
        if(edgeType == CuboidEdgeType.TR)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3d(1 - edgeLength, 1, pos.z);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3d(1, 1 - edgeLength, pos.z);
            }            
            //  coords = new Vector3d(1, 1, pos.z);
        }        
        else
        if(edgeType == CuboidEdgeType.TF)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3d(pos.x, 1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3d(pos.x, 1 - edgeLength, 1);
            }              
       //     coords = new Vector3d(pos.x, 1, 1);
        }        
        else
        if(edgeType == CuboidEdgeType.TL)
        {
            if(faceType == GridFaceType.TOP)
            {
                coords = new Vector3d(-1 + edgeLength, 1, pos.z);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3d(-1, 1 - edgeLength, pos.z);
            }              
         //   coords = new Vector3d(-1, 1, pos.z);
        }        
        else
        if(edgeType == CuboidEdgeType.BB)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3d(pos.x, -1, -1 + edgeLength);
            }
            else
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3d(pos.x, -1 + edgeLength, -1);
            }              
          //  coords = new Vector3d(pos.x, -1, -1);
        }        
        else
        if(edgeType == CuboidEdgeType.BR)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3d(1 - edgeLength, -1, pos.z);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3d(1, -1 + edgeLength, pos.z);
            }              
         //   coords = new Vector3d(1, -1, pos.z);
        }        
        else
        if(edgeType == CuboidEdgeType.BF)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3d(pos.x, -1, 1 - edgeLength);
            }
            else
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3d(pos.x, -1 + edgeLength, 1);
            }              
          //  coords = new Vector3d(pos.x, -1, 1);
        }        
        else
        if(edgeType == CuboidEdgeType.BL)
        {
            if(faceType == GridFaceType.BOTTOM)
            {
                coords = new Vector3d(-1 + edgeLength, -1, pos.z);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3d(-1, -1 + edgeLength, pos.z);
            }              
         //   coords = new Vector3d(-1, -1, pos.z);
        }        
        else
        if(edgeType == CuboidEdgeType.BKR)
        {
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3d(1 - edgeLength, pos.y, -1);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3d(1, pos.y, -1 + edgeLength);
            }              
         //   coords = new Vector3d(1, pos.y, -1);
        }        
        else
        if(edgeType == CuboidEdgeType.BKL)
        {
            if(faceType == GridFaceType.BACK)
            {
                coords = new Vector3d(-1 + edgeLength, pos.y, -1);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3d(-1, pos.y, -1 + edgeLength);
            }              
        //    coords = new Vector3d(-1, pos.y, -1);
        }        
        else
        if(edgeType == CuboidEdgeType.FR)
        {
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3d(1 - edgeLength, pos.y, 1);
            }
            else
            if(faceType == GridFaceType.RIGHT)
            {
                coords = new Vector3d(1, pos.y, 1 - edgeLength);
            }              
      //      coords = new Vector3d(1, pos.y, 1);
        }        
        else
        if(edgeType == CuboidEdgeType.FL)
        {
            if(faceType == GridFaceType.FRONT)
            {
                coords = new Vector3d(-1 + edgeLength, pos.y, 1);
            }
            else
            if(faceType == GridFaceType.LEFT)
            {
                coords = new Vector3d(-1, pos.y, 1 - edgeLength);
            }              
         //   coords = new Vector3d(-1, pos.y, 1);
        }        

        return coords;
    }

    public CuboidEdgeType GetEdgeType(Vector3d pos)
    {
        CuboidEdgeType edgeType = CuboidEdgeType.NONE;

        if(Mathd.Approximately(pos.y, 1.0f) == true && Mathd.Approximately(pos.z, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.TB;
        }
        else
        if(Mathd.Approximately(pos.x, 1.0f) == true && Mathd.Approximately(pos.y, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.TR;
        }        
        else
        if(Mathd.Approximately(pos.y, 1.0f) == true && Mathd.Approximately(pos.z, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.TF;
        }        
        else
        if(Mathd.Approximately(pos.x, -1.0f) == true && Mathd.Approximately(pos.y, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.TL;
        }        
        else
        if(Mathd.Approximately(pos.y, -1.0f) == true && Mathd.Approximately(pos.z, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BB;
        }        
        else
        if(Mathd.Approximately(pos.x, 1.0f) == true && Mathd.Approximately(pos.y, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BR;
        }        
        else
        if(Mathd.Approximately(pos.y, -1.0f) == true && Mathd.Approximately(pos.z, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.BF;
        }        
        else
        if(Mathd.Approximately(pos.x, -1.0f) == true && Mathd.Approximately(pos.y, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BL;
        }        
        else
        if(Mathd.Approximately(pos.x, 1.0f) == true && Mathd.Approximately(pos.z, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BKR;
        }        
        else
        if(Mathd.Approximately(pos.x, -1.0f) == true && Mathd.Approximately(pos.z, -1.0f) == true)
        {
            edgeType = CuboidEdgeType.BKL;
        }        
        else
        if(Mathd.Approximately(pos.x, 1.0f) == true && Mathd.Approximately(pos.z, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.FR;
        }        
        else
        if(Mathd.Approximately(pos.x, -1.0f) == true && Mathd.Approximately(pos.z, 1.0f) == true)
        {
            edgeType = CuboidEdgeType.FL;
        }        

        return edgeType;
    }

    public double GetEdgeHeightValue(CuboidEdgeType edgeType, Vector3d pos, double edgeLength)
    {
        double height = 0;

        Vector2d uv1 = new Vector2d();
        Vector2d uv2 = new Vector2d();        

        PrecisionHeightMapScript face1 = null;
        PrecisionHeightMapScript face2 = null;

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

        double h1 = face1.GetHeightValue(uv1.x, uv1.y) * Height;
        double h2 = face2.GetHeightValue(uv2.x, uv2.y) * Height;

        height = (h1 + h2) / 2;

        return height;
    }

    public double GetCornerHeightValue(CuboidCornerType cornerType, double edgeLength)
    {
        double height = 0;

        Vector2d uv1 = new Vector2d();
        Vector2d uv2 = new Vector2d();
        Vector2d uv3 = new Vector2d();

        PrecisionHeightMapScript face1 = null;
        PrecisionHeightMapScript face2 = null;
        PrecisionHeightMapScript face3 = null;

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

        double h1 = face1.GetHeightValue(uv1.x, uv1.y) * Height;
        double h2 = face2.GetHeightValue(uv2.x, uv2.y) * Height;
        double h3 = face3.GetHeightValue(uv3.x, uv3.y) * Height;

        height = (h1 + h2 + h3) / 3;

        return height;
    }

    public double GetFaceHeightValue(GridFaceType faceType, Vector2d uv, double edgeLength)
    {
        PrecisionHeightMapScript selectedFace = null;

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

        // Vector2d v1 = new Vector2d(uv.x, uv.y);
        // Vector2d v2 = new Vector2d(uv.x + edgeLength, uv.y);
        // Vector2d v3 = new Vector2d(uv.x, uv.y + edgeLength);
        // Vector2d v4 = new Vector2d(uv.x + edgeLength, uv.y + edgeLength);

        // double h1 = selectedFace.GetHeightValue(v1.x, v1.y);
        // double h2 = selectedFace.GetHeightValue(v2.x, v2.y);
        // double h3 = selectedFace.GetHeightValue(v3.x, v3.y);
        // double h4 = selectedFace.GetHeightValue(v4.x, v4.y);

        // double tx = uv.x - Mathd.Floor(uv.x);
        // double ty = uv.y - Mathd.Floor(uv.y);

        // return this.GetBilinearValue(tx, ty, h1, h2, h3, h4) * Height;
        return selectedFace.GetHeightValue(uv.x, uv.y) * Height;
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

    public Vector2d GetFaceUV(Vector3d pos, GridFaceType faceType)
    {
        double u = 0, v = 0;

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

        return new Vector2d(u, v);
    }

    public Vector3d GetHeightValue(Vector3d pos, GridFaceType faceType, double edgeLength)
    {
        double height = 0;
        Vector2d uv = new Vector2d();

        uv = this.GetFaceUV(pos, faceType);

        double absX = Mathd.Abs(pos.x);
        double absY = Mathd.Abs(pos.y);
        double absZ = Mathd.Abs(pos.z);

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

        Vector2d cuv = new Vector2d();

        cuv = this.GetFaceUV(pos, faceType);

        if(cornerType != CuboidCornerType.NONE)
        {
            height = this.GetCornerHeightValue(cornerType, edgeLength);
        }
        else
        if(cornerType == CuboidCornerType.NONE)
        {
            CuboidEdgeType edgeType = this.GetEdgeType(pos);

            if(edgeType != CuboidEdgeType.NONE)
            {
                height = this.GetEdgeHeightValue(edgeType, pos, edgeLength);
            }
            else
            {
                height = this.GetFaceHeightValue(faceType, cuv, edgeLength);
            }
        }

        return new Vector3d(uv.x, uv.y, height);
    }
}
