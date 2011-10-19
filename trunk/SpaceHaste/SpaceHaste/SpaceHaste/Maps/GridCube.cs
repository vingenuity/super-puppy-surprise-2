using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;

namespace SpaceHaste.Maps
{
    public class GridCube
    {
        public static float GRIDSQUARELENGTH = 500;
        public int X, Y, Z;
        public Vector3 Position;
        public Vector3 Center;
        private GameObject ContainedObject;
        public List<GridCube> ConnectedGridSquares;
        public enum TerrainType { none = 0, asteroid, nebula, wreck }
        private TerrainType Terrain;

        public GridCube(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            Position = new Vector3(X, Y, Z);
            Center = new Vector3(X + GRIDSQUARELENGTH / 2, Y + GRIDSQUARELENGTH / 2, +Z + GRIDSQUARELENGTH / 2);
            ContainedObject = null;
            ConnectedGridSquares = new List<GridCube>();
            Terrain = TerrainType.none;
        }

        public void AddObject(GameObject obj)
        {
            ContainedObject = obj;
        }

        public bool HasObject()
        {
            if (ContainedObject == null)
                return false;
            return true;
        }

        public void RemoveObject(GameObject obj)
        {
            ContainedObject = null;
        }

        public TerrainType getTerrain() { return Terrain; }
        public void setTerrain(TerrainType t) { Terrain = t; }
    }
}
