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
//------------------------------------------------------
//Game object holds the model information for all assets in game ie: Ships, Environmental Obstacles, Visual Assets
//Most game objects take up space in the grid with the exception of Visual assets ie: The Sun, extra stuff
//------------------------------------------------------
namespace SpaceHaste.GameObjects
{
    public class GameObject
    {
        //Health
        public int[] hull = new int[2];          //health  -- 0 = current, 1 = max
        public int[] shield = new int[2];        //shields -- 0 = current, 1 = max

        //Energy
        public double[] energy = new double[2];        //energy  -- 0 = current, 1 = max
        public double[] efficiency = new double[3];    //contains [moveEff,laserEff,shieldsEff] efficiencies track energy usage for specified action                        
        public double regen;                           //The amount of energy each ship regenerates in one regeneration round.  Should be somewhere around 5 - 30.

        //Weapons
        public int[] dmg = new int[2];           //Damage done by weapons 0 = Laser, 1 = Missile
        public int numMiss;                            //Number of missiles ship has left         
        //Model/Render Information
        public float Scale;
        public Model Model;             //Holds model info and effect information
        public Matrix World;            //Render position of model
        public Vector3 DrawPosition;
        //Position Information
        public BoundingSphere boundingSphere;
        public Vector3 Direction;       //Unit Vector of the direction the ship is facing
        public GridCube GridLocation;   //Cube of map where object is.
        public Vector3 GridPosition;    //Our notion of position within game grid
        //Energy Information
        public double Energy;
        public double MovementEnergyCost = 20;
        public double AttackEnergyCost = 30;
        public double EnergyEfficiency = 4;
        //Ship Information(For HUD)
        public string Name;
        public double Health;
        public double maxHealth;
        public enum Team { Player = 0, Enemy = 1 }
        public Team team;
        //Player Selection Information
        public int LaserRange;
        public int MovementRange;
        public double waitTime;

        //Constructor
        public GameObject(String name, Vector3 location, Team side, int maxHull, int maxShield, double regeneration, int numMissiles, int lsrDmg, int missDmg, double[] eff)
        {
            Name = name;
            Energy = 100;
            Load();
            GridPosition = location;
            team = side;


            MovementRange = (int) (Energy / MovementEnergyCost);
	        LaserRange = 6;

            //Fill hull and shields to max.
            hull[0] = hull[1] = maxHull;
            shield[0] = shield[1] = maxShield;

            //Start energy at zero. We will recharge energy starting on turn 1.
            energy[0] = 0;
            energy[1] = 100;

            //Set regen
            regen = regeneration;

            //Set up our weapons
            numMiss = numMissiles;
            dmg[0] = lsrDmg;
            dmg[1] = missDmg;

            LaserDamage = 50;
        }

        //Creation and Deletion
        public static GameObject createBasicShip(String name, Vector3 location, Team team) {
            return new GameObject(name, location, team, 100, 100, 20, 3, 50, 100, new double[] { .25, .5, .5 });
        }

        public virtual void Load()
        {
            //Set up our rendering options.
            Model = GraphicsManager.Content.Load<Model>("Ship");

            Scale = .25f;

            GraphicsManager.AddGameObject(this);
            GameMechanicsManager.GameObjectList.Add(this);
        }
        
        public virtual void Unload()
        {
            GridLocation.RemoveObject(this);
            Map.map.removeGameObject(this);
            GraphicsManager.GraphicsGameObjects.Remove(this);
            GameMechanicsManager.GameObjectList.Remove(this);
        }

        //Object Actions

        public double getShipRegen()
        {
            return regen;
        }

        public void fireMissile(GameObject ship)
        {
            ship.isHit(dmg[1]);
        }

        public void isHit(int damage)
        {
            //If our shield can absorb it all, deal damage and return.
            if (shield[0] > damage)
            {
                shield[0] -= damage;
                return;
            }
            //Otherwise, take all of the damage and proceed to hull.
            else
            {
                damage -= shield[0];
                shield[0] = 0;
            }

            //If our hull can take it, deal remaining damage and exit
            if (hull[0] > damage)
            {
                hull[0] -= damage;
                return;
            }
            //Otherwise, we're dead. Get rid of us.
            else
            {
                SoundManager.Sounds.PlaySound(SoundEffects.explode);
                Unload();
            }
        }

        public double getHull()
        {
            return hull[0];
        }

        public double getMaxHull()
        {
            return hull[1];
        }

        public void Generate(int amount_energy) { energy[0] += regen; }

        public void AddEnergy(double energy)
        {
            Energy += energy;
            waitTime -= energy;
            if (waitTime < 0)
                waitTime = 0;
            if (Energy < 0)
                Energy = 0;
            if (Energy > 100)
                Energy = 100;
        }
        public int LaserDamage
        {
            get;
            set;
        }

        //Team Operations
        public Team getTeam() { return team; }
        public void setTeam(Team side) { team = side; }

        //Operators(for sorting)
        public static bool operator >(GameObject go1, GameObject go2)
        {
            if (go1.Energy - go1.waitTime > go2.Energy - go2.waitTime)
                return true;
            else return false;
        }
        public static bool operator <(GameObject go1, GameObject go2)
        {
            if (go1.Energy - go1.waitTime < go2.Energy - go2.waitTime)
                return true;
            else return false;
        }


        //REDUNDANT FUNCTIONS



    }
}
