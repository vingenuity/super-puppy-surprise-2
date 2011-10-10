using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

//This class holds position and any other information necessary for rendering out star and other visual asset bilboards in 3d space
namespace SpaceHaste.Graphics
{
    public class starz
    {
        List<Vector3> StarPoints;       //Holds star positions  StarPoints = new List<Vector

        public starz() {
            StarPoints = new List<Vector3>();

        }
        public void AddStars()
        {
            //StarPoints.Add(new Vector3(4,4,4));
        }
    }
}
