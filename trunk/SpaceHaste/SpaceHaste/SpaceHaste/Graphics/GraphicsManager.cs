#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.Cameras;
using SpaceHaste.Grids;
using SpaceHaste.GameObjects;
using SpaceHaste.Controls;
using SpaceHaste.Maps;
using SpaceHaste.Huds;
using SpaceHaste.Primitives;
#endregion

namespace SpaceHaste.Graphics
{
    /// <summary>
    /// Sample showing how to implement non-photorealistic rendering techniques,
    /// providing a cartoon shader, edge detection, and pencil sketch rendering effect.
    /// </summary>
    public class GraphicsManager : DrawableGameComponent
    {
        public static List<GameObject> GraphicsGameObjects;
        public static List<SuperTerrain> GraphicsSuperTerrain;
        /// <summary>
        /// Position of planet and scale (Draw Coords, Draw Scale) 
        /// </summary>
        public static List<Tuple<Vector3,float>> Planets;
        /// <summary>
        /// Position of Nebula (Draw Coords) 
        /// </summary>
        public static List<Vector3> Nebula;

        public static GraphicsDeviceManager graphics;

        public static ContentManager Content;

      //  GraphicsShaders GraphicsShader;
        public static Random random;
        public static Model Asteroid1;
        public static Model Asteroid2;
        public static Model Asteroid3;
        public static Model Asteroid4;
        public static Model Asteroid5;
        public static Model Asteroid6;

        public static Model TestCube;

        public static Model SkyDome;

        public static Model IcePlanet;
        // Change the model to use our custom cartoon shading effect.
        //static Effect cartoonEffect;

        public GraphicsManager(Game game, GraphicsDeviceManager _graphics)
            : base(game)
        {
            random = new Random();
            
            Planets = new List<Tuple<Vector3, float>>();
            Nebula = new List<Vector3>();
            graphics = _graphics;
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content";
           // GraphicsShader = new GraphicsShaders(graphics);
            GraphicsGameObjects = new List<GameObject>();
            DrawOrder = 10;
        }
        static void InitTestCube()
        {
            TestCube = Content.Load<Model>("models/asteroid1");
            IcePlanet = Content.Load<Model>("icePlanet");
            Missile.Model = Content.Load<Model>("models/missile");
            SkyDome = Content.Load<Model>("models/skydome");
           // GraphicsShaders.ChangeEffectUsedByModel(TestCube, cartoonEffect);
        }

        public static void AddGameObject(GameObject gameObject)
        {
            GraphicsGameObjects.Add(gameObject);
          //  GraphicsShaders.ChangeEffectUsedByModel(gameObject.Model, cartoonEffect);
            //GraphicsShader.CreateEffectsForModel(gameObject);
        }
        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            //GraphicsShader.LoadContent();
            //cartoonEffect = GraphicsManager.Content.Load<Effect>("CartoonEffect");
            //for(int i = 0; i<GraphicsGameObjects.Count; i++)
              //  GraphicsShaders.ChangeEffectUsedByModel(GraphicsGameObjects[i].Model, cartoonEffect);
            InitTestCube();
        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
           // GraphicsShader.UnloadContent();
        }



