using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Ship(double maxHull,double maxEnergy, int numMissiles, double lsrDmg, double missDmg, double[3] eff,double regen) {
            //Hull and energy only initialize max set health and energy to max
            hull[0] = hull[1] = maxHull;
            energy[0] = energy[1] = maxEnergy;

            numMiss = numMissiles;

            dmg[0] = lsrDmg;
            dmg[1] = missDmg;
        
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
