using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using SpaceHaste.Primitives;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;
using SpaceHaste.Graphics;

namespace SpaceHaste.Maps
{
    public class Map
    {
        public GridCube[, ,] MapGridCubes;
        protected GridCube[, ,] BottomMap;

        protected Line[,] XYMatrix;
        protected Line[,] XZMatrix;
        protected Line[,] YZMatrix;

        public List<GridCube> EnvMapObjects;
        public List<int> EnvMapObjectsRandomNum;
        public List<GameObject> ShipMapObjects;
        public static List<NebulaParticle2> Nebulae;
        public Vector3 Size;

        public static Map map;

        public int Act = 1;
        public int Scene = 1;

        public Map(Vector3 Size)
        {
            this.Size = Size;
            ShipMapObjects = new List<GameObject>();
            EnvMapObjects = new List<GridCube>();
            EnvMapObjectsRandomNum = new List<int>();
            Nebulae = new List<NebulaParticle2>();
            XYMatrix = new Line[4, Math.Max((int)Size.X + 1, (int)Size.Y + 1)];
            XZMatrix = new Line[4, Math.Max((int)Size.X + 1, (int)Size.Z + 1)];
            YZMatrix = new Line[4, Math.Max((int)Size.Y + 1, (int)Size.Z + 1)];

            InitMapGridCubes();
            InitMapGameObjects();
            map = this;
           
        }

        public void addGameObject(GameObject go, Vector3 position) {
            MapGridCubes[(int)position.X, (int)position.Y, (int)position.Z].AddObject(go);
            go.GridPosition = position;
            go.DrawPosition = MapGridCubes[(int)position.X, (int)position.Y, (int)position.Z].Center;
            go.GridLocation = MapGridCubes[(int)position.X, (int)position.Y, (int)position.Z];
            ShipMapObjects.Add(go);
        }
        public void addGameObject(GameObject go)
        {
            MapGridCubes[(int)go.GridPosition.X, (int)go.GridPosition.Y, (int)go.GridPosition.Z].AddObject(go);
            go.DrawPosition = MapGridCubes[(int)go.GridPosition.X, (int)go.GridPosition.Y, (int)go.GridPosition.Z].Center;
            go.GridLocation = MapGridCubes[(int)go.GridPosition.X, (int)go.GridPosition.Y, (int)go.GridPosition.Z];
            ShipMapObjects.Add(go);
        }
        public void removeGameObject(GameObject go)
        {

            ShipMapObjects.Remove(go);
        }
        public void AddEnvObject(GridCube.TerrainType e, int x, int y, int z)
        {
            switch (e)
            {
                case GridCube.TerrainType.asteroid:
                    MapGridCubes[x, y, z].SetTerrain(GridCube.TerrainType.asteroid);
                    EnvMapObjects.Add(MapGridCubes[x, y, z]);
                    EnvMapObjectsRandomNum.Add(GraphicsManager.random.Next(6));
                    break;
                case GridCube.TerrainType.nebula:
                    MapGridCubes[x, y, z].SetTerrain(GridCube.TerrainType.nebula);
                    Nebulae.Add(NebulaParticle2.CreateParticle( MapGridCubes[x, y, z].Center));
                    EnvMapObjectsRandomNum.Add(GraphicsManager.random.Next(0));
                    break;
                case GridCube.TerrainType.wreck:
                    EnvMapObjects.Add(MapGridCubes[x, y, z]);
                    MapGridCubes[x, y, z].SetTerrain(GridCube.TerrainType.wreck);
                    ParticleManager.Instance.Add(new ShipWreckageParticle(MapGridCubes[x, y, z].Center));
                    //EnvMapObjectsRandomNum.Add(GraphicsManager.random.Next(0));
                    break;
            };
           
        }
        //please add adjacent cubes as "nearplanet"
        /// <summary>
        /// Adds planets to grid coords with a scale in grid coords
        /// Planet goes from x to x + length in grid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="scale"></param>
        public void AddPlanet(int x, int y, int z, float length)
        {
            for(int i = x; i < x+length; i++)
                for(int j = y; j < y+length; j++)
                    for (int k = z; k < z+ length; k++)
                    {
                        try
                        {
                            MapGridCubes[i, j, k].SetTerrain(GridCube.TerrainType.planet);
                            EnvMapObjects.Add(MapGridCubes[i, j, k]);
                            EnvMapObjectsRandomNum.Add(GraphicsManager.random.Next(0));
                            
                        }
                        catch { }
                    }
            Graphics.GraphicsManager.Planets.Add(new Tuple<Vector3, float>(new Vector3((float)x, (float)y, (float)z), length));
        }

