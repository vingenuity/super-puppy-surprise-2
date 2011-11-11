using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.Maps;
using SpaceHaste.GameMech;
using SpaceHaste.Graphics;
using SpaceHaste.Sounds;

namespace SpaceHaste.GameObjects
{
    public class SuperTerrain
    {
        public Vector3 Position;

        //Model/Render Information
        public float Scale;
        public Model Model;             //Holds model info and effect information
        public Matrix World;            //Render position of model
        public Vector3 DrawPosition;
       
        //Position Information
        public BoundingSphere boundingSphere;
        public GridCube GridLocation;   //Cube of map where object is.
        public Vector3 GridPosition;    //Our notion of position within game grid

        public SuperTerrain(Vector3 location) 
        {
            Load();

            GridPosition = Position = location;
        }

        public virtual void Load()
        {
            //Set up our rendering options.
            Model = GraphicsManager.Content.Load<Model>("Ship");

            Scale = .25f;

            //GraphicsManager.AddGameObject(this);
            GameMechanicsManager.SuperTerrainList.Add(this);
        }
    }
}
