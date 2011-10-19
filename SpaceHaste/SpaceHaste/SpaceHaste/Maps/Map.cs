using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using SpaceHaste.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceHaste.Maps
{
    public class Map
    {
        protected GridCube[, ,] MapGridCubes;
        protected GridCube[, ,] BottomMap;
        public List<GameObject> MapObjects;
        public int Size;
        private KeyboardState kState;

        public static Map map;

        public Map(int Size)
        {
            this.Size = Size;
            MapObjects = new List<GameObject>();

            InitMapGridCubes();
            InitMapGameObjects();
            map = this;
        }

        public void addGameObject(GameObject go, Vector3 position) {
            MapGridCubes[(int)position.X, (int)position.Y, (int)position.Z].AddObject(go);
            go.GridPosition = position;
            go.DrawPosition = MapGridCubes[(int)position.X, (int)position.Y, (int)position.Z].Center;
            go.location = MapGridCubes[(int)position.X, (int)position.Y, (int)position.Z];
            MapObjects.Add(go);
        }

        public void colorGrids()
        {
            foreach (GameObject go in MapObjects)
            {
            }
        }

        protected virtual void InitMapGameObjects()
        {
        }

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

        public List<GameObject> GetGameObjectsInRange(Vector3 loc, int range)
        {
            return GetGameObjectsInRange(MapGridCubes[(int)loc.X, (int)loc.Y, (int)loc.Z], range);
        }

        /// <summary>
        /// This is primarily for the lasers
        /// axis-bound distance check eliminates game objects out of range
        /// a ray test checks if objects are blocked
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<GameObject> GetGameObjectsInRange(GridCube gs, int range) 
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < MapObjects.Count; i++) {
                int d = (int)Math.Abs(MapObjects[i].GridPosition.X - gs.Position.X) +
                        (int)Math.Abs(MapObjects[i].GridPosition.Y - gs.Position.Y) +
                        (int)Math.Abs(MapObjects[i].GridPosition.Z - gs.Position.Z);
                if (d > range)
                    continue;
                Vector3 rayDirection = MapObjects[i].GridPosition - gs.Position;
           
                Ray ray = new Ray(gs.Position, rayDirection);
                float? distance = ray.Intersects(MapObjects[i].boundingSphere);
                float? r = 0;
                for (int j = 0; j < MapObjects.Count; j++) {
                    if (MapObjects[j].Passable) continue; //ignore nebulae
                    r = ray.Intersects(MapObjects[j].boundingSphere);
                    if (r == null || r < 1 || r > distance) continue;
                    else break; // 1 < r < d
                }
                if (!(r > 1 && r < distance) || r == null)
                    list.Add(MapObjects[i]);
            }
            return new List<GameObject>();
        }
        
        /// <summary>
        /// Finds the number of grid squares in range of a particular grid square.
        /// This will presumably be used to find valid moves for each ship.
        /// </summary>
        /// <param name="gs">Grid square to start the search.</param>
        /// <param name="range">Distance of squares away to search.</param>
        /// <returns>List of valid grid squares in range.</returns>
        public List<GridCube> GetGridSquaresInRange(GridCube gs, int range)
        {
            List<GridCube> inRange = new List<GridCube>();
            if (range <= 0) return inRange;
            foreach (GridCube neighbor in gs.ConnectedGridSquares)
            {
                //Can't be in same square as another ship
                if (neighbor.HasObject())
                    continue;
                //Can't cross through asteroid
                if(neighbor.getTerrain() == GridCube.TerrainType.asteroid)
                    continue;
                //Movement penalty for nebulae
                if (neighbor.getTerrain() == GridCube.TerrainType.nebula)
                    inRange.AddRange(GetGridSquaresInRange(neighbor, range-2));
                //Otherwise, take normal movement
                else
                    inRange.AddRange(GetGridSquaresInRange(neighbor, range-1));
                inRange.Add(neighbor);
            }
            inRange = inRange.Distinct().ToList();
            return inRange;
        }
       
        /// <summary>
        /// Finds the number of grid squares in range of a grid square x, y, z.
        /// This extends the functionality of the above function for more general use.
        /// </summary>
        /// <param name="loc"> location to start the search.</param>
        /// <param name="range"> number of neighboring squares to search.</param>
        /// <returns>List of valid grid squares in range.</returns>
        public List<GridCube> GetGridSquaresInRange(Vector3 loc, int range)
        {
            return GetGridSquaresInRange(MapGridCubes[(int)loc.X, (int)loc.Y, (int)loc.Z], range);
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

        public void AddGridXY0()
        {
            for (int i = 0; i <= Size; i++)
            {
                float x = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float z = -1 * Size / 2 * GridCube.GRIDSQUARELENGTH;
                LineManager.AddLine(new Line(new Vector3(x, y , z), new Vector3(x, -y, z), 
                                    new Color((float) (Size - i) * .1f, 0, 0)));
                LineManager.AddLine(new Line(new Vector3(y, x, z), new Vector3(-y, x, z),
                                    new Color((float) (Size - i) * .1f, 0, 0)));
            }
                

        }
        public void AddGrid0YZ()
        {
            for (int i = 0; i <= Size; i++)
            {
                float x = -1 * Size / 2 * GridCube.GRIDSQUARELENGTH;
                float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float z = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;
                LineManager.AddLine(new Line(new Vector3(x, y, z), new Vector3(x, -y, z),
                                    new Color(0, (float)(Size - i) * .1f, 0)));
                LineManager.AddLine(new Line(new Vector3(x, z, y), new Vector3(x, z, -y),
                                    new Color(0, (float)(Size - i) * .1f, 0)));
            }


        }
        public void AddGridX0Z() 
        {
            for (int i = 0; i <= Size; i++) 
            {
                float x = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;
                float z = Size / 2 * GridCube.GRIDSQUARELENGTH;
                float y = -1 * (Size / 2) * GridCube.GRIDSQUARELENGTH;
                LineManager.AddLine(new Line(new Vector3(x, y, z), new Vector3(x, y, -z), 
                                    new Color(0, 0, (float) (Size - i) * .1f )));
                LineManager.AddLine(new Line(new Vector3(z, y, x), new Vector3(-z, y, x),
                                    new Color(0, 0, (float) (Size - i) * .1f)));
            }
        }
        public void AddGridXYZ()
        {
            for (int j = 0; j <= Size; j++)
            {
                float z = ((Size / 2) - j) * GridCube.GRIDSQUARELENGTH;
                for (int i = 0; i <= Size; i++)
                {
                    float x = ((Size / 2) - i) * GridCube.GRIDSQUARELENGTH;
                    float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
                    LineManager.AddLine(new Line(new Vector3(x, y, z),
                                        new Vector3(x, -y, z)));
                    LineManager.AddLine(new Line(new Vector3(y, x, z),
                                       new Vector3(-y, x, z)));
                }

            }

        }

        public GridCube GetCubeAt(Vector3 loc) { return MapGridCubes[(int)loc.X, (int)loc.Y, (int)loc.Z]; }

        public void DrawLineTest()
        {
            float y = Size / 2 * GridCube.GRIDSQUARELENGTH;
            LineManager.AddLine(new Line(new Vector3(10, 10, 10),
                                new Vector3(0, 0, 0)));
        }
        public void AddGameObjectToGridSquare(GameObject gameObject, int x, int y, int z)
        {
            gameObject.location = MapGridCubes[x, y, z];
            gameObject.GridPosition = MapGridCubes[x, y, z].Center;
        }

        public void MoveObject(GameObject obj, int x, int y, int z)
        {
            obj.location.RemoveObject(obj);
            obj.location = MapGridCubes[x, y, z];
            obj.location.AddObject(obj);
            addGameObject(obj, new Vector3(x, y, z));
        }
    }
}

