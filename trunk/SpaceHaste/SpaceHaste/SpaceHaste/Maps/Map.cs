using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using SpaceHaste.Primitives;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class Map
    {
        protected GridCube[, ,] MapGridCubes;
        protected GridCube[, ,] BottomMap;

        protected Line[,] XYMatrix;
        protected Line[,] XZMatrix;
        protected Line[,] YZMatrix;

        public List<GridCube> EnvMapObjects;
        public List<GameObject> ShipMapObjects;
        public int Size;

        public static Map map;

        public Map(int Size)
        {
            this.Size = Size;
            ShipMapObjects = new List<GameObject>();
            EnvMapObjects = new List<GridCube>();

            XYMatrix = new Line[2, Size + 1];
            XZMatrix = new Line[2, Size + 1];
            YZMatrix = new Line[2, Size + 1];

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

        public void colorGrids()
        {
            foreach (GameObject go in ShipMapObjects)
            {
            }
        }

        public void AddEnvObject(GridCube.TerrainType e , int x, int y, int z)
        {
            switch (e)
            {
                case GridCube.TerrainType.asteroid:
                    MapGridCubes[x, y, z].SetTerrain(GridCube.TerrainType.asteroid);
                    break;
                case GridCube.TerrainType.nebula:
                    MapGridCubes[x, y, z].SetTerrain(GridCube.TerrainType.nebula);
                    break;
                case GridCube.TerrainType.wreck:
                    MapGridCubes[x, y, z].SetTerrain(GridCube.TerrainType.wreck);
                    break;
            };
            EnvMapObjects.Add(MapGridCubes[x, y, z]);
        }

        protected virtual void InitMapGameObjects() { }

        void InitMapGridCubes()
        {
            float bounds = -GridCube.GRIDSQUARELENGTH * Size / 2 + GridCube.GRIDSQUARELENGTH/2;

            MapGridCubes = new GridCube[Size, Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    for (int k = 0; k < Size; k++)
                    {
                        MapGridCubes[i, j, k] = new GridCube(i, j, k);
                        MapGridCubes[i, j, k].Center = new Vector3(bounds + GridCube.GRIDSQUARELENGTH * i,
                            bounds + GridCube.GRIDSQUARELENGTH * j,
                            bounds + GridCube.GRIDSQUARELENGTH * k);
                    }

            BottomMap = new GridCube[Size, 1, Size];
            for (int i = 0; i < Size; i++)
                for (int k = 0; k < Size; k++)
                {
                    int j = 0;
                    BottomMap[i, j, k] = new GridCube(i, j, k);
                    BottomMap[i, j, k].Center = new Vector3(bounds + GridCube.GRIDSQUARELENGTH * i,
                        bounds + GridCube.GRIDSQUARELENGTH * j,
                        bounds + GridCube.GRIDSQUARELENGTH * k);
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
            Ray ray = new Ray(go.GridPosition, rayDirection);
            float? distance = ray.Intersects(target.boundingSphere);
            float? r = 0;

            for (int i = 0; i < EnvMapObjects.Count; i++)
            {
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

        /// <summary>
        /// Finds the number of grid squares in range of a particular grid square.
        /// This will presumably be used to find valid moves for each ship.
        /// This function does a depth first search
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
                if (gs.X < Size - 1)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X + 1, gs.Y, gs.Z]);
                if (gs.Y > 0)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X, gs.Y - 1, gs.Z]);
                if (gs.Y < Size - 1)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X, gs.Y + 1, gs.Z]);
                if (gs.Z > 0)
                    gs.ConnectedGridSquares.Add(MapGridCubes[gs.X, gs.Y, gs.Z - 1]);
                if (gs.Z < Size - 1)
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
        public void AddGridXY0()
        {
            for (int i = 0; i <= Size; i++)
            {
                float x = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float z = -1 * Size / 2 * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                     new Color((float)(Size - i) * .1f, 0, 0));
                LineManager.AddLine(line);
                XYMatrix[0, i] = line;

                Line orthongal = new Line(new Vector3(y, x, z), new Vector3(-y, x, z),
                                          new Color((float)(Size - i) * .1f, 0, 0));
                LineManager.AddLine(orthongal);
                XYMatrix[1, i] = orthongal;
            }
                

        }
        public void AddGrid0YZ()
        {
            for (int i = 0; i <= Size; i++)
            {
                float x = -1 * Size / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float z = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                     new Color(0, (float)(Size - i) * .1f, 0));
                LineManager.AddLine(line);
                YZMatrix[0, i] = line;

                Line orthogonal = new Line(new Vector3(x, z, y), new Vector3(x, z, -y),
                                           new Color(0, (float)(Size - i) * .1f, 0));
                LineManager.AddLine(orthogonal);
                YZMatrix[1, i] = orthogonal;
            }


        }
        public void AddGridX0Z()
        {
            for (int i = 0; i <= Size; i++) 
            {
                float x = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float z = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float y = -1 * (Size / 2) * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, y, -z), 
                                    new Color(0, 0, (float) (Size - i) * .1f ));
                LineManager.AddLine(line);
                XZMatrix[0, i] = line;

                Line orthogonal = new Line(new Vector3(z, y, x), new Vector3(-z, y, x),
                                    new Color(0, 0, (float) (Size - i) * .1f));
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
            for (int i = 0; i <= Size; i++)
            {
                float x = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float z = Size / 2 * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                     new Color((float) 1 - ((Size - i) * .1f), 0, 0));
                LineManager.AddLine(line);
                XYMatrix[0, i] = line;

                Line orthongal = new Line(new Vector3(y, x, z), new Vector3(-y, x, z),
                                          new Color((float)1 - ((Size - i) * .1f), 0, 0));
                LineManager.AddLine(orthongal);
                XYMatrix[1, i] = orthongal;
            }


        }
        public void AddGrid1YZ()
        {
            for (int i = 0; i <= Size; i++)
            {
                float x = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float z = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                     new Color(0, (float)1 - ((Size - i) * .1f), 0));
                LineManager.AddLine(line);
                YZMatrix[0, i] = line;

                Line orthogonal = new Line(new Vector3(x, z, y), new Vector3(x, z, -y),
                                           new Color(0, (float)1 - ((Size - i) * .1f), 0));
                LineManager.AddLine(orthogonal);
                YZMatrix[1, i] = orthogonal;
            }


        }
        public void AddGridX1Z()
        {
            for (int i = 0; i <= Size; i++)
            {
                float x = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float z = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float y = (Size / 2) * GridCube.GRIDSQUARELENGTH;

                Line line = new Line(new Vector3(x, y, z), new Vector3(x, y, -z),
                                    new Color(0, 0, (float) 1 - ((Size - i) * .1f)));
                LineManager.AddLine(line);
                XZMatrix[0, i] = line;

                Line orthogonal = new Line(new Vector3(z, y, x), new Vector3(-z, y, x),
                                    new Color(0, 0, (float) 1 - ((Size - i) * .1f)));
                LineManager.AddLine(orthogonal);
                XZMatrix[1, i] = orthogonal;
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
            for(int i = 0; i <= Size; i++)
            {
                LineManager.RemoveLine(XYMatrix[0, i]);
                LineManager.RemoveLine(XYMatrix[1, i]);
            }
        }
        public void RemoveXZMatrix()
        {
            for (int i = 0; i <= Size; i++)
            {
                LineManager.RemoveLine(XZMatrix[0, i]);
                LineManager.RemoveLine(XZMatrix[1, i]);
            }
        }
        public void RemoveYZMatrix()
        {
            for (int i = 0; i <= Size; i++)
            {
                LineManager.RemoveLine(YZMatrix[0, i]);
                LineManager.RemoveLine(YZMatrix[1, i]);
            }
        }

        public GridCube GetCubeAt(Vector3 loc)
        {
            return MapGridCubes[(int)loc.X, (int)loc.Y, (int)loc.Z]; 
        }

        public void DrawLineTest()
        {
            float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
            LineManager.AddLine(new Line(new Vector3(10, 10, 10),
                                new Vector3(0, 0, 0)));
        }

        public void AddGameObjectToGridSquare(GameObject gameObject, int x, int y, int z)
        {
            gameObject.GridLocation = MapGridCubes[x, y, z];
            gameObject.GridPosition = MapGridCubes[x, y, z].Center;
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