        public void AddGameObjectToGridSquare(GameObject gameObject, int x, int y, int z)
        {
            gameObject.GridLocation = MapGridCubes[x, y, z];
            gameObject.GridPosition = MapGridCubes[x, y, z].Center;
        }

        public void colorGrids()
        {
            foreach (GameObject go in ShipMapObjects)
            {
            }
        }

        protected virtual void InitMapGameObjects() 
        {
        }
        void InitMapGridCubes()
        {
            Vector3 bounds = -GridCube.GRIDSQUARELENGTH * Size / 2 + GridCube.GRIDSQUARELENGTH/2 * new Vector3(1,1,1);

            MapGridCubes = new GridCube[(int)Size.X, (int)Size.Y, (int)Size.Z];
            for (int i = 0; i < Size.X; i++)
                for (int j = 0; j < Size.Y; j++)
                    for (int k = 0; k < Size.Z; k++)
                    {
                        MapGridCubes[i, j, k] = new GridCube(i, j, k);
                        MapGridCubes[i, j, k].Center = new Vector3(bounds.X + GridCube.GRIDSQUARELENGTH * i,
                            bounds.Y + GridCube.GRIDSQUARELENGTH * j,
                            bounds.Z + GridCube.GRIDSQUARELENGTH * k);
                    }

            BottomMap = new GridCube[(int)Size.X, 1, (int)Size.Z];
            for (int i = 0; i < Size.X; i++)
                for (int k = 0; k < Size.Z; k++)
                {
                    int j = 0;
                    BottomMap[i, j, k] = new GridCube(i, j, k);
                    BottomMap[i, j, k].Center = new Vector3(bounds.X + GridCube.GRIDSQUARELENGTH * i,
                        bounds.Y + GridCube.GRIDSQUARELENGTH * j,
                        bounds.Z + GridCube.GRIDSQUARELENGTH * k);
                }
            ConnectGridSquares();
        }

