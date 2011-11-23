using System;
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
            Map = new Map1();
        }
        
        public override void Update(GameTime gameTime)
        {
            //double horizontalAngle = Controls.ControlManager.camera.getHorizontalAngle();
            //if (horizontalAngle > 1.54)
            //{
            //    gridX1Z = true;
            //    gridX0Z = false;
            //}
            //else
            //{
            //    gridX0Z = true;
            //    gridX1Z = false;
            //}

            //double verticalAngle = Controls.ControlManager.camera.getVerticalAngle();
            //if (horizontalAngle > 1.57 && horizontalAngle < 1.57 + Math.PI)
            //{
            //    gridXY1 = true;
            //    gridXY0 = false;
            //}
            //else 
            //{
            //    gridXY0 = true;
            //    gridXY1 = false;
            //}

            //if (horizontalAngle > 0 && horizontalAngle < Math.PI)
            //{
            //    grid0YZ = true;
            //    grid1YZ = false;
            //}
            //else
            //{
            //    grid1YZ = true;
            //    grid0YZ = false;
            //}

            if (Controls.ControlManager.camera.getVerticalAngle() > 1.54)
            {
                double a = Controls.ControlManager.camera.getVerticalAngle();
                Map.RemoveXZMatrix();
                Map.AddGridX1Z();

            }
            else
            {
                double a = Controls.ControlManager.camera.getVerticalAngle();
                Map.RemoveXZMatrix();
                Map.AddGridX0Z();

            }

            if (Controls.ControlManager.camera.getHorizontalAngle() > 1.57
             && Controls.ControlManager.camera.getHorizontalAngle() < 1.57 + Math.PI)
            {
                Map.RemoveXYMatrix();
                Map.AddGridXY1();
                isDrawingZGridBottom = false;
            }
            else
            {
                Map.RemoveXYMatrix();
                Map.AddGridXY0();
                isDrawingZGridBottom = true;
            }

            if (Controls.ControlManager.camera.getHorizontalAngle() > 0
             && Controls.ControlManager.camera.getHorizontalAngle() < Math.PI)
            {
                Map.RemoveYZMatrix();
                Map.AddGrid0YZ();
                isDrawingXGridBottom = false;
            }
            else
            {
                Map.RemoveYZMatrix();
                Map.AddGrid1YZ();
                isDrawingXGridBottom = true;
            }

                base.Update(gameTime);
        }
    }
}
