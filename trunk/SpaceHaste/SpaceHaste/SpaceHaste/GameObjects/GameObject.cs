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
using SpaceHaste.DPSFParticles;
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

        //energy[0]
        public double[] energy = new double[2];        //energy  -- 0 = current, 1 = max
        public double[] efficiency = new double[3];    //contains [moveEff,laserEff,shieldsEff] efficiencies track energy usage for specified action                        
        public double regen;                           //The amount of energy each ship regenerates in one regeneration round.  Should be somewhere around 5 - 30.

        //Weapons
        public int[] dmg = new int[2];           //Damage done by weapons 0 = Laser, 1 = Missile
        public int MissileCount;                            //Number of missiles ship has left        
        public int MissileRange;
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
        //energy[0] Information
        public double MovementEnergyCost;
        public double AttackEnergyCost;
        //Ship Information(For HUD)
        public string Name;
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
            energy[0] = 100;
            Load();
            GridPosition = location;
            team = side;

            boundingSphere = new BoundingSphere(GridPosition, 0.5f);

            //Set efficiencies
            efficiency = eff;
            MovementEnergyCost = Math.Round(20 * efficiency[0]);
            AttackEnergyCost = Math.Round(30 * efficiency[1]);

            MovementRange = (int) (energy[0] / MovementEnergyCost);
	        LaserRange = 6;
            MissileRange = 4;

            //Fill hull and shields to max.
            hull[0] = hull[1] = maxHull;
            shield[1] = maxShield;
            shield[0] = 0;
            //Start energy at zero. We will recharge energy starting on turn 1.
            energy[0] = 0;
            energy[1] = 100;

            //Set regen
            regen = regeneration;

            //Set up our weapons
            MissileCount = numMissiles;
            dmg[0] = lsrDmg;
            dmg[1] = missDmg;

            LaserDamage = 50;
        }

        public virtual void Load()
        {
            //Set up our rendering options.
            Model = GraphicsManager.Content.Load<Model>("light_ship_combined");

            Scale = 2f;

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
                DeathParticle.CreateDeathParticle(DrawPosition);
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

        public void AddEnergy(double En)
        {
            energy[0] += En;
            waitTime -= En;
            if (waitTime < 0)
                waitTime = 0;
            if (energy[0] < 0)
                energy[0] = 0;
            if (energy[0] > 100)
                energy[0] = 100;
        }
        public int LaserDamage
        {
            get;
            set;
        }

        public int GetLaserDamage(GameObject target) 
        {
            int distance = (int) Math.Abs(GridPosition.X - target.GridPosition.X) +
                           (int) Math.Abs(GridPosition.Y - target.GridPosition.Y) +
                           (int) Math.Abs(GridPosition.Z - target.GridPosition.Z);

            if (distance <= 2)
                return (int) (LaserDamage * 1);
            else if (distance <= 5)
                return (int) (LaserDamage * 0.6f);
            else return (int) (LaserDamage * 0.4f);
        }

        public void updateBoundingSphere() 
        {
            boundingSphere.Center = GridPosition;
        }

        //Team Operations
        public Team getTeam() { return team; }
        public void setTeam(Team side) { team = side; }

        //Operators(for sorting)
        public static bool operator >(GameObject go1, GameObject go2)
        {
            if (go1.energy[0] - go1.waitTime > go2.energy[0] - go2.waitTime)
                return true;
            else return false;
        }
        public static bool operator <(GameObject go1, GameObject go2)
        {
            if (go1.energy[0] - go1.waitTime < go2.energy[0] - go2.waitTime)
                return true;
            else return false;
        }
        //REDUNDANT FUNCTIONS
    }
}
