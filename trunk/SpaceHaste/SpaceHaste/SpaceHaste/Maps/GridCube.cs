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
        private List<GameObject> ContainedObjects;
        public List<GridCube> ConnectedGridSquares;
        public enum TerrainType { none = 0, asteroid, nebula, wreck }
        private TerrainType Terrain;

        public GridCube(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            Position = new Vector3(X, Y, Z);
            Center = new Vector3(X + GRIDSQUARELENGTH / 2, Y + GRIDSQUARELENGTH / 2, + Z + GRIDSQUARELENGTH / 2);
            ContainedObjects = new List<GameObject>();
            ConnectedGridSquares = new List<GridCube>();
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

        /// <summary>
        /// This is a starting block for the function below.
        /// </summary>
        /// <param name="range"> Maximum distance to check.</param>
        /// <returns>List of valid squares in range.</returns>
        public List<GridCube> GetGridSquaresInRange(int range)
        {
            return GetGridSquaresInRange(this, range);
        }

        /// <summary>
        /// Finds the number of grid squares in range of a particular grid square.
        /// This will presumably be used to find valid moves for each ship.
        /// </summary>
        /// <param name="gs">Grid square to start the search.</param>
        /// <param name="range">Distance of squares away to search.</param>
        /// <returns>List of valid grid squares in range.</returns>
        private List<GridCube> GetGridSquaresInRange(GridCube gs, int range)
        {
            List<GridCube> inRange = new List<GridCube>();
            if (range == 0) return inRange;
            foreach (GridCube neighbor in gs.ConnectedGridSquares)
            {
                //Can't cross through asteroid
                if (neighbor.getTerrain() == GridCube.TerrainType.asteroid)
                    continue;
                //Movement penalty for nebulae
                if (neighbor.getTerrain() == GridCube.TerrainType.nebula)
                    inRange.AddRange(GetGridSquaresInRange(neighbor, range - 2));
                //Otherwise, take normal movement
                else
                    inRange.AddRange(GetGridSquaresInRange(neighbor, range - 1));
            }
            inRange = inRange.Distinct().ToList();
            return inRange;
        }
    }
}
