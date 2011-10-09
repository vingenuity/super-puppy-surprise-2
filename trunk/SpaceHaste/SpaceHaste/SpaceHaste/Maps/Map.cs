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
        protected GameObject[, ,] MapGameObjects;
        protected GridSquare[, ,] MapGridSquares;
        protected int Size;
        public Map(int Size)
        {
            this.Size = Size;
            InitMapGridSquares();
            InitMapGameObjects();
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
        }
        protected virtual void InitMapGameObjects()
        {
        }
        public List<GameObject> GetGameObjectsInRange(int range)
        {
            List<GameObject> list = new List<GameObject>();
           
            return new List<GameObject>();
        }
       
        public List<GridSquare> GetGridSquaresInRange(int range)
        {
            return new List<GridSquare>();
        }
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
        public void DrawLineTest()
        {
            float y = Size / 2 * GridSquare.GRIDSQUARELENGTH;
            LineManager.AddLine(new Line(new Vector3(10, 10, 10),
                                new Vector3(0, 0, 0)));
        }
        public void AddGameObjectToGridSquare(GameObject gameObject, int x, int y, int z)
        {
            MapGameObjects[x, y, z] = gameObject;
            gameObject.Position = MapGridSquares[x, y, z].Center;
        }
    }
}

