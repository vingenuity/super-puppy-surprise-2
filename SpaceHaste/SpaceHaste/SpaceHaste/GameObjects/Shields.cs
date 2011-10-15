using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceHaste.GameObjects
{
    public class Shields
    {
        //Directions of the shields(front, back, left, right, up, down, non-nautically)
        public enum dir{bow=1, stern=2, port=4, starboard=8, top=16, bottom=32}
        private int dirs;
        private int max;
        private int power;

        public Shields(int maxPower) 
        {
            dirs = 63;
            max = maxPower;
            power = 0;
        }

        public void directToAll()
        {
            dirs = 63;
        }

        public void recharge(int amount)
        {
            power += amount;
        }

        public void directTo(dir direction)
        {
            dirs = (int)direction;
        }
    }
}
