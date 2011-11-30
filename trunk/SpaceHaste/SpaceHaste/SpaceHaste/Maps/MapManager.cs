﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class MapManager : GameComponent
    {
        bool gridXY0;
        bool gridXY1;
        bool gridX0Z;
        bool gridX1Z;
        bool grid0YZ;
        bool grid1YZ; 
        bool gridXY0was;
        bool gridXY1was;
        bool gridX0Zwas;
        bool gridX1Zwas;
        bool grid0YZwas;
        bool grid1YZwas;
        int gridXYwas;
        int gridXZwas;
        int gridYZwas;
        public static Map Map;
        public static bool isDrawingXGridBottom = false;
        public static bool isDrawingZGridBottom = false;
        public MapManager(Game game)
            : base(game)
        {
            gridXY0 = false;
            gridXY1 = false;
            gridX0Z = false;
            gridX1Z = false;
            grid0YZ = false;
            grid1YZ = false;
            gridXY0was = true;
            gridXY1was = true;
            gridX0Zwas = true;
            gridX1Zwas = true;
            grid0YZwas = true;
            grid1YZwas = true;
            Map = new Map1();
            Map.AddGrid1YZ();
            Map.AddGrid0YZ();
            Map.AddGridX0Z();
            Map.AddGridX1Z();
            Map.AddGridXY0();
            Map.AddGridXY1();
        }
        
        public override void Update(GameTime gameTime)
        {
            double horizontalAngle = Controls.ControlManager.camera.getHorizontalAngle();
            double verticalAngle = Controls.ControlManager.camera.getVerticalAngle();

            if (horizontalAngle > 0.34f && horizontalAngle < 2.80f)
                 grid1YZ = false;
            else grid1YZ = true;
            if (horizontalAngle > 3.48f && horizontalAngle < 5.91f)
                 grid0YZ = false;
            else grid0YZ = true;
            if (horizontalAngle > 1.22f && horizontalAngle < 5.06f)
                 gridXY0 = false;
            else gridXY0 = true;
            if (horizontalAngle < 1.91f || horizontalAngle > 4.37f)
                 gridXY1 = false;
            else gridXY1 = true;

            if (grid0YZ && !grid0YZwas)
            {
                Map.AddGrid0YZ();
                grid0YZwas = true;
            }
            if (!grid0YZ && grid0YZwas)
            {
                Map.Remove0YZMatrix();
                grid0YZwas = false;
            }
            if (grid1YZ && !grid1YZwas)
            {
                Map.AddGrid1YZ();
                grid1YZwas = true;
            }
            if (!grid1YZ && grid1YZwas)
            {
                Map.Remove1YZMatrix();
                grid1YZwas = false;
            }

            if (gridXY0 && !gridXY0was)
            {
                Map.AddGridXY0();
                gridXY0was = true;
            }
            if (!gridXY0 && gridXY0was)
            {
                Map.RemoveXY0Matrix();
                gridXY0was = false;
            }
            if (gridXY1 && !gridXY1was)
            {
                Map.AddGridXY1();
                gridXY1was = true;
            }
            if (!gridXY1 && gridXY1was)
            {
                Map.RemoveXY1Matrix();
                gridXY1was = false;
            }

            base.Update(gameTime);
        }
    }
}
