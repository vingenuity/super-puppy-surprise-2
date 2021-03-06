﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;

namespace SpaceHaste.Maps
{
    public class GridCube
    {
        public double distance;
        public static float GRIDSQUARELENGTH = 500;
        public int X, Y, Z;
        public Vector3 Position;
        public Vector3 Center;
        public GameObject ContainedObject;
        public List<GridCube> ConnectedGridSquares;
        public enum TerrainType { none = 1, asteroid=1000, nebula=2, wreck=2000, planet=3, nearplanet=4 }
        public BoundingSphere boundingSphere; 
        private TerrainType Terrain;

        //For AI pathfinding
        public GridCube came_from { get; set; }
        public float f_score { get; set; }
        public float g_score { get; set; }
        public float h_score { get; set; }
        
        public GridCube(int X, int Y, int Z)
        {
            distance = 1000;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            Position = new Vector3(X, Y, Z);
            Center = new Vector3(X + GRIDSQUARELENGTH / 2, Y + GRIDSQUARELENGTH / 2, +Z + GRIDSQUARELENGTH / 2);
            ContainedObject = null;
            ConnectedGridSquares = new List<GridCube>();
            Terrain = TerrainType.none;
            boundingSphere = new BoundingSphere(Position, 0f);
        }
        //Backtrack to get the Path to the GridCube
        public GridCube PreviousGridSquare;

        //Conversion
        public Vector3 AsVector() { return new Vector3(X, Y, Z); }

        //Draw Function
        public void Draw() { }

        //Object Functions
        public void AddObject(GameObject obj) { ContainedObject = obj; }
        public GameObject GetObject() { return ContainedObject; }
        public bool HasObject() { return (ContainedObject == null) ? false : true; }
        public void RemoveObject(GameObject obj) { ContainedObject = null; }

        //Terrain Functions
        public double GetMoveCost() 
        {
            if (Terrain == TerrainType.nearplanet)
                return .333;
            else return 1;
        }
        public TerrainType GetTerrain() { return Terrain; }
        public void SetTerrain(TerrainType t) { 
            Terrain = t;
            if (Terrain == TerrainType.asteroid || Terrain == TerrainType.wreck)
                boundingSphere = new BoundingSphere(Position, 0.5f);
        }

        /// <summary>
        /// Backtrack to get the Path to the GridCube
        /// </summary>
        public List<Vector3> GetPath()
        {
            List<Vector3> Path = new List<Vector3>();
            GridCube l = PreviousGridSquare;
            while (l != null)
            {
                Path.Insert(0,l.Position);
                l = l.PreviousGridSquare;
            }
            return Path;
        }
        public void SetPath(GridCube PreviousGridSquare)
        {
            this.PreviousGridSquare = PreviousGridSquare;
        }

        public bool BlocksMovement()
        {
            if (this.Terrain == TerrainType.wreck || this.Terrain == TerrainType.nearplanet  || this.Terrain == TerrainType.planet || this.Terrain == TerrainType.asteroid || this.HasObject())
                return true;
            return false;
        }
    }

    
}
