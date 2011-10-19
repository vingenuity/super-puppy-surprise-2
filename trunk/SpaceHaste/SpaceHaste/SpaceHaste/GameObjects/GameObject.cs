using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.Maps;
using SpaceHaste.GameMech;
using SpaceHaste.Graphics;
//------------------------------------------------------
//Game object holds the model information for all assets in game ie: Ships, Environmental Obstacles, Visual Assets
//Most game objects take up space in the grid with the exception of Visual assets ie: The Sun, extra stuff
//------------------------------------------------------
namespace SpaceHaste.GameObjects
{
    public class GameObject
    {
        //Our info for engine
        public Vector3 GridPosition;    //Our notion of position within game grid
        public Vector3 DrawPosition;
        public Vector3 Velocity;        //Might be abstracted into ship, Speed of ship movement to new square
        public Vector3 Direction;       //Unit Vector of the direction the ship is facing
        public BoundingSphere boundingSphere;
        public Boolean Passable;
        public int Team;
        public double NeededEnergy;
        public GridCube location;

        public int MovementRange;

        //Model/Render Info
        public float Scale;
        public Model Model;             //Holds model info and effect information
        public Matrix World;            //Render position of model

        //Constructor
        public GameObject(Vector3 location)
        {
            NeededEnergy = 4;
            Load();
            GridPosition = location;
            MovementRange = 4;
        }
        public virtual void Load()
        {
            GraphicsManager.GraphicsGameObjects.Add(this);
            GameMechanicsManager.MoveableSceneGameObjectList.Add(this);
        }
        public virtual void Unload()
        {
        }
    }
}
