using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHaste.GameObjects
{
    public class GameObject
    {
        public Vector3 Position;
       // public Vector3 Velocity;
        public Vector3 Direction;
        public float Speed;
        public float Scale;
        public double Health;
        //Radius is used for physics only Graphics uses Size
        public float Radius;
        public Vector2 Size;
        int Side;
        public Model Model;
        public Matrix World;
        public GameObject()
        {
        }
    }
}
