using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.Graphics;
using SpaceHaste.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHaste.GameObjects
{
    public class Ship : GameObject
    {

        //Health
        int[] hull = new int[2];          //health  -- 0 = current, 1 = max
        int[] shield = new int[2];        //shields -- 0 = current, 1 = max

        //Energy
        int[] energy = new int[2];        //energy  -- 0 = current, 1 = max
        double[] efficiency = new double[3];    //contains [moveEff,laserEff,shieldsEff] efficiencies track energy usage for specified action                        

        //Weapons
        int[] dmg = new int[2];           //Damage done by weapons 0 = Laser, 1 = Missile
        int numMiss;                            //Number of missiles ship has left              

        public Ship(Vector3 location)
            : this(location, 100, 100, 100, 1, 20, 10, new double[] {.5, .5, .5})
        {
            Name = "MovementShip";
        }

        public Ship(Vector3 location, int maxHull, int maxShield, int maxEnergy, int numMissiles, int lsrDmg, int missDmg, double[] eff) 
            : base(location)
        {
            //Fill hull and shields to max.
            hull[0] = hull[1] = maxHull;
            shield[0] = shield[1] = maxShield;

            //Start energy at zero. We will recharge energy starting on turn 1.
            energy[0] = 0;
            energy[1] = maxEnergy;

            //Set up our weapons
            numMiss = numMissiles;
            dmg[0] = lsrDmg;
            dmg[1] = missDmg;

            
        }

        public override void Load()
        {
            //Set up our rendering options.
            Model = GraphicsManager.Content.Load<Model>("Ship");
            
            Scale = .25f;
            base.Load();
        }
        public void fireLaser(Ship ship)
        {
            ship.energy[0] -= 40;
            ship.isHit(dmg[0]);
        }

        public void fireMissile(Ship ship) 
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
                Unload();
        }

        public void Generate(int amount_energy) { energy[0] += amount_energy; }

        public override void Unload()
        {
        }
    }
}
