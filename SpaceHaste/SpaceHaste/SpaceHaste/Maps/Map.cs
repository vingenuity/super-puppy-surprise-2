using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;

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
           
        }
        void InitMapGridSquares()
        {
            MapGridSquares = new GridSquare[Size, Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    for (int k = 0; k < Size; k++)
                        MapGridSquares[i, j, k] = new GridSquare(i, j, k);
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
    }
}
