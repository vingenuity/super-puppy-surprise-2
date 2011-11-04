using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.Graphics;
using SpaceHaste.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.Sounds;

namespace SpaceHaste.GameObjects
{
    public class Ship : GameObject
    {
        public Ship(String name, Vector3 location, Team side)
            : this(location, 100, 100, 13, 1, 20, 10, new double[] {.25, .5, .5})
        {
            Name = name;
            team = side;
        }

        public Ship(Vector3 location, int maxHull, int maxShield, double regeneration, int numMissiles, int lsrDmg, int missDmg, double[] eff) 
            : base(location)
        {
            //Fill hull and shields to max.
            //hull[0] = hull[1] = maxHull;
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

        public double getShipRegen() {
            return regen;
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
            SoundManager.Sounds.PlaySound(SoundEffects.laser);
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
            /*if (hull[0] > damage)
            {
                hull[0] -= damage;
                return;
            }
            //Otherwise, we're dead. Get rid of us.
            else
            {
                SoundManager.Sounds.PlaySound(SoundEffects.explode);
                Unload();
            }*/
        }

        /*public double getHull() {
            return hull[0];
        }

        public double getMaxHull() {
            return hull[1];
        }*/

        public void Generate(int amount_energy) { energy[0] += regen; }

        //public override void Unload() { }
    }
}
