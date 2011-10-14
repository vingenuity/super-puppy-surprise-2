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
        protected GridSquare[,,] MapGridSquares;
        protected List<GameObject> MapObjects;
        protected int Size;
        public Map(int Size)
        {
            this.Size = Size;
            MapObjects = new List<GameObject>();

            InitMapGridSquares();
            InitMapGameObjects();
   
        }

        public void addGameObject(GameObject go, Vector3 position) {
            MapGridSquares[(int)position.X, (int)position.Y, (int)position.Z].AddObject(go);
            go.GridPosition = position;
            go.DrawPosition = MapGridSquares[(int)position.X, (int)position.Y, (int)position.Z].Center;
            MapObjects.Add(go);
        }

        public int getGridSize() { return Size; }

        protected virtual void InitMapGameObjects()
        {
        }

        void InitMapGridSquares()
        {
            float bounds = -GridSquare.GRIDSQUARELENGTH * Size / 2 + GridSquare.GRIDSQUARELENGTH/2;

            MapGridSquares = new GridSquare[Size, Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    for (int k = 0; k < Size; k++)
                    {
                        MapGridSquares[i, j, k] = new GridSquare(i, j, k);
                        MapGridSquares[i, j, k].Center = new Vector3(bounds + GridSquare.GRIDSQUARELENGTH * i, 
                            bounds + GridSquare.GRIDSQUARELENGTH * j,
                            bounds + GridSquare.GRIDSQUARELENGTH * k);
                    }
            ConnectGridSquares();
        }

        /// <summary>
        /// This is primarily for the lasers
        /// axis-bound distance check eliminates game objects out of range
        /// a ray test checks if objects are blocked
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<GameObject> GetGameObjectsInRange(GridSquare gs, int range) 
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
        public List<GridSquare> GetGridSquaresInRange(GridSquare gs, int range)
        {
            List<GridSquare> inRange = new List<GridSquare>();
            if (range == 0) return inRange;
            foreach (GridSquare neighbor in gs.ConnectedGridSquares)
            {
                //Can't cross through asteroid
                if(neighbor.getTerrain() == GridSquare.TerrainType.asteroid)
                    continue;
                //Movement penalty for nebulae
                if(neighbor.getTerrain() == GridSquare.TerrainType.nebula)
                    inRange.AddRange(GetGridSquaresInRange(neighbor, range - 2));
                //Otherwise, take normal movement
                else
                    inRange.AddRange(GetGridSquaresInRange(neighbor, range - 1));
            }
            inRange = inRange.Distinct().ToList();
            return inRange;
        }
       
        /// <summary>
        /// Finds the number of grid squares in range of a grid square x, y, z.
        /// This extends the functinoality of the above function for more general use.
        /// </summary>
        /// <param name="x"> X coordinate to start the search.</param>
        /// <param name="y"> Y coordinate to start the search.</param>
        /// <param name="z"> Z coordinate to start the search.</param>
        /// <param name="range"> number of neighboring squares to search.</param>
        /// <returns>List of valid grid squares in range.</returns>
        public List<GridSquare> GetGridSquaresInRange(int x, int y, int z, int range)
        {
            return GetGridSquaresInRange(MapGridSquares[x,y,z], range);
        }

        /// <summary>
        /// Fills each grid square with a list of neighboring grid squares.
        /// </summary>
        public void ConnectGridSquares()
        {
            foreach (GridSquare gs in MapGridSquares)
            {
                if (gs.X > 0)
                    gs.ConnectedGridSquares.Add(MapGridSquares[gs.X - 1, gs.Y, gs.Z]);
                if (gs.X < Size - 1)
                    gs.ConnectedGridSquares.Add(MapGridSquares[gs.X + 1, gs.Y, gs.Z]);
                if (gs.Y > 0)
                    gs.ConnectedGridSquares.Add(MapGridSquares[gs.X, gs.Y - 1, gs.Z]);
                if (gs.Y < Size - 1)
                    gs.ConnectedGridSquares.Add(MapGridSquares[gs.X, gs.Y + 1, gs.Z]);
                if (gs.Z > 0)
                    gs.ConnectedGridSquares.Add(MapGridSquares[gs.X, gs.Y, gs.Z - 1]);
                if (gs.Z < Size - 1)
                    gs.ConnectedGridSquares.Add(MapGridSquares[gs.X, gs.Y, gs.Z + 1]);
            }
        }
        //here!
        public void AddGridXY0()
        {
            for (int i = 0; i <= Size; i++)
            {
               float x = ((Size / 2) - i) * GridSquare.GRIDSQUARELENGTH;
                float y = Size / 2 * GridSquare.GRIDSQUARELENGTH;
                LineManager.AddLine(new Line(new Vector3(x, y , 0),
                                    new Vector3(x,-y,0)));
                LineManager.AddLine(new Line(new Vector3(y, x, 0),
                                   new Vector3(-y, x, 0)));
            }
                

        }
        public void AddGridXYZ()
        {
            for (int j = 0; j <= Size; j++)
            {
                float z = ((Size / 2) - j) * GridSquare.GRIDSQUARELENGTH;
                for (int i = 0; i <= Size; i++)
                {
                    float x = ((Size / 2) - i) * GridSquare.GRIDSQUARELENGTH;
                    float y = Size / 2 * GridSquare.GRIDSQUARELENGTH;
                    LineManager.AddLine(new Line(new Vector3(x, y, z),
                                        new Vector3(x, -y, z)));
                    LineManager.AddLine(new Line(new Vector3(y, x, z),
                                       new Vector3(-y, x, z)));
                }

            }

        }
        public void DrawLineTest()
        {
            float y = Size / 2 * GridSquare.GRIDSQUARELENGTH;
            LineManager.AddLine(new Line(new Vector3(10, 10, 10),
                                new Vector3(0, 0, 0)));
        }
        public void AddGameObjectToGridSquare(GameObject gameObject, int x, int y, int z)
        {
            //MapGameObjects[x, y, z] = gameObject;
            gameObject.GridPosition = MapGridSquares[x, y, z].Center;
        }
    }
}