        /// <summary>
        /// Allows the game to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            //GraphicsShader.Update(gameTime);
            UpdateWorldMatricies(gameTime);
        }
        void DrawSkyDome()
        {
            DrawModel(GraphicsManager.SkyDome,
                  Matrix.CreateScale(100f) * Matrix.CreateTranslation(Vector3.Zero),
                  ControlManager.View, ControlManager.Projection);
        }
        void DrawAllModels()
        {
           
            for (int i = 0; i < GraphicsManager.GraphicsGameObjects.Count; i++)
                DrawModel(GraphicsManager.GraphicsGameObjects[i].Model,
                    GraphicsManager.GraphicsGameObjects[i].World,
                    ControlManager.View, ControlManager.Projection);
            for (int i = 0; i < Maps.Map.map.EnvMapObjects.Count; i++)
            {
                if (Maps.Map.map.EnvMapObjects[i].GetTerrain() == Maps.GridCube.TerrainType.asteroid)
                {

                    
                    //ADD SWITCH CASES FOR ASTEROIDS



                    DrawModel(GraphicsManager.TestCube,
                        Matrix.CreateScale(40f) * Matrix.CreateTranslation(Maps.Map.map.EnvMapObjects[i].Center),
                        ControlManager.View, ControlManager.Projection);
                }
                if (Maps.Map.map.EnvMapObjects[i].GetTerrain() == Maps.GridCube.TerrainType.wreck)
                    DrawModel(GraphicsManager.TestCube,
                        Matrix.CreateScale(.2f) * Matrix.CreateTranslation(Maps.Map.map.EnvMapObjects[i].Center),
                        ControlManager.View, ControlManager.Projection);
            }
            for (int i = 0; i < Planets.Count; i++)
            {
                DrawModel(GraphicsManager.IcePlanet,
                    Matrix.CreateScale(15f * Planets[i].Item2) 
                    * Matrix.CreateTranslation(Maps.Map.map.GetCubeAt(Planets[i].Item1).Center-new Vector3(GridCube.GRIDSQUARELENGTH/2, GridCube.GRIDSQUARELENGTH/2, GridCube.GRIDSQUARELENGTH/2)
                        + new Vector3(GridCube.GRIDSQUARELENGTH, GridCube.GRIDSQUARELENGTH, GridCube.GRIDSQUARELENGTH) * (Planets[i].Item2)/2),
                    ControlManager.View, ControlManager.Projection);
            }
            if (Missile.shouldDraw == true)
            {
                Matrix world = Matrix.CreateScale(80,80,20) *
                        Matrix.CreateFromYawPitchRoll(
                        Missile.Direction.Y,
                        Missile.Direction.X,
                        Missile.Direction.Z)
                        * Matrix.CreateTranslation(Missile.DrawPosition);
                DrawModel(Missile.Model, world, ControlManager.View, ControlManager.Projection);
                     
            }
        }
        private void DrawModel(Model model, Matrix world, Matrix View, Matrix Projections)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;

                    // Use the matrices provided by the chase camera
                    effect.View = View;
                    effect.Projection = Projections;
                }
                mesh.Draw();
            }
        }
        private void UpdateWorldMatricies(GameTime gameTime)
        {
            for (int i = 0; i < GraphicsGameObjects.Count; i++)
            {
                if (GraphicsGameObjects[i].AnimationRotation == Vector3.Zero)
                {
                    GraphicsGameObjects[i].World = Matrix.CreateScale(GraphicsGameObjects[i].Scale) *
                        Matrix.CreateFromYawPitchRoll(
                        GraphicsGameObjects[i].IdleAngle.Y + GraphicsGameObjects[i].AnimationRotation.Y,
                        GraphicsGameObjects[i].IdleAngle.X + GraphicsGameObjects[i].AnimationRotation.X,
                        GraphicsGameObjects[i].IdleAngle.Z + GraphicsGameObjects[i].AnimationRotation.Z)
                        * Matrix.CreateTranslation(GraphicsGameObjects[i].DrawPosition);
                }
                else
                {
                    GraphicsGameObjects[i].World = Matrix.CreateScale(GraphicsGameObjects[i].Scale) *
                        Matrix.CreateFromYawPitchRoll(
                        GraphicsGameObjects[i].AnimationRotation.Y,
                        GraphicsGameObjects[i].AnimationRotation.X,
                        GraphicsGameObjects[i].AnimationRotation.Z)
                        * Matrix.CreateTranslation(GraphicsGameObjects[i].DrawPosition);
                }
            }
        }


       
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;
            
          //  device.Clear(Color.Black);

            graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            DrawAllModels();
            //GraphicsShader.Draw(gameTime);

            DPSFParticles.ParticleManager.Instance.Draw(gameTime);
            Hud.Instance.Draw(gameTime);
            //LineManager.Instance.Draw(gameTime);
            //DrawSkyDome();

            base.Draw(gameTime);
        }

    }
}
