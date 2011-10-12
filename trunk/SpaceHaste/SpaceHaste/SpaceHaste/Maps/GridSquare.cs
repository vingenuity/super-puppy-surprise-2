using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;

namespace SpaceHaste.Maps
{
    public class GridSquare
    {
        public static float GRIDSQUARELENGTH = 500;
        public int X, Y, Z;
        public Vector3 Position;
        public Vector3 Center;
        private List<GameObject> ContainedObjects;
        public List<GridSquare> ConnectedGridSquares;
        public enum TerrainType { none = 0, asteroid, nebula, wreck }
        private TerrainType Terrain;

        public GridSquare(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            Position = new Vector3(X, Y, Z);
            Center = new Vector3(X + GRIDSQUARELENGTH / 2, Y + GRIDSQUARELENGTH / 2, + Z + GRIDSQUARELENGTH / 2);
            ContainedObjects = new List<GameObject>();
            ConnectedGridSquares = new List<GridSquare>();
            Terrain = TerrainType.none;
        }

        public void AddObject(GameObject obj)
        {
            ContainedObjects.Add(obj);
        }

        public bool HasObject()
        {
            if(ContainedObjects.Count() == 0)
                return false;
            return true;
        }

        public void RemoveObject(GameObject obj)
        {
            ContainedObjects.Remove(obj);
        }

        public TerrainType getTerrain() { return Terrain; }
        public void setTerrain(TerrainType t) { Terrain = t; }
    }
}