        /// <summary>
        /// This is primarily for the lasers
        /// axis-bound distance check eliminates game objects out of range
        /// a ray test checks if objects are blocked
        /// </summary>
        /// <param name="go"> selected game object, trying to fire a laser</param>
        /// <param name="target"> targeted object </param>
        /// <returns> true if the target is within laser range of the go</returns>
        public Boolean IsObjectInRange(GameObject go, GameObject target)
        {
            int d = (int)Math.Abs(go.GridPosition.X - target.GridPosition.X) +
                    (int)Math.Abs(go.GridPosition.Y - target.GridPosition.Y) +
                    (int)Math.Abs(go.GridPosition.Z - target.GridPosition.Z);
            if (d > go.LaserRange)
                return false;

            Vector3 rayDirection = target.GridPosition - go.GridPosition;
            rayDirection.Normalize();
            Ray ray = new Ray(go.GridPosition, rayDirection);
            float? distance = ray.Intersects(target.boundingSphere);
            float? r = 0;

            for (int i = 0; i < EnvMapObjects.Count; i++)
            {
                if ((int)EnvMapObjects[i].GetTerrain() == 2)
                    continue;
                r = ray.Intersects(EnvMapObjects[i].boundingSphere);
                if (r == null || r < 0 || r > distance) 
                    continue;
                else 
                    return false; // 1 < r < d
            }

            for (int i = 0; i < ShipMapObjects.Count; i++)
            {
                r = ray.Intersects(ShipMapObjects[i].boundingSphere);
                if (r == null || r < 0 || r > distance) continue;
                else break; // 1 < r < d
            }

            if (!(r > 0 && r < distance) || r == null)
                return true;
            else return false;
        }
        public Boolean IsTargetCubeInRange(GridCube loc, GridCube target)
        {
            int range = (int)Math.Abs(loc.X - target.X) +
                        (int)Math.Abs(loc.Y - target.Y) +
                        (int)Math.Abs(loc.Z - target.Z);
            //Create queues and initialize
            RefreshGridSearch();
            List<GridCube> inRange = new List<GridCube>();
            Queue<GridCube> GridQueue = new Queue<GridCube>();
            GridQueue.Enqueue(loc);
            inRange.Add(loc);
            loc.distance = 0;
            loc.SetPath(null);
            while (GridQueue.Count != 0)
            {
                GridCube gc = GridQueue.Dequeue();
                if (gc.distance >= range) continue;
                foreach (GridCube neighbor in gc.ConnectedGridSquares)
                {
                    if (neighbor.GetTerrain() != GridCube.TerrainType.none)
                        continue;
                    if (gc.distance + neighbor.GetMoveCost() < neighbor.distance)
                    {
                        neighbor.distance = gc.distance + neighbor.GetMoveCost();
                        neighbor.SetPath(gc);
                        GridQueue.Enqueue(neighbor);
                        inRange.Add(neighbor);
                    }
                }
            }
            if (inRange.Contains(target))
                return true;
            else return false;
        }

        //change to allow near planet objects
        /// <summary>
        /// Finds the number of grid squares in range of a particular grid square.
        /// This will presumably be used to find valid moves for each ship.
        /// This function does a breth first search
        /// </summary>
        /// <param name="loc">Grid square to start the search.</param>
        /// <param name="range">Distance of squares away to search.</param>
        /// <returns>List of valid grid squares in range.</returns>
        public List<GridCube> GetGridCubesInRange(GridCube loc, int range)
        {
            //Create queues and initialize
            RefreshGridSearch();
            List<GridCube> inRange = new List<GridCube>();
            Queue<GridCube> GridQueue = new Queue<GridCube>();
            GridQueue.Enqueue(loc);
            inRange.Add(loc);
            loc.distance = 0;
            loc.SetPath(null); 
            while(GridQueue.Count != 0)
            {
                GridCube gc = GridQueue.Dequeue();
                if (gc.distance >= range) continue;
                foreach (GridCube neighbor in gc.ConnectedGridSquares)
                {
                    if (neighbor.HasObject() || neighbor.GetTerrain() != GridCube.TerrainType.none)
                        continue;
                    if (gc.distance + neighbor.GetMoveCost() < neighbor.distance)
                    {
                        neighbor.distance = gc.distance + neighbor.GetMoveCost();
                        neighbor.SetPath(gc);
                        GridQueue.Enqueue(neighbor);
                        inRange.Add(neighbor);
                    }
                }
            }
            return inRange;
        }
        /// <summary>
        /// Finds the number of grid squares in range of a grid square x, y, z.
        /// This extends the functionality of the above function for more general use.
        /// This function calls GetGridSquaresInRange for the GridCube at location loc
        /// </summary>
        /// <param name="loc"> location to start the search.</param>
        /// <param name="range"> number of neighboring squares to search.</param>
        /// <returns>List of valid grid squares in range.</returns>
        public List<GridCube> GetGridCubesInRange(Vector3 loc, int range)
        {
            RefreshGridSearch();
            return GetGridCubesInRange(MapGridCubes[(int)loc.X, (int)loc.Y, (int)loc.Z], range);
        }

