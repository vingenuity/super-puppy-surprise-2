using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class MapManager : GameComponent
    {
        public static Map Map;
        public MapManager(Game game)
            : base(game)
        {
            Map = new Map1();
        }
        public override void Update(GameTime gameTime)
        {
            if (Controls.ControlManager.camera.getVerticalAngle() > 1.54)
            {
                double a = Controls.ControlManager.camera.getVerticalAngle();
                Map.RemoveXZMatrix();
                Map.AddGridX1Z();
            } else {
                double a = Controls.ControlManager.camera.getVerticalAngle();
                Map.RemoveXZMatrix();
                Map.AddGridX0Z();
            }

            if (Controls.ControlManager.camera.getHorizontalAngle() > 1.57
             && Controls.ControlManager.camera.getHorizontalAngle() < 1.57 + Math.PI)
            {
                Map.RemoveXYMatrix();
                Map.AddGridXY1();
            } else 
            {
                Map.RemoveXYMatrix();
                Map.AddGridXY0();
            }

            if (Controls.ControlManager.camera.getHorizontalAngle() > 0 
             && Controls.ControlManager.camera.getHorizontalAngle() < Math.PI)
            {
                Map.RemoveYZMatrix();
                Map.AddGrid0YZ();
            }
            else
            {
                Map.RemoveYZMatrix();
                Map.AddGrid1YZ();
            }

            base.Update(gameTime);
        }
    }
}
