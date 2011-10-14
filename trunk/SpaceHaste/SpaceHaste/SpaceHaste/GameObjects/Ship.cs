using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHaste.GameObjects
{
    public class Ship : GameObject
    {
        double[] hull = new double[2];          //health arr 0=current 1=max
        double[] energy = new double[2];        //energy arr 0=current 1=max
        int numMiss;                            //Number of missiles ship has left                     
        double[] dmg = new double[2];           //Damage done by weapons 0= Laser, 1= Missile
        double[] efficiency= new double[3];     //contains [moveEff,laserEff,shieldsEff] efficiencies track energy usage for specified action
        double eRegen;                          //Amount energy regens per tern                       
        //shield shield;        **Need shield type

        public Ship()
            : this(100, 100, 1, 20, 10, new double[] {.5, .5, .5}, 50){ }

        public Ship(double maxHull,double maxEnergy, int numMissiles, double lsrDmg, double missDmg, double[] eff,double regen) {
            //Hull and energy only initialize max set health and energy to max
            hull[0] = hull[1] = maxHull;

            energy[0] = energy[1] = maxEnergy;

            numMiss = numMissiles;

            dmg[0] = lsrDmg;
            dmg[1] = missDmg;

            Model = GraphicsManager.Content.Load<Model>("Ship");
            GraphicsManager.GraphicsGameObjects.Add(this);
            Scale = .5f;
        }

        public void Laser() { 
            //Laser attack against enemy
        }

        public void Missile() { 
            //Missile attack against enemy
        }

        public void isHitMissile() { 
            //ship is hit by a missile attack
        }

        public void isHitLaser() { 
            //ship is hit by a laser attack
        }

        
        

    }
}