        /// <summary>
        /// This function resets the distances in each cube of the grid to a very high number.
        /// This is used to prepare the GetGridCubesInRange function for search.
        /// </summary>
        public void RefreshGridSearch()
        {
            foreach (GridCube gs in MapGridCubes)
            {
                gs.distance = 1000;
            }
        }

        /// <summary>
        /// Fills each grid square with a list of neighboring grid squares.
        /// </summary>
        public void ConnectGridSquares()
        {
            foreach (GridCube gs in MapGridCubes)
            {
                if (gs.X > 0)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X - 1, gs.Y, gs.Z]);
                if (gs.X < Size.X - 1)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X + 1, gs.Y, gs.Z]);
                if (gs.Y > 0)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X, gs.Y - 1, gs.Z]);
                if (gs.Y < Size.Y - 1)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X, gs.Y + 1, gs.Z]);
                if (gs.Z > 0)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X, gs.Y, gs.Z - 1]);
                if (gs.Z < Size.Z - 1)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X, gs.Y, gs.Z + 1]);
            }
        }

        /// <summary>
        /// 0 Version:
        /// These functions add our side grids for each plane: XY, XZ, YZ.
        /// Each fucntion stores the added lines to a matrix.
        /// AddGridIsometric() calls the AddGrid for each plane.
        /// AddGrid XYZ is never called.
        /// </summary>
        //red
        public void AddGridXY0()
        {
            AddGridXY0Lines();
            AddGridXY0Orthogonal();
        }
        public void AddGridXY0Lines() 
        {
            for (int i = 0; i <= Size.X; i++)
            {
                float x = ((Size.X / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float y = Size.Y / 2 * GridCube.GRIDSQUARELENGTH;
                float z = -1 * Size.Z / 2 * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                     new Color(1.0f, 0, 0));
                LineManager.AddLine(line);
                XYMatrix[0, i] = line;
            }
        }
        public void AddGridXY0Orthogonal()
        {
            for (int i = 0; i <= Size.Y; i++)
            {
                float y = ((Size.Y / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float x = Size.X / 2 * GridCube.GRIDSQUARELENGTH;
                float z = -1 * Size.Z / 2 * GridCube.GRIDSQUARELENGTH;

                Line orthongal = new Line(new Vector3(x, y, z), new Vector3(-x, y, z),
                                          new Color(1.0f, 0, 0));
                LineManager.AddLine(orthongal);
                XYMatrix[1, i] = orthongal;
            }
        }

        //green
        public void AddGrid0YZ()
        {
            AddGrid0YZLines();
            AddGrid0YZOrthogonal();
        }
        public void AddGrid0YZLines()
        {
            for (int i = 0; i <= Size.Z; i++)
            {
                float x = -1 * Size.X / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size.Y / 2 * GridCube.GRIDSQUARELENGTH;
                float z = ((Size.Z / 2) - i) * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                     new Color(0, 1.0f, 0));
                LineManager.AddLine(line);
                YZMatrix[0, i] = line;
            }
        }
        public void AddGrid0YZOrthogonal()
        {
            for (int i = 0; i <= Size.Y; i++)
            {
                float x = -1 * Size.X / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size.Z / 2 * GridCube.GRIDSQUARELENGTH;
                float z = ((Size.Y / 2) - i) * GridCube.GRIDSQUARELENGTH;

                Line orthogonal = new Line(new Vector3(x, z, y), new Vector3(x, z, -y),
                                           new Color(0, 1.0f, 0));
                LineManager.AddLine(orthogonal);
                YZMatrix[1, i] = orthogonal;
            }
        }

        //blue
        public void AddGridX0Z()
        {
            AddGridX0ZLines();
            AddGridX0ZOrthogonal();
        }
        public void AddGridX0ZLines()
        {
            for (int i = 0; i <= Size.X; i++)
            {
                float x = ((Size.X / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float z = Size.Z / 2 * GridCube.GRIDSQUARELENGTH;
                float y = -1 * (Size.Y / 2) * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, y, -z),
                                    new Color(0, 0, 1.0f));
                LineManager.AddLine(line);
                XZMatrix[0, i] = line;
            }
        }
        public void AddGridX0ZOrthogonal()
        {
            for (int i = 0; i <= Size.Z; i++)
            {
                float z = ((Size.Z / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float x = Size.X / 2 * GridCube.GRIDSQUARELENGTH;
                float y = -1 * (Size.Y / 2) * GridCube.GRIDSQUARELENGTH;

                Line orthogonal = new Line(new Vector3(x, y, z), new Vector3(-x, y, z),
                                    new Color(0, 0, 1.0f));
                LineManager.AddLine(orthogonal);
                XZMatrix[1, i] = orthogonal;
            }
        }

        public void AddGridIsometric()
        {
            AddGrid0YZ();
            AddGridX0Z();
            AddGridXY0();
        }

        /// <summary>
        /// 1 Version:
        /// These functions are analogous to the 0 versions.
        /// Adds Grids to opposite ends of planes.
        /// AddGridAltIsometric() calls all 1 version AddGrid functions.
        /// </summary>
        public void AddGridXY1()
        {
            AddGridXY1Lines();
            AddGridXY1Orthogonal();
        }
        public void AddGridXY1Lines()
        {
            for (int i = 0; i <= Size.X; i++)
            {
                float x = ((Size.X / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float y = Size.Y / 2 * GridCube.GRIDSQUARELENGTH;
                float z = Size.Z / 2 * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                     new Color(1.0f, 0, 0));
                LineManager.AddLine(line);
                XYMatrix[2, i] = line;
            }
        }
        public void AddGridXY1Orthogonal()
        {
            for (int i = 0; i <= Size.Y; i++)
            {
                float x = ((Size.Y / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float y = Size.X / 2 * GridCube.GRIDSQUARELENGTH;
                float z = Size.Z / 2 * GridCube.GRIDSQUARELENGTH;

                Line orthongal = new Line(new Vector3(y, x, z), new Vector3(-y, x, z),
                                          new Color(1.0f, 0, 0));
                LineManager.AddLine(orthongal);
                XYMatrix[3, i] = orthongal;
            }
        }

        public void AddGrid1YZ()
        {
            AddGrid1YZLines();
            AddGrid1YZOrthogonal();
        }
        public void AddGrid1YZLines()
        {
            for (int i = 0; i <= Size.Z; i++)
            {
                float x = Size.X / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size.Y / 2 * GridCube.GRIDSQUARELENGTH;
                float z = ((Size.Z / 2) - i) * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                     new Color(0, 1.0f, 0));
                LineManager.AddLine(line);
                YZMatrix [2, i] = line;
            }
        }
        public void AddGrid1YZOrthogonal()
        {
            for (int i = 0; i <= Size.Y; i++)
            {
                float x = Size.X / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size.Z / 2 * GridCube.GRIDSQUARELENGTH;
                float z = ((Size.Y / 2) - i) * GridCube.GRIDSQUARELENGTH;

                Line orthogonal = new Line(new Vector3(x, z, y), new Vector3(x, z, -y),
                                           new Color(0, 1.0f, 0));
                LineManager.AddLine(orthogonal);
                YZMatrix [3, i] = orthogonal;
            }
        }

        public void AddGridX1Z()
        {
            AddGridX1ZLines();
            AddGridX1ZOrthogonal();
        }
        public void AddGridX1ZLines()
        {
            for (int i = 0; i <= Size.X; i++)
            {
                float x = ((Size.X / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float z = Size.Z / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size.Y / 2 * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, y, -z),
                                    new Color(0, 0, 1.0f));
                LineManager.AddLine(line);
                XZMatrix[2, i] = line;
                
            }
        }
        public void AddGridX1ZOrthogonal()
        {
            for (int i = 0; i <= Size.Z; i++)
            {
                float z = ((Size.Z / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float x = Size.X / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size.Y / 2 * GridCube.GRIDSQUARELENGTH;

                Line orthogonal = new Line(new Vector3(x, y, z), new Vector3(-x, y, z),
                                    new Color(0, 0, 1.0f));
                LineManager.AddLine(orthogonal);
                XZMatrix[3, i] = orthogonal;
            }
        }

        public void AddGridAltIsometric()
        {
            AddGrid1YZ();
            AddGridX1Z();
            AddGridXY1();
        }

        /// <summary>
        /// helper functions to move grids
        /// </summary>
        public void RemoveXYMatrix()
        {
            foreach (Line line in XYMatrix) 
            {
                LineManager.RemoveLine(line);
            }
        }
        public void RemoveXZMatrix()
        {
            foreach (Line line in XZMatrix)
            {
                LineManager.RemoveLine(line);
            }
        }
        public void RemoveYZMatrix()
        {
            foreach (Line line in YZMatrix)
            {
                LineManager.RemoveLine(line);
            }
        }
        public void Remove0YZMatrix()
        {
            for (int i = 0; i < Math.Max((int)Size.Y + 1, (int)Size.Z + 1); i++)
            {
                LineManager.RemoveLine(YZMatrix[0, i]);
                LineManager.RemoveLine(YZMatrix[1, i]);
            }
        }
        public void Remove1YZMatrix()
        {
            for (int i = 0; i < Math.Max((int)Size.Y + 1, (int)Size.Z + 1); i++)
            {
                LineManager.RemoveLine(YZMatrix[2, i]);
                LineManager.RemoveLine(YZMatrix[3, i]);
            }
        }
        public void RemoveXY0Matrix()
        {
            for (int i = 0; i < Math.Max((int)Size.X + 1, (int)Size.Y + 1); i++)
            {
                LineManager.RemoveLine(XYMatrix[0, i]);
                LineManager.RemoveLine(XYMatrix[1, i]);
            }
        }
        public void RemoveXY1Matrix()
        {
            for (int i = 0; i < Math.Max((int)Size.X + 1, (int)Size.Y + 1); i++)
            {
                LineManager.RemoveLine(XYMatrix[2, i]);
                LineManager.RemoveLine(XYMatrix[3, i]);
            }
        }
        public void RemoveX0ZMatrix()
        {
            for (int i = 0; i < Math.Max((int)Size.X + 1, (int)Size.Z + 1); i++)
            {
                LineManager.RemoveLine(XZMatrix[0, i]);
                LineManager.RemoveLine(XZMatrix[1, i]);
            }
        }
        public void RemoveX1ZMatrix()
        {
            for (int i = 0; i < Math.Max((int)Size.X + 1, (int)Size.Z + 1); i++)
            {
                LineManager.RemoveLine(XZMatrix[2, i]);
                LineManager.RemoveLine(XZMatrix[3, i]);
            }
        }

        public GridCube GetCubeAt(Vector3 loc)
        {
            return MapGridCubes[(int)loc.X, (int)loc.Y, (int)loc.Z]; 
        }

        public void DrawLineTest()
        {
            float y = Size.X / 2 * GridCube.GRIDSQUARELENGTH;
            LineManager.AddLine(new Line(new Vector3(10, 10, 10),
                                new Vector3(0, 0, 0)));
        }

        public void MoveObject(GameObject obj, int x, int y, int z)
        {
            obj.GridLocation.RemoveObject(obj);
            obj.GridLocation = MapGridCubes[x, y, z];
            obj.GridLocation.AddObject(obj);
            addGameObject(obj, new Vector3(x, y, z));
            //addGameObject(obj);
        }
    }
}
