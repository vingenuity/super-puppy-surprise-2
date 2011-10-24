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
        public int LaserRange;
        public double Energy;
        public double MovementEnergyCost = 10;
        public double EnergyEfficiency = 4;
        public GridCube GridLocation;
        public string Name;
        public double Health;
        public double maxHealth;
        public int MovementRange;

        //Model/Render Info
        public float Scale;
        public Model Model;             //Holds model info and effect information
        public Matrix World;            //Render position of model

        //Constructor
        public GameObject(Vector3 position)
        {
            Energy = 100;
            Load();
            GridPosition = position;
            MovementRange = 4;
            LaserRange = 6;
        }
        public virtual void Load()
        {
            GraphicsManager.GraphicsGameObjects.Add(this);
            GameMechanicsManager.GameObjectList.Add(this);
        }


        public virtual void Unload()
        {
            GridLocation.RemoveObject(this);
            Map.map.removeGameObject(this);
            GraphicsManager.GraphicsGameObjects.Remove(this);
            GameMechanicsManager.GameObjectList.Remove(this);
        }

        public int LaserDamage { 
            get; 
            set; 
        }
    }
}
